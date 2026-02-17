using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Scriptable Objects/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    public List<ScoreForCraftSerializable> scoresForCraft;
}

[Serializable]
public class ScoreForCraftSerializable
{
    public CellType cellType;
    public int level;
    public int score;
}
