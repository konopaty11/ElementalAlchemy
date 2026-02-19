using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;
    [SerializeField] ResultWindowController resultWindow;

    bool _isGameStarted;
    bool _isSpawned = false;
    bool _isLoose;
    bool _isWin;

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
        _isWin = false;
        _isLoose = false;
        resultWindow.ResetWindow();

        lattice.ClearCells();
    }

    public void ToMenu()
    {
        if (_isLoose || _isWin)
            RestartGame();

        _isGameStarted = false;
        gameCanvas.SetActive(false);
        menuManager.OpenMenu();
    }

    public void LooseGame()
    {
        _isLoose = true;
        resultWindow.Loose();
    }

    public void WinGame()
    {
        _isWin = true;
        resultWindow.Win();
    }

    void OnCrafted(CellType _type, int _level)
    {
        if (_type == CellType.Dragon)
            WinGame();
    }

    void OnSwipeTrack(Direction _direction)
    {
        if (_isLoose || _isWin) return;

        lattice.SwipeHandle(_direction);
    }
}
