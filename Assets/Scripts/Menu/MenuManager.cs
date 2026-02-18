using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameManager gameManager;

    public void Play()
    {
        menuCanvas.SetActive(false);
        gameManager.StartGame();
    }

    public void OpenMenu()
    {
        menuCanvas.SetActive(true);
    }
}
