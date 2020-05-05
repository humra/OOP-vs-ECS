using UnityEngine;

public class UIManager_Demo_OOP : MonoBehaviour
{
    private bool elementsLoaded = false;
    private GameObject pauseUIParent;

    private IPauseMenuManager pauseMenuManager;

    public void SetPauseMenuManager(IPauseMenuManager pauseMenuManager)
    {
        this.pauseMenuManager = pauseMenuManager;
    }

    public void LoadElements()
    {
        if (elementsLoaded)
        {
            return;
        }

        pauseUIParent = GameObject.Find(UIComponentNames.pauseUIParent);
        pauseUIParent.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        pauseUIParent.SetActive(!pauseUIParent.activeSelf);

        if (pauseUIParent.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void ResumeGame()
    {
        TogglePauseMenu();
    }

    public void RestartGame()
    {
        pauseMenuManager.RestartGame();
    }

    public void QuitGame()
    {
        pauseMenuManager.QuitGame();
    }

    public void MainMenu()
    {
        pauseMenuManager.MainMenu();
    }
}
