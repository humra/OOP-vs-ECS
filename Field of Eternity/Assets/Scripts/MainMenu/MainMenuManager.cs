﻿using UnityEngine;
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
}