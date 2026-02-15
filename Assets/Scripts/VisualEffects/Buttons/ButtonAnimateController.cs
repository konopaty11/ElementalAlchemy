using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimateController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Sprite pressedSprite;
    [SerializeField] Sprite notPressedSprite;

    Image _image;
    RectTransform _rectTransform;
    float _heightDifference;
    float _pressedYOffset;
    float _notPressedYOffset;

    void Start()
    {
        Initialize();   
    }

    void Initialize()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        _heightDifference = Mathf.Abs(notPressedSprite.textureRect.height - pressedSprite.textureRect.height);
        _pressedYOffset = _rectTransform.offsetMax.y - _heightDifference;
        _notPressedYOffset = _rectTransform.offsetMax.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _image.sprite = pressedSprite;
        _rectTransform.offsetMax = new(_rectTransform.offsetMax.x, _pressedYOffset);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _image.sprite = notPressedSprite;
        _rectTransform.offsetMax = new(_rectTransform.offsetMax.x, _notPressedYOffset);
    }
}
