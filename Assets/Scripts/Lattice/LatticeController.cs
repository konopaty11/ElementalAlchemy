using UnityEngine;

public class LatticeController : MonoBehaviour
{
    [SerializeField] PoolController cellPool;
    [SerializeField] GeneralConfig generalConfig;

    public void SpawnStartSet()
    {
        foreach (CellType _type in generalConfig.StartCells)
        {
            SpawnCell(_type, generalConfig.StartLevel);
        }
    }

    public void SpawnCell(CellType _cellType, int _level)
    {
        GameObject _cellObject = cellPool.SpawnByPool();

        CellController _cell = _cellObject.GetComponent<CellController>();
        _cell.Initialize(_cellType, _level);
    }
}
