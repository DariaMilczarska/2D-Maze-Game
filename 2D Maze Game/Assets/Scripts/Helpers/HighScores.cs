using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{

    private int capacity;

    private PriorityQueue<string, int> scores;

    private void Start()
    {
        capacity = transform.childCount;
        scores = GetCurrentScores();
    }

    private PriorityQueue<string, int> GetCurrentScores()
    {
        PriorityQueue<string, int> tempScores = new PriorityQueue<string, int>();
        for (int i = 0; i < capacity; ++i)
        {
            Transform row = transform.GetChild(i);
            int score = Int32.Parse(row.Find("Score").gameObject.GetComponent<Text>().text);
            String date = row.Find("Score").gameObject.GetComponent<Text>().text;
            tempScores.Enqueue(date, score);
        }
        return tempScores;
    }

    public void AddScore(string date, int score)
    {
        if (!IsScoreHigherThanLowestInTable(score))
        {
            return;
        }

        scores.Enqueue(date, score);
        scores.Dequeue();
        SaveScoresTable();
    }

    private bool IsScoreHigherThanLowestInTable(int score)
    {
        int lastScore = scores.Get(capacity - 1).Value;
        if(score > lastScore)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void LoadScoresTable()
    {
        for (int i = 0; i < capacity; ++i)
        {
            Transform row = transform.GetChild(i);
            row.Find("Score").gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score" + i, 0).ToString();
            row.Find("Date").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Date" + i, "None").ToString();
        }
    }

    private void SaveScoresTable()
    {
        for (int i = 0; i < capacity; ++i)
        {
            Transform row = transform.GetChild(capacity - i - 1);
            PlayerPrefs.SetInt("Score" + i ,Int32.Parse(row.Find("Score").gameObject.GetComponent<Text>().text));
            PlayerPrefs.SetString("Date" + i, row.Find("Date").gameObject.GetComponent<Text>().text);
        }
        PlayerPrefs.Save();
    }
}
