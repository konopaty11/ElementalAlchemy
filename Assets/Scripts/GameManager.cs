using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;
    [SerializeField] ResultWindowController resultWindow;
    [SerializeField] Saves saves;
    [SerializeField] GeneralConfig generalConfig;

    bool _isGameStarted;
    bool _isSpawned = false;
    public static bool IsLoose;
    public static bool IsWin;

    float _swipeTime = 0f;

    void OnEnable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
        LatticeController.OnCrafted += OnCrafted;
    }

    void OnDisable()
    {
        InputSwipe.OnSwipeTrack -= OnSwipeTrack;
        LatticeController.OnCrafted -= OnCrafted;
    }

    void Update()
    {
        SwipeTimer();
    }

    void SwipeTimer()
    {
        _swipeTime += Time.deltaTime;
    }

    public void StartGame()
    {
        _isGameStarted = true;
        gameCanvas.SetActive(true);

        if (!_isSpawned)
        {
            _isSpawned = true;
            lattice.SpawnStartSet();
        }
    }

    public void RestartGame()
    {
        IsWin = false;
        IsLoose = false;
        resultWindow.ResetWindow();

        lattice.ClearCells();
    }

    public void ToMenu()
    {
        if (IsLoose || IsWin)
            RestartGame();

        _isGameStarted = false;
        gameCanvas.SetActive(false);
        menuManager.OpenMenu();
    }

    public void LooseGame()
    {
        if (IsLoose) return;

        IsLoose = true;
        resultWindow.Loose();
    }

    public void WinGame()
    {
        if (IsWin) return;

        IsWin = true;
        resultWindow.Win();
    }

    void OnCrafted(CellType _type, int _level)
    {
        if (_type == CellType.Dragon)
            WinGame();
    }

    void OnSwipeTrack(Direction _direction)
    {
        if (IsLoose || IsWin || _swipeTime < generalConfig.SwipeDelay) return;

        _swipeTime = 0f;
        lattice.SwipeHandle(_direction);
    }
}
