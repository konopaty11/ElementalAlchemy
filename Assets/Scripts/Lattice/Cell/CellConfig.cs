using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "CellConfig", menuName = "Scriptable Objects/CellConfig")]
public class CellConfig : ScriptableObject
{
    [SerializeField] List<CellConfigSerializable> cellConfigs;

    public CellConfigSerializable GetCellConfig(CellType _type)
    {
        foreach (CellConfigSerializable _cellConfig in cellConfigs)
        {
            if (_cellConfig.Type == _type)
                return _cellConfig;
        }

        Debug.LogError("A cell config is missing");
        return null;
    }
}

[Serializable]
public class CellConfigSerializable
{
    [SerializeField] CellType type;
    [SerializeField] Sprite icon;
    [SerializeField] Color backgroundColor;
    [SerializeField] Color fontColor;

    public CellType Type => type;
    public Sprite Icon => icon;
    public Color BackgroundColor => backgroundColor;
    public Color FontColor => fontColor;
}
