using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Playable()
    {
        SceneManager.LoadScene("Playable");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Demo_OOP()
    {
        SceneManager.LoadScene("Demo_OOP");
    }
}
