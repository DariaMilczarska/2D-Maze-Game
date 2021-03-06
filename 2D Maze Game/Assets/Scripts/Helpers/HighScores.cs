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
    }

    private static PriorityQueue<string, int> GetCurrentScores()
    {
        PriorityQueue<string, int> tempScores = new PriorityQueue<string, int>();
        for (int i = 0; i < capacity; ++i)
        {
            int score = PlayerPrefs.GetInt("Score" + LevelParameters.name + i, 0);
            String date = PlayerPrefs.GetString("Date" + LevelParameters.name + i, "None").ToString();
            tempScores.Enqueue(date, score);
        }
        return tempScores;
    }

    public static void AddScore(string date, int score)
    {
        scores = GetCurrentScores();
        if (!IsScoreHigherThanLowestInTable(score))
        {
            return;
        }

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
            row.Find("Score").gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score" + LevelParameters.name + i, 0).ToString();
            row.Find("Date").gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("Date" + LevelParameters.name + i, "None").ToString();
        }
    }

    private static void SaveScoresTable()
    {
        for (int i = 0; i < capacity; ++i)
        {        
            PlayerPrefs.SetInt("Score" + LevelParameters.name + i, scores.Get(capacity - i - 1).Value);
            PlayerPrefs.SetString("Date" + LevelParameters.name + i, scores.Get(capacity - i - 1).Key);
        }
        PlayerPrefs.Save();
    }
}