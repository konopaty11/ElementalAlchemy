using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig", menuName = "Scriptable Objects/GeneralConfig")]
public class GeneralConfig : ScriptableObject
{
    [Header("Input")]
    [SerializeField] float minSwipeLenght = 400f;

    [Header("Cells")]
    [SerializeField] float durationCellMove = 0.1f;
    [SerializeField] int startLevel = 1;
    [SerializeField] List<CellType> startCells;

    public float MinSwipeLenght => minSwipeLenght;

    public float DurationCellMove => durationCellMove;
    public int StartLevel => startLevel;
    public List<CellType> StartCells => startCells;
}
