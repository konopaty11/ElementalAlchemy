using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;

    void OnEnable()
    {
        InputSwipe.OnSwipeTrack += OnSwipeTrack;
    }

    public void StartGame()
    {
        gameCanvas.SetActive(true);
        lattice.SpawnStartSet();
    }

    void OnSwipeTrack(Direction _direction)
    {
        Debug.Log(_direction);
    }
}
