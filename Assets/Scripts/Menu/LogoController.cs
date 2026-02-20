using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoController : MonoBehaviour
{
    [SerializeField] GeneralConfig generalConfig;
    [SerializeField] List<LogoPairSerializable> pairs;

    float _currentTime = 0f;
    float _delay = -1f;

    void Update()
    {
        LogoHandle();
    }

    void LogoHandle()
    {
        if (GameManager.IsGameStarted) return;

        _currentTime += Time.deltaTime;

        if (_delay == -1)
            _delay = UnityEngine.Random.Range(generalConfig.minDelayBetweenMovements, generalConfig.maxDelayBetweenMovements);

        if (_currentTime >= _delay)
        {
            _currentTime = 0f;
            _delay = -1;

            StartCoroutine(ChangeElementsInPair());
        }
    }

    IEnumerator ChangeElementsInPair()
    {
        LogoPairSerializable _pair = pairs[UnityEngine.Random.Range(0, pairs.Count)];

        RectTransform _number = _pair.number;
        RectTransform _image = _pair.image;

        Vector3 _numberPosition = _number.anchoredPosition;
        Vector3 _imagePosition = _image.anchoredPosition;

        float _elapsed = 0f;
        float _duration = generalConfig.durationMovement;
        while (_elapsed < _duration)
        {
            _elapsed += Time.deltaTime;

            _number.anchoredPosition = Vector3.Lerp(_numberPosition, _imagePosition, _elapsed / _duration);
            _image.anchoredPosition = Vector3.Lerp(_imagePosition, _numberPosition, _elapsed / _duration);

            yield return null;
        }
    }
}

[Serializable]
class LogoPairSerializable
{
    public RectTransform number;
    public RectTransform image;
}
