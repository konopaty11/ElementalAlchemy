using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxScoreText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] ScoreConfig scoreConfig;

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

    public int Record { get; private set; }

    void OnEnable()
    {
        LatticeController.OnCrafted += OnCrafted;
    }

    void OnDisable()
    {
        LatticeController.OnCrafted -= OnCrafted;
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
            maxScoreText.text = Record.ToString();
        }

        scoreText.text = Score.ToString();
    }
}
