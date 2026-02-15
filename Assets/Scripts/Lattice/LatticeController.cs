using UnityEngine;

public class LatticeController : MonoBehaviour
{
    [SerializeField] int sizeLattice = 6;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] PoolController cellPool;
    [SerializeField] GeneralConfig generalConfig;

    Transform[,] _cells;

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
        _cells = new Transform[sizeLattice, sizeLattice];
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

        CellRandomPosition(_cellObject.transform);

        CellController _cell = _cellObject.GetComponent<CellController>();
        _cell.Initialize(_cellType, _level);
    }

    void CellRandomPosition(Transform _cellToPlace)
    {
        Vector2Int _indices = GetRandomAvailableCellIndices();
        _cells[_indices.x, _indices.y] = _cellToPlace;
        _cellToPlace.localPosition = offsetPosition + ConvertVector2BetweenCoordsAndIndices(_indices);

        //int _j = Random.Range(0, sizeLattice);
        //int _i = Random.Range(0, sizeLattice);

        //_cell.localPosition = offsetPosition + new Vector3(_j, _i);
        //_cells[_cells.GetLength(0) - 1 - _i, _j] = _cell;
    }

    Vector2Int GetRandomAvailableCellIndices()
    {
        int _countAvailbaleCells = 0;
        foreach (Transform _cell in _cells)
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
        string _str = "";
        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] == null)
                    _str += "^";
                else
                    _str += "*";
            }
            _str += "\n";
        }
        Debug.Log(_str);

        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            int _jAvailable = 0;
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] != null && _jAvailable != _j)
                {
                    Vector3Int _position = ConvertVector2BetweenCoordsAndIndices(new(_i, _jAvailable));
                    _cells[_i, _j].localPosition = offsetPosition + _position;

                    _cells[_i, _jAvailable] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                    Debug.Log($"{new Vector3(_jAvailable, _i)}");
                    _jAvailable++;
                }
                //MoveCell(_cells[_j, _i], _cells[_j, _iAvailable], new(_j, _iAvailable), _i, ref _iAvailable);
            }
        }

        string _str2 = "";
        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] == null)
                    _str2 += "^";
                else
                    _str2 += "*";
            }
            _str2 += "\n";
        }
        Debug.Log(_str2);

    }

    void MoveCell(Transform _currentCell, Transform _availableCell, Vector3 _newPosition, int _changeCoord, ref int _availableCoord)
    {
        if (_currentCell != null && _availableCoord != _changeCoord)
        {
            _currentCell.localPosition = offsetPosition + _newPosition;

            _availableCell = _currentCell;
            _currentCell = null;
            _availableCoord++;
        }
    }

    Vector3Int ConvertVector2BetweenCoordsAndIndices(Vector2Int _base)
    {
        Vector3Int _converted = new Vector3Int(_base.y, _cells.GetLength(0) - 1 - _base.x);
        return _converted;
    }
}
