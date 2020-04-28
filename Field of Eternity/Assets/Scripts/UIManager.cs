using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject gameUIParent;
    private Text playerHPText;
    private Text computerHPText;
    private Text playerSupplyText;
    private bool elementsLoaded = false;
    private int selectedIndex = 0;

    private GameObject pauseUIParent;
    private Button resumeBtn;
    private Button restartBtn;
    private Button quitBtn;

    [SerializeField]
    private GameObject[] unitNumberSelectionPanels;

    private IPauseMenuManager pauseMenuManager;

    public void SetPauseMenuManager(IPauseMenuManager pauseMenuManager)
    {
        this.pauseMenuManager = pauseMenuManager;
    }

    public void LoadElements()
    {
        if(elementsLoaded)
        {
            return;
        }

        gameUIParent = GameObject.Find(UIComponentNames.gameUIParent);
        playerHPText = GameObject.Find(UIComponentNames.playerHPText).GetComponent<Text>();
        computerHPText = GameObject.Find(UIComponentNames.computerHPText).GetComponent<Text>();
        playerSupplyText = GameObject.Find(UIComponentNames.playerSupplyText).GetComponent<Text>();

        pauseUIParent = GameObject.Find(UIComponentNames.pauseUIParent);
        resumeBtn = GameObject.Find(UIComponentNames.resumeGameButton).GetComponent<Button>();
        restartBtn = GameObject.Find(UIComponentNames.restartGameButton).GetComponent<Button>();
        quitBtn = GameObject.Find(UIComponentNames.quitGameButton).GetComponent<Button>();

        pauseUIParent.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        pauseUIParent.SetActive(!pauseUIParent.activeSelf);
        gameUIParent.SetActive(!gameUIParent.activeSelf);

        if(pauseUIParent.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void UpdateUI()
    {
        playerHPText.text = ResourceTracker.PlayerHealthCurrent.ToString();
        computerHPText.text = ResourceTracker.ComputerHealthCurrent.ToString();
        playerSupplyText.text = ResourceTracker.PlayerSupplyCurrent.ToString();

        for (int i = 0; i < unitNumberSelectionPanels.Length; i++)
        {
            Color tempColor = unitNumberSelectionPanels[i].GetComponent<Image>().color;

            if (i == selectedIndex)
            {
                tempColor.a = 1f;
            }
            else
            {
                tempColor.a = 0f;
            }

            unitNumberSelectionPanels[i].GetComponent<Image>().color = tempColor;
        }
    }

    public void increaseSelectedIndex()
    {
        if(selectedIndex == unitNumberSelectionPanels.Length - 1)
        {
            return;
        }

        selectedIndex++;
        UpdateUI();
    }

    public void decreaseSelectedIndex()
    {
        if(selectedIndex == 0)
        {
            return;
        }

        selectedIndex--;
        UpdateUI();
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
}
