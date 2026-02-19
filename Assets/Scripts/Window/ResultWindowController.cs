using System.Collections;
using TMPro;
using UnityEngine;

public class ResultWindowController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject canvas;
    [SerializeField] float duration = 0.5f;

    [Header("Background")]
    [SerializeField] SpriteRenderer background;
    [SerializeField] Color startBackgroundColor;
    [SerializeField] Color targetLooseBackgroundColor;
    [SerializeField] Color targetWinBackgroundColor;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI looseText;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] Color startTextColor;
    [SerializeField] Color targetLooseTextColor;
    [SerializeField] Color targetWinTextColor;
    [SerializeField] Vector3 startTextPosition;
    [SerializeField] Vector3 targetTextPosition;

    public void Loose()
    {
        canvas.SetActive(true);
        StartCoroutine(ResultCoroutine(targetLooseBackgroundColor, targetLooseTextColor, looseText));
    }

    public void Win()
    {
        canvas.SetActive(true);
        StartCoroutine(ResultCoroutine(targetWinBackgroundColor, targetWinTextColor, winText));
    }

    public void ResetWindow()
    {
        canvas.SetActive(false);
        background.color = startBackgroundColor;

        looseText.color = startTextColor;
        looseText.transform.localPosition = startTextPosition;
        winText.color = startTextColor;
        winText.transform.localPosition = startTextPosition;
    }

    IEnumerator ResultCoroutine(Color _targetBackgroundColor, Color _targetTextColor, TextMeshProUGUI _text)
    {
        float _elapsed = 0f;
        while (_elapsed <= duration)
        {
            _elapsed += Time.deltaTime;

            background.color = Color.Lerp(startBackgroundColor, _targetBackgroundColor, _elapsed / duration);

            _text.color = Color.Lerp(startTextColor, _targetTextColor, _elapsed / duration);
            _text.transform.localPosition = Vector3.Lerp(startTextPosition, targetTextPosition, _elapsed / duration);

            yield return null;
        }
    }
}
