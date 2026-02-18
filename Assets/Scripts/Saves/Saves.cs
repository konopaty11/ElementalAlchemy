using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Saves : MonoBehaviour
{
    public static UnityAction<SaveData> OnDataLoad;

    SaveData _data;

    public const string FileName = "UserData.json";
    string _path;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        _path = Path.Combine(Application.persistentDataPath, FileName);
        if (!File.Exists(_path))
        {
            File.Create(_path).Dispose();

            _data = new();
            SaveData();

            OnDataLoad?.Invoke(_data);
            return;
        }

        string _jsonData = File.ReadAllText(_path);
        _data = JsonUtility.FromJson<SaveData>(_jsonData);
        OnDataLoad?.Invoke(_data);
    }

    public void SaveScore(int _currentScore, int _record)
    {
        _data.score = _currentScore;
        _data.record = _record;

        SaveData();
    }

    public void SaveCells(CellController[,] _cellControllers)
    {
        List<CellSerializable> _cells = new();
        foreach (CellController _cellController in _cellControllers)
        {
            CellSerializable _cell = null;
            if (_cellController != null)
            {
                _cell = new(_cellController.CellType, _cellController.Level);
            }

            _cells.Add(_cell);
        }

        SaveData();
    }

    public void SaveData()
    {
        string _jsonData = JsonUtility.ToJson(_data);
        File.WriteAllText(_path, _jsonData);
    }
}
