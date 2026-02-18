using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Events;

public class LatticeController : MonoBehaviour
{
    [SerializeField] int sizeLattice = 6;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] PoolController cellPool;
    [SerializeField] GeneralConfig generalConfig;
    [SerializeField] CraftConfig craftConfig;
    [SerializeField] Saves saves;

    public static UnityAction<CellType, int> OnCrafted;

    CellController[,] _cells;
    List<SaveCellSerializable> _saveCells;

    int _lastAddedCellIndex;

    void OnEnable()
    {
        Saves.OnDataLoad += OnDataLoad;
    }

    void OnDisable()
    {
        Saves.OnDataLoad -= OnDataLoad;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _cells = new CellController[sizeLattice, sizeLattice];
    }

    void OnDataLoad(SaveData _data)
    {
        Debug.Log(_data.cells.Count);
        _saveCells = _data.cells;
    }

    public void SpawnStartSet()
    {
        if (_saveCells.Count == 0)
        {
            foreach (CellType _type in generalConfig.StartCells)
            {
                SpawnCellInRandomPosition(_type, generalConfig.StartLevel);
            }
        }
        else
        {
            foreach (SaveCellSerializable _cell in _saveCells)
            {
                SpawnCellInPosition(_cell.type, _cell.level, _cell.indices);
            }
        }

        //SpawnCellInPosition(CellType.Water, 1, new(4, 0));
        //SpawnCellInPosition(CellType.Water, 1, new(4, 1));

        //SpawnCellInPosition(CellType.Fire, 1, new(4, 2));
        //SpawnCellInPosition(CellType.Fire, 1, new(4, 3));

        //SpawnCellInPosition(CellType.Fire, 1, new(4, 4));
        //SpawnCellInPosition(CellType.Fire, 1, new(4, 5));
    }

    public void ClearCells()
    {
        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            for (int _j = 0; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] != null)
                    DestroyCell(new(_i, _j));
            }
        }
    }

    public void SpawnCellInRandomPosition(CellType _cellType, int _level)
    {
        CellController _cell = SpawnCell(_cellType, _level);

        Vector2Int _indices = GetRandomAvailableCellIndices();
        CellPosition(_cell, _indices);
    }

    public void SpawnCellInPosition(CellType _cellType, int _level, Vector2Int _indices)
    {
        CellController _cell = SpawnCell(_cellType, _level);
        CellPosition(_cell, _indices);
    }

    CellController SpawnCell(CellType _cellType, int _level)
    {
        Debug.Log(_cellType);
        GameObject _cellObject = cellPool.SpawnByPool();

        CellController _cell = _cellObject.GetComponent<CellController>();
        _cell.Initialize(_cellType, _level);

        return _cell;
    }

    void CellPosition(CellController _cellToPlace, Vector2Int _indices)
    {
        _cells[_indices.x, _indices.y] = _cellToPlace;
        _cellToPlace.transform.localPosition = offsetPosition + (Vector3Int)ConvertVector2BetweenCoordsAndIndices(_indices);

        saves.SaveCells(_cells);

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

    public void SwipeHandle(Direction _direction)
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

        AddCellsForSwipe();

        saves.SaveCells(_cells);
    }

    void AddCellsForSwipe()
    {
        int _targetIndex = _lastAddedCellIndex + generalConfig.CountCellsToAdd;
        for (int i = _lastAddedCellIndex; i < _targetIndex; i++)
        {
            CellType _type = generalConfig.CellsToAdd[i %  generalConfig.CellsToAdd.Count];
            SpawnCellInRandomPosition(_type, generalConfig.StartLevel);
        }

        _lastAddedCellIndex = _targetIndex;
    }

    void SwipeLeft()
    {
        int _jStartValue = 0;

        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            int _jAvailable = _jStartValue;
            for (int _j = _jStartValue; _j < _cells.GetLength(1); _j++)
            {
                if (_cells[_i, _j] == null) continue;

                bool _isCrafted = false;
                Vector2Int _targetIndices = new(_i, _j);

                if (_jAvailable != _j)
                {
                    _targetIndices = new(_i, _jAvailable);

                    _cells[_i, _jAvailable] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }

                if (_jAvailable != _jStartValue)
                    _isCrafted = CheckCraft(new(_i, _jAvailable - 1), new(_i, _jAvailable));

                if (_isCrafted)
                    _targetIndices = new(_i, _jAvailable - 1);

                if (_cells[_i, _jAvailable] == null) continue;

                MoveSmoothCell(_cells[_i, _jAvailable].transform, new(_i, _j), _targetIndices);

                if (!_isCrafted)
                    _jAvailable++;
            }
        }
    }

    void SwipeRight()
    {
        int _jStartValue = _cells.GetLength(1) - 1;

        for (int _i = 0; _i < _cells.GetLength(0); _i++)
        {
            int _jAvailable = _jStartValue;
            for (int _j = _jStartValue; _j >= 0; _j--)
            {
                if (_cells[_i, _j] == null) continue;

                bool _isCrafted = false;
                Vector2Int _targetIndices = new(_i, _j);

                if (_jAvailable != _j)
                {
                    _targetIndices = new(_i, _jAvailable);

                    _cells[_i, _jAvailable] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }

                if (_jAvailable != _jStartValue)
                    _isCrafted = CheckCraft(new(_i, _jAvailable + 1), new(_i, _jAvailable));

                if (_isCrafted)
                    _targetIndices = new(_i, _jAvailable + 1);

                if (_cells[_i, _jAvailable] == null) continue;

                MoveSmoothCell(_cells[_i, _jAvailable].transform, new(_i, _j), _targetIndices);

                if (!_isCrafted)
                    _jAvailable--;
            }
        }
    }

    void SwipeTop()
    {
        int _iStartValue = 0;

        for (int _j = 0; _j < _cells.GetLength(1); _j++)
        {
            int _iAvailable = _iStartValue;
            for (int _i = _iStartValue; _i < _cells.GetLength(0); _i++)
            {
                if (_cells[_i, _j] == null) continue;

                bool _isCrafted = false;
                Vector2Int _targetIndices = new(_i, _j);

                if (_iAvailable != _i)
                {
                    _targetIndices = new(_iAvailable, _j);

                    _cells[_iAvailable, _j] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }

                if (_iAvailable != _iStartValue)
                    _isCrafted = CheckCraft(new(_iAvailable - 1, _j), new(_iAvailable, _j));

                if (_isCrafted)
                    _targetIndices = new(_iAvailable - 1, _j);

                if (_cells[_iAvailable, _j] == null) continue;

                MoveSmoothCell(_cells[_iAvailable, _j].transform, new(_i, _j), _targetIndices);

                if (!_isCrafted)
                    _iAvailable++;
            }
        }
    }

    void SwipeBottom()
    {
        int _iStartValue = _cells.GetLength(1) - 1;

        for (int _j = 0; _j < _cells.GetLength(1); _j++)
        {
            int _iAvailable = _iStartValue;
            for (int _i = _iStartValue; _i >= 0; _i--)
            {
                if (_cells[_i, _j] == null) continue;

                bool _isCrafted = false;
                Vector2Int _targetIndices = new(_i, _j);

                if (_iAvailable != _i)
                {
                    _targetIndices = new(_iAvailable, _j);

                    _cells[_iAvailable, _j] = _cells[_i, _j];
                    _cells[_i, _j] = null;
                }

                if (_iAvailable != _iStartValue)
                {
                    _isCrafted = CheckCraft(new(_iAvailable + 1, _j), new(_iAvailable, _j));
                }

                if (_isCrafted)
                    _targetIndices = new(_iAvailable + 1, _j);

                if (_cells[_iAvailable, _j] == null) continue;

                MoveSmoothCell(_cells[_iAvailable, _j].transform, new(_i, _j), _targetIndices);

                if (!_isCrafted)
                    _iAvailable--;
            }
        }
    }

    void MoveCell(Transform _currentCell, Transform _availableCell, Vector2Int _newPosition, int _changeCoord, int _availableCoord)
    {
        if (_currentCell == null) return;

        if (_availableCoord != _changeCoord)
        {
            Vector3Int _position = (Vector3Int)ConvertVector2BetweenCoordsAndIndices(_newPosition);
            _currentCell.localPosition = offsetPosition + _position;

            _availableCell = _currentCell;
            _currentCell = null;
        }

        _availableCoord++;
    }

    void MoveSmoothCell(Transform _cell, Vector2Int _fromIndices, Vector2Int _toIndices)
    {
        Vector3 _startPosition = offsetPosition + (Vector3Int)ConvertVector2BetweenCoordsAndIndices(_fromIndices);
        Vector3 _targetPosition = offsetPosition + (Vector3Int)ConvertVector2BetweenCoordsAndIndices(_toIndices);

        _cell.localPosition = _targetPosition;

        //float _duration = generalConfig.DurationCellMove;
        //float _elapsed = 0f;
        //while (_elapsed < _duration)
        //{
        //    _elapsed += Time.deltaTime;

        //    _cell.localPosition = Vector3.Lerp(_startPosition, _targetPosition, _elapsed / _duration);

        //    yield return null;
        //}

        //for (float t = 0; t <= 1; t += Time.deltaTime)
        //{
        //    _cell.localPosition = Vector3.Lerp(_startPosition, _targetPosition, t);

        //    yield return null;
        //}
        //_cell.localPosition = _targetPosition;
    }

    bool CheckCraft(Vector2Int _cell1Indices, Vector2Int _cell2Indices)
    {
        CellController _cell1 = _cells[_cell1Indices.x, _cell1Indices.y];
        CellController _cell2 = _cells[_cell2Indices.x, _cell2Indices.y];

        foreach (CraftSerializable _craft in craftConfig.crafts)
        {
            //Debug.Log($"{_craft.resultCell.type}" +
            //    $"{_craft.cell_1.type == _cell1.CellType && _craft.cell_1.level == _cell1.Level && _craft.cell_2.type == _cell2.CellType && _craft.cell_2.level == _cell2.Level}" +
            //    $"{_craft.cell_1.type == _cell1.CellType && _craft.cell_2.level == _cell2.Level && _craft.cell_2.type == _cell2.CellType && _craft.cell_1.level == _cell1.Level}");

            if ((_craft.cell_1.type == _cell1.CellType && _craft.cell_1.level == _cell1.Level &&
                _craft.cell_2.type == _cell2.CellType && _craft.cell_2.level == _cell2.Level) 
                ||
                (_craft.cell_1.type == _cell2.CellType && _craft.cell_1.level == _cell2.Level &&
                _craft.cell_2.type == _cell1.CellType && _craft.cell_2.level == _cell1.Level))
            {
                Craft(_craft, _cell1Indices, _cell2Indices);

                Debug.Log($"{_cell1Indices} -- {_cell2Indices} ------ {_craft.resultCell.type}");

                OnCrafted?.Invoke(_craft.resultCell.type, _craft.resultCell.level);
                return true;
            }
        }

        return false;
    }

    void Craft(CraftSerializable _craft, Vector2Int _cell1Indices, Vector2Int _cell2Indices)
    {
        //Debug.Log($"{_cells[_cell1Indices.x, _cell1Indices.y]} --- {_cells[_cell2Indices.x, _cell2Indices.y]}");

        //StartCoroutine(DestroyCell(_cell1Indices));
        //yield return StartCoroutine(DestroyCell(_cell2Indices));

        DestroyCell(_cell1Indices);
        DestroyCell(_cell2Indices);

        //Debug.Log(_craft.resultCell.type);

        SpawnCellInPosition(_craft.resultCell.type, _craft.resultCell.level, _cell1Indices);
    }

    void DestroyCell(Vector2Int _cellIndices)
    {
        GameObject _cellObject = _cells[_cellIndices.x, _cellIndices.y].gameObject;
        _cells[_cellIndices.x, _cellIndices.y] = null;
        saves.SaveCells(_cells);

        //yield return new WaitForSeconds(generalConfig.DurationCellMove);

        cellPool.ReturnToPool(_cellObject);
    }

    Vector2Int ConvertVector2BetweenCoordsAndIndices(Vector2Int _base)
    {
        Vector2Int _converted = new(_base.y, _cells.GetLength(0) - 1 - _base.x);
        return _converted;
    }
}
