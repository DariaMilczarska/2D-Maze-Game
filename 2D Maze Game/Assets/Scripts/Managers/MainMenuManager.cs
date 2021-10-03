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

    [SerializeField]
    private GameObject highScoresPanel;

   
    private void Start()
    {
        HighScores.Initialize();
        QuitRulesPanel();
        QuitSelectLevelPanel();
        QuitHighScoresPanel();
    }
    public void LoadGame()
    {
        levelPanel.SetActive(true);
    }

    public void Rules()
    {
        rulesPanel.SetActive(true);
    }

    public void QuitRulesPanel()
    {
        rulesPanel.SetActive(false);
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

    public void QuitSelectLevelPanel()
    {
        levelPanel.SetActive(false);
    }

    public void ShowHighScores()
    {
        HighScores.LoadScoresTable();
        highScoresPanel.SetActive(true);
    }

    public void QuitHighScoresPanel()
    {
        highScoresPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
