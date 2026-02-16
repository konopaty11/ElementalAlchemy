using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;

    bool _isGameStarted;

    void OnEnable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
    }

    public void StartGame()
    {
        _isGameStarted = true;

        gameCanvas.SetActive(true);
        lattice.SpawnStartSet();
    }

    void OnSwipeTrack(Direction _direction)
    {
        lattice.SwipeHandle(_direction);
    }
}
