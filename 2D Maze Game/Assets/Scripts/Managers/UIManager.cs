using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public int time { get; set; } = 0;

    public bool levelFinished { get; set; } = false;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject levelFinishedPanel;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text bestScoreText;

    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private GameObject winSound;

    private GameObject showSummaryLabel;


    void Start()
    {
        levelFinishedPanel.SetActive(false);
        levelFinishedPanel.transform.Find("NewRecordLabel").gameObject.SetActive(false);
        showSummaryLabel = GameObject.Find("ShowSummaryLabel");
        showSummaryLabel.gameObject.SetActive(false);
        bestScoreText.text = PlayerPrefs.GetInt("Score" + LevelParameters.name + "0", 0).ToString();
        pausePanel.SetActive(false);
        StartCoroutine(SetUpTime());
    }

    private void Update()
    {
        if (levelFinished)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (levelFinishedPanel.activeInHierarchy)
                {
                    HideSummary();
                    showSummaryLabel.gameObject.SetActive(true);
                }
                else
                {
                    showSummaryLabel.gameObject.SetActive(false);
                    UnhideSummarry();
                }
            }
        }      
    }

    public IEnumerator SetUpTime()
    {
        while (!levelFinished)
        {         
            yield return new WaitForSeconds(1);
            timeText.text = "Time: " + ++time;
        }     
    }

    public void ShowSummary(int points)
    {
        winSound.GetComponent<AudioSource>().Play();
        levelFinishedPanel.SetActive(true);
        scoreText.text = points.ToString();
        Time.timeScale = 0;
        CheckForBestScore(points);
    }

    public void CloseSummary()
    {
        levelFinishedPanel.SetActive(false);
        Time.timeScale = 1;
        time = 0;
        levelFinished = false;
    }

    public void HideSummary()
    {
        levelFinishedPanel.SetActive(false);
    }

    public void UnhideSummarry()
    {
        levelFinishedPanel.SetActive(true);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        time = 0;
        levelFinished = false;
    }

    public void ReturnToMainMenu()
    {
        CloseSummary();
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void CheckForBestScore(int score)
    {
        if(score > PlayerPrefs.GetInt("Score" + LevelParameters.name + "0", 0))
        {
            levelFinishedPanel.transform.Find("NewRecordLabel").gameObject.SetActive(true);
            bestScoreText.text = score.ToString();         
        }
        DateTime currentDate = DateTime.Now;
        HighScores.AddScore(currentDate.ToString(), score);
    }


}
