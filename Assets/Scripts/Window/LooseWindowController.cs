using System.Collections;
using TMPro;
using UnityEngine;

public class LooseWindowController : MonoBehaviour
{
    [SerializeField] SpriteRenderer background;
    [SerializeField] Color startColor;
    [SerializeField] Color targetColor;
    [SerializeField] float duration = 0.5f;

    public void Loose()
    {
        StartCoroutine(FadeBackground(startColor, targetColor));
    }

    public void SetStartColor()
    {
        background.color = startColor;
    }

    IEnumerator FadeBackground(Color _startColor, Color _targetColor)
    {
        float _elapsed = 0f;
        while (_elapsed <= duration)
        {
            _elapsed += Time.deltaTime;

            background.color = Color.Lerp(_startColor, _targetColor, _elapsed / duration);

            yield return null;
        }
    }
}
