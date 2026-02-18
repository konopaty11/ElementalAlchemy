using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftConfig", menuName = "Scriptable Objects/CraftConfig")]
public class CraftConfig : ScriptableObject
{
    public List<CraftSerializable> crafts;
}

[Serializable]
public class CraftSerializable
{
    public CellSerializable cell_1;
    public CellSerializable cell_2;
    public CellSerializable resultCell;
}