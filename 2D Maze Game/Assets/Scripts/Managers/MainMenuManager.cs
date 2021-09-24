using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject rulesPanel;

    [SerializeField]
    private GameObject levelPanel;

    private void Start()
    {
        QuitRulesPanel();
        levelPanel.SetActive(false);
    }
    public void LoadGame()
    {
        levelPanel.SetActive(true);
    }

    public void LoadLevel(string name)
    {
        switch (name)
        {
            case "Easy":
                LevelParameters.gridDimensions = new Dimensions(10, 7); break;
            case "Medium":
                LevelParameters.gridDimensions = new Dimensions(20, 15); break;
            case "Hard":
                LevelParameters.gridDimensions = new Dimensions(30, 25); break;
        }
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

    public void QuitSelectLevelPanel()
    {
        levelPanel.SetActive(false);
    }

    public void HighScores()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
