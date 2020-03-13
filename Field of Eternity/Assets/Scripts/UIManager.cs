using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text playerHPText;
    private Text ComputerHPText;
    private bool elementsLoaded = false;
    private int selectedIndex = 0;

    [SerializeField]
    private GameObject[] unitNumberSelectionPanels;

    public void LoadElements()
    {
        if(elementsLoaded)
        {
            return;
        }

        playerHPText = GameObject.Find(UIComponentNames.playerHPText).GetComponent<Text>();
        ComputerHPText = GameObject.Find(UIComponentNames.computerHPText).GetComponent<Text>();
    }

    public void UpdateUI()
    {
        playerHPText.text = ResourceTracker.PlayerHealthCurrent.ToString();
        ComputerHPText.text = ResourceTracker.ComputerHealthCurrent.ToString();

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
}
