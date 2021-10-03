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


    void Start()
    {
        levelFinishedPanel.SetActive(false);
        levelFinishedPanel.transform.Find("NewRecordLabel").gameObject.SetActive(false);
        bestScoreText.text = PlayerPrefs.GetInt("Score0", 0).ToString();
        pausePanel.SetActive(false);
        StartCoroutine(SetUpTime());
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

    public void HideSummary()
    {
        levelFinishedPanel.SetActive(false);
        Time.timeScale = 1;
        time = 0;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        time = 0;
    }

    public void ReturnToMainMenu()
    {
        HideSummary();
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
        if(score > PlayerPrefs.GetInt("Score0", 0))
        {
            bestScoreText.text = score.ToString();
            levelFinishedPanel.transform.Find("NewRecordLabel").gameObject.SetActive(true);
        }
        DateTime currentDate = DateTime.Now;
        HighScores.AddScore(currentDate.ToString(), score);
    }
}
