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

    private GameObject audioSource;


    private void Start()
    {
        audioSource = GameObject.Find("BacgroundMusic");
        SetUpSpeakerIcon();
        HighScores.Initialize();
        QuitRulesPanel();
        QuitSelectLevelPanel();
        QuitHighScoresPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioSource.GetComponent<AudioManager>().isPlaying)
            {
                Mute();
            }
            else
            {
                UnMute();
            }
        }      
    }

    private void SetUpSpeakerIcon()
    {
        if (audioSource.GetComponent<AudioManager>().isPlaying)
        {
            speakerMuted.SetActive(false);
        }
        else
        {
            speaker.SetActive(false);
        }
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
                LevelParameters.name = "E"; 
                LevelParameters.speed = 350; break;
            case "Medium":
                LevelParameters.gridDimensions = new Dimensions(18, 14);
                LevelParameters.name = "M";
                LevelParameters.speed = 200; break;
            case "Hard":
                LevelParameters.gridDimensions = new Dimensions(25, 20);
                LevelParameters.name = "H";
                LevelParameters.speed = 100;  break;
        }
        SceneManager.LoadScene(2);
    }

    public void QuitSelectLevelPanel()
    {
        levelPanel.SetActive(false);
    }

    public void ShowHighScores()
    {
        LevelParameters.name = "E";
        HighScores.LoadScoresTable();
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
            Mute();
        }
        else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("SpeakerMuted"))
        {
            UnMute();
        }
    }

    private void Mute()
    {
        speaker.SetActive(false);
        speakerMuted.SetActive(true);
        audioSource.GetComponent<AudioManager>().Stop();
    }

    private void UnMute()
    {
        speaker.SetActive(true);
        speakerMuted.SetActive(false);
        audioSource.GetComponent<AudioManager>().Play();
    }
}
