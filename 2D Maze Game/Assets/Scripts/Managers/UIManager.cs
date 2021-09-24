using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public int time { get; set; } = 0;

    public bool levelFinished { get; set; } = false;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject levelFinishedPanel;

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

    public void ShowSummary(float points)
    {
        winSound.GetComponent<AudioSource>().Play();
        levelFinishedPanel.SetActive(true);
        scoreText.text = points.ToString();
        Time.timeScale = 0;
    }

    public void HideSummary()
    {
        levelFinishedPanel.SetActive(false);
        Time.timeScale = 1;
        time = 0;
    }

    public void RestartLevel()
    {
        levelManager.RestartLevel();
        HideSummary();
        StartCoroutine(SetUpTime());
    }

    public void ReturnToMainMenu()
    {
        HideSummary();
        SceneManager.LoadScene(0);
    }
}
