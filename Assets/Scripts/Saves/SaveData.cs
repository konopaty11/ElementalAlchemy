using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public int score;
    public int record;
    public List<SaveCellSerializable> cells = new();
}

[Serializable]
public class SaveCellSerializable : CellSerializable
{
    public Vector2Int indices;

    public SaveCellSerializable(CellType _type, int _level, Vector2Int _indices) : base(_type, _level)
    {
        indices = _indices;
    }

    public SaveCellSerializable() { }
}
