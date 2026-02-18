using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ScoreConfig scoreConfig;
    [SerializeField] Saves saves;

    int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreUpdate();
        }
    }

    int _record;
    public int Record 
    {
        get => _record;
        private set
        {
            _record = value;
            RecordUpdate();
        }
    }

    void OnEnable()
    {
        LatticeController.OnCrafted += OnCrafted;
        Saves.OnDataLoad += OnLoadData;
    }

    void OnDisable()
    {
        LatticeController.OnCrafted -= OnCrafted;
        Saves.OnDataLoad -= OnLoadData;
    }

    void OnLoadData(SaveData _data)
    {
        Score = _data.score;
        Record = _data.record;
    }

    void OnCrafted(CellType _cellType, int _level)
    {
        foreach (ScoreForCraftSerializable _score in scoreConfig.scoresForCraft)
        {
            if (_score.cellType == _cellType && _score.level == _level)
                Score += _score.score;
        }
    }

    void ScoreUpdate()
    {
        if (Score > Record)
        {
            Record = Score;
            RecordUpdate();
        }

        scoreText.text = Score.ToString();

        saves.SaveScore(Score, Record);
    }

    void RecordUpdate()
    {
        maxScoreText.text = Record.ToString();
    }
}
