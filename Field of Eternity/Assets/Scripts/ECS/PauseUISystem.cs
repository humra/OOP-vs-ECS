using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUISystem : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    private EntityManager entityManager;

    private void Start()
    {
        pauseUI.SetActive(false);
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);

        if(pauseUI.activeSelf)
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
        ClearEntities();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        ClearEntities();
        SceneManager.LoadScene("MainMenu");
    }

    private void ClearEntities()
    {
        CombatSystem.player1Entities.Clear();
        CombatSystem.player2Entities.Clear();

        NativeArray<Entity> entities = entityManager.GetAllEntities();

        for (int i = entities.Length - 1; i >= 0; i--)
        {
            entityManager.DestroyEntity(entities[i]);
        }
        entities.Dispose();
    }
}
