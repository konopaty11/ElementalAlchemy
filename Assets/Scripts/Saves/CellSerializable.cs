using System;
using UnityEngine;

[Serializable]
public class CellSerializable
{
    public CellType type;
    public int level;

    public CellSerializable(CellType _type, int _level)
    {
        type = _type;
        level = _level;
    }

    public CellSerializable() { }
}
