using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;
    [SerializeField] LooseWindowController looseWindow;

    bool _isGameStarted;
    bool _isSpawned = false;

    void OnEnable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
    }

    void OnDisable()
    {
        InputSwipe.OnSwipeTrack -= OnSwipeTrack;
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
        lattice.ClearCells();
    }

    public void ToMenu()
    {
        _isGameStarted = false;
        gameCanvas.SetActive(false);
        menuManager.OpenMenu();
    }

    public void LooseGame()
    {
        Debug.Log("loose");
    }

    void OnSwipeTrack(Direction _direction)
    {
        lattice.SwipeHandle(_direction);
    }
}
