using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class HighScores
{

    private static int capacity;

    private static PriorityQueue<string, int> scores;


    private static GameObject scoresTable;

    public static void Initialize()
    {
        scoresTable = GameObject.Find("/Canvas/HighScoresPanel/ScoreTable");
        capacity = scoresTable.transform.childCount;
        scores = GetCurrentScores();
    }

    private static PriorityQueue<string, int> GetCurrentScores()
    {
        LoadScoresTable();
        PriorityQueue<string, int> tempScores = new PriorityQueue<string, int>();
        for (int i = 0; i < capacity; ++i)
        {
            Transform row = scoresTable.transform.GetChild(i);
            int score = Int32.Parse(row.Find("Score").gameObject.GetComponent<Text>().text);
            String date = row.Find("Date").gameObject.GetComponent<Text>().text;
            tempScores.Enqueue(date, score);
        }
        return tempScores;
    }

    public static void AddScore(string date, int score)
    {
        Debug.Log(scores.Size());
        if (!IsScoreHigherThanLowestInTable(score))
        {
            return;
        }
        Debug.Log(capacity);

        scores.Enqueue(date, score);
        scores.Dequeue();
        SaveScoresTable();
    }

    private static bool IsScoreHigherThanLowestInTable(int score)
    {
        int lastScore = scores.Get(0).Value;
        if (score > lastScore)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static void LoadScoresTable()
    {
        for (int i = 0; i < capacity; ++i)
        {
            Transform row = scoresTable.transform.GetChild(i);
            row.Find("Score").gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score" + i, 0).ToString();
            row.Find("Date").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Date" + i, "None").ToString();
        }
    }

    private static void SaveScoresTable()
    {
        for (int i = 0; i < capacity; ++i)
        {        
            PlayerPrefs.SetInt("Score" + i, scores.Get(capacity - i - 1).Value);
            PlayerPrefs.SetString("Date" + i, scores.Get(capacity - i - 1).Key);
        }
        PlayerPrefs.Save();
    }
}