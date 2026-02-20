using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig", menuName = "Scriptable Objects/GeneralConfig")]
public class GeneralConfig : ScriptableObject
{
    [Header("Input")]
    [SerializeField] float minSwipeLenght = 400f;

    [Header("Logo")]
    public float minDelayBetweenMovements = 1f;
    public float maxDelayBetweenMovements = 3f;
    public float durationMovement = 0.1f;

    [Header("Cells")]
    [SerializeField] float durationCellMove = 0.1f;
    [SerializeField] float durationCellShow = 0.2f;
    [SerializeField] float swipeDelay = 0.3f;
    [SerializeField] int startLevel = 1;
    [SerializeField] int countCellsToAdd = 2;
    [SerializeField] List<CellType> cellsToAdd;
    [SerializeField] List<CellType> startCells;

    public float MinSwipeLenght => minSwipeLenght;

    public float DurationCellMove => durationCellMove;
    public float DurationCellShow => durationCellShow;
    public float SwipeDelay => swipeDelay;
    public int StartLevel => startLevel;
    public int CountCellsToAdd => countCellsToAdd;
    public List<CellType> CellsToAdd => cellsToAdd;
    public List<CellType> StartCells => startCells;
}
