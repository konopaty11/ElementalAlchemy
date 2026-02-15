using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameCanvas;
    [SerializeField] LatticeController lattice;

    public void StartGame()
    {
        gameCanvas.SetActive(true);
        lattice.SpawnStartSet();
    }
}
