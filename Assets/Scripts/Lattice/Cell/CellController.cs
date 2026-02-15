using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Image icon;
    [SerializeField] Image background;

    [Header("Configs")]
    [SerializeField] RomanArabicNumberConverter _numberConverter;
    [SerializeField] CellConfig cellConfig;

    public CellType CellType { get; private set; }

    int _level;
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            LevelTextUpdate();
        }
    }

    public void Initialize(CellType _type, int _level)
    {
        CellType = _type;
        Level = _level;

        CellConfigSerializable _cellConfig = cellConfig.GetCellConfig(CellType);
        levelText.color = _cellConfig.FontColor;
        icon.sprite = _cellConfig.Icon;
        background.color = _cellConfig.BackgroundColor;
    }

    void LevelTextUpdate()
    {
        levelText.text = _numberConverter.GetRomanNumber(Level);
    }
}
