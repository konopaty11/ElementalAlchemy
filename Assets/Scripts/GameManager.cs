using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MenuManager menuManager;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;

    bool _isGameStarted;

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
        lattice.SpawnStartSet();
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

    void OnSwipeTrack(Direction _direction)
    {
        lattice.SwipeHandle(_direction);
    }
}
