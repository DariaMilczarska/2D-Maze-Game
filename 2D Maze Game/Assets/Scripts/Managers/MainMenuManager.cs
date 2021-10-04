using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject rulesPanel;

    [SerializeField]
    private GameObject levelPanel;

    [SerializeField]
    private GameObject highScoresPanel;

    [SerializeField]
    private GameObject dropDown;

    [SerializeField]
    private GameObject speaker;

    [SerializeField]
    private GameObject speakerMuted;

    [SerializeField]
    private GameObject audioSource;


    private void Start()
    {
        HighScores.Initialize();
        speakerMuted.SetActive(false);
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
                LevelParameters.gridDimensions = new Dimensions(10, 7);
                LevelParameters.name = "E";  break;
            case "Medium":
                LevelParameters.gridDimensions = new Dimensions(20, 15);
                LevelParameters.name = "M"; break;
            case "Hard":
                LevelParameters.gridDimensions = new Dimensions(30, 25);
                LevelParameters.name = "H"; break;
        }
        SceneManager.LoadScene(1);
    }

    public void QuitSelectLevelPanel()
    {
        levelPanel.SetActive(false);
    }

    public void ShowHighScores()
    {
        highScoresPanel.SetActive(true);
    }

    public void QuitHighScoresPanel()
    {
        HighScores.LoadScoresTable();
        highScoresPanel.SetActive(false);
    }

    public void SelectHighScoreTable()
    {
        switch (dropDown.GetComponent<Dropdown>().value)
        {
            case 0:
                LevelParameters.name = "E"; break;
            case 1:
                LevelParameters.name = "M"; break;
            case 2:
                LevelParameters.name ="H"; break;
        }
        HighScores.LoadScoresTable();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Speaker"))
        {
            speaker.SetActive(false);
            speakerMuted.SetActive(true);
            audioSource.GetComponent<AudioManager>().Stop();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("SpeakerMuted"))
        {
            speaker.SetActive(true);
            speakerMuted.SetActive(false);
            audioSource.GetComponent<AudioManager>().Play();
        }
    }
}
