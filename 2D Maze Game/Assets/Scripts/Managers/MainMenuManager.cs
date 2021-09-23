using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject rulesPanel;

    private void Start()
    {
        QuitRulesPanel();
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Rules()
    {
        rulesPanel.SetActive(true);
    }

    public void QuitRulesPanel()
    {
        rulesPanel.SetActive(false);
    }

    public void HighScores()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
