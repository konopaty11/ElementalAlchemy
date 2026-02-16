using UnityEngine;

public class LatticeController : MonoBehaviour
{
    [SerializeField] int sizeLattice = 6;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] PoolController cellPool;
    [SerializeField] GeneralConfig generalConfig;
    [SerializeField] CraftConfig craftConfig;

    CellController[,] _cells;

    void OnEnable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
    }

    void OnDisable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _cells = new CellController[sizeLattice, sizeLattice];
    }

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

        CellRandomPosition(_cell);
    }

    void CellRandomPosition(CellController _cellToPlace)
    {
        Vector2Int _indices = GetRandomAvailableCellIndices();
        _cells[_indices.x, _indices.y] = _cellToPlace;
        _cellToPlace.transform.localPosition = offsetPosition + ConvertVector2BetweenCoordsAndIndices(_indices);

        //int _j = Random.Range(0, sizeLattice);
        //int _i = Random.Range(0, sizeLattice);

        //_cell.localPosition = offsetPosition + new Vector3(_j, _i);
        //_cells[_cells.GetLength(0) - 1 - _i, _j] = _cell;
    }

    Vector2Int GetRandomAvailableCellIndices()
    {
        int _countAvailbaleCells = 0;
        foreach (CellController _cell in _cells)
        {
            if (_cell == null)
                _countAvailbaleCells++;
        }

        int _randomIndexCell = Random.Range(1, _countAvailbaleCells);
        int _indexAvailbaleCell = 0;

        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] == null)
                {
                    _indexAvailbaleCell++;
                    if (_indexAvailbaleCell == _randomIndexCell)
                        return new(_i, _j);
                }
            }
        }

        return Vector2Int.down;
    }

    void OnSwipeTrack(Direction _direction)
    {
        switch (_direction)
        {
            case Direction.Left:
                SwipeLeft();
                break;
            case Direction.Right:
                SwipeRight();
                break;
            case Direction.Top:
                SwipeTop();
                break;
            case Direction.Bottom:
                SwipeBottom();
                break;
        }
    }

    void SwipeLeft()
    {
        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            int _jAvailable = 0;
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] == null) continue;

                if (_jAvailable != _j)
                {
                    Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(new(_i, _jAvailable));
                    _cells[_i, _j].transform.localPosition = offsetPosition + _position;

                    _cells[_i, _jAvailable] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }
                _jAvailable++;
            }
        }
    }

    void SwipeRight()
    {
        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            int _jAvailable = _cells.GetLength(1) - 1;
            for (int _j = _cells.GetLength(1) - 1; _j >= 0; _j--)
            {
                if (_cells[_i, _j] == null) continue;

                if (_jAvailable != _j)
                {
                    Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(new(_i, _jAvailable));
                    _cells[_i, _j].transform.localPosition = offsetPosition + _position;

                    _cells[_i, _jAvailable] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }
                _jAvailable--;
            }
        }
    }

    void SwipeTop()
    {
        for (int _j = 0; _j < _cells.GetLength(1); _j++)
        {
            int _iAvailable = 0;
            for (int _i = 0; _i < _cells.GetLength(0); _i++)
            {
                if (_cells[_i, _j] == null) continue;

                if (_iAvailable != _i)
                {
                    Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(new(_iAvailable, _j));
                    _cells[_i, _j].transform.localPosition = offsetPosition + _position;

                    _cells[_iAvailable, _j] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }
                _iAvailable++;
            }
        }
    }

    void SwipeBottom()
    {
        for (int _j = 0; _j < _cells.GetLength(1); _j++)
        {
            int _iAvailable = _cells.GetLength(0) - 1;
            for (int _i = _cells.GetLength(0) - 1; _i >= 0; _i--)
            {
                if (_cells[_i, _j] == null) continue;

                if (_iAvailable != _i)
                {
                    Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(new(_iAvailable, _j));
                    _cells[_i, _j].transform.localPosition = offsetPosition + _position;

                    _cells[_iAvailable, _j] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }
                _iAvailable--;
            }
        }
    }

    void MoveCell(Transform _currentCell, Transform _availableCell, Vector2Int _newPosition, int _changeCoord, int _availableCoord)
    {
        if (_currentCell == null) return;

        if (_availableCoord != _changeCoord)
        {
            Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(_newPosition);
            _currentCell.localPosition = offsetPosition + _position;

            _availableCell = _currentCell;
            _currentCell = null;
        }

        _availableCoord++;
    }

    bool CheckCraft(Vector2Int _cell1Indices, Vector2Int _cell2Indices)
    {
        CellController _cell1 = _cells[_cell1Indices.x, _cell1Indices.y];
        CellController _cell2 = _cells[_cell2Indices.x, _cell2Indices.y];

        foreach (CraftSerializable _craft in craftConfig.crafts)
        {
            if ((_craft.cell_1.type == _cell1.CellType && _craft.cell_1.level == _cell1.Level &&
                _craft.cell_2.type == _cell2.CellType && _craft.cell_2.level == _cell2.Level) 
                ||
                (_craft.cell_1.type == _cell1.CellType && _craft.cell_2.level == _cell2.Level &&
                _craft.cell_2.type == _cell2.CellType && _craft.cell_1.level == _cell1.Level))
            { 
                
            }
        }

        return false;
    }

    void DestroyCell()
    {

    }

    Vector3Int ConvertVector2BetweenCoordsAndIndices(Vector2Int _base)
    {
        Vector3Int _converted = new Vector3Int(_base.y, _cells.GetLength(0) - 1 - _base.x);
        return _converted;
    }
}
