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

    public void OOP(int intensity)
    {
        switch(intensity)
        {
            case 1:
                SceneManager.LoadScene("Demo_OOP_LowIntensity");
                break;
            case 2:
                SceneManager.LoadScene("Demo_OOP_MediumIntensity");
                break;
            case 3:
                SceneManager.LoadScene("Demo_OOP_HighIntensity");
                break;
            case 4:
                SceneManager.LoadScene("Demo_OOP_ExtremeIntensity");
                break;
        }
    }

    public void ECS(int intensity)
    {
        switch (intensity)
        {
            case 1:
                SceneManager.LoadScene("Demo_ECS_LowIntensity");
                break;
            case 2:
                SceneManager.LoadScene("Demo_ECS_MediumIntensity");
                break;
            case 3:
                SceneManager.LoadScene("Demo_ECS_HighIntensity");
                break;
            case 4:
                SceneManager.LoadScene("Demo_ECS_ExtremeIntensity");
                break;
        }
    }
}
