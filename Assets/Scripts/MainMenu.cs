using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject creditsUI;

    private void Start()
    {
        ButtonClick.StopMusic();
        
    }
    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseTab()
    {
        optionsMenuUI.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsUI.SetActive(false);
    }
}
