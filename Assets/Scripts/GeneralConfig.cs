using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig", menuName = "Scriptable Objects/GeneralConfig")]
public class GeneralConfig : ScriptableObject
{
    [SerializeField] int startLevel = 1;
    [SerializeField] List<CellType> startCells;

    public int StartLevel => startLevel;
    public List<CellType> StartCells => startCells;
}
