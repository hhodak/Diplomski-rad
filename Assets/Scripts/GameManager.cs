using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameStats GameStats;
    public GameObject endScreenPanel;
    int playerBuildings;
    int enemyBuildings;
    static bool gameEnded = false;

    private void Awake()
    {
        instance = this;
        gameEnded = false;
        GameStats = new GameStats();
        GetNumberOfPlayerBuildings();
        GetNumberOfEnemyBuildings();
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (playerBuildings == 0)
            {
                gameEnded = true;
                AudioManager.instance.PlaySound("GameLost");
                ShowEndGameScreen(false);
            }
            if (enemyBuildings == 0)
            {
                gameEnded = true;
                AudioManager.instance.PlaySound("GameWon");
                GameStats.gameWon = true;
                ShowEndGameScreen(true);
            }
        }
    }

    public void QuitToMainMenu()
    {
        GameStats.timePlayed = LogController.instance.GetElapsedTime();
        SaveCurrentGameStats();
        UpdateGameStats();
        InputHandler.instance.GamePause(true);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    void SaveCurrentGameStats()
    {
        string jsonData = JsonUtility.ToJson(GameStats, true);
        File.WriteAllText(Application.persistentDataPath + "/lastgamestats.json", jsonData);
    }

    void UpdateGameStats()
    {
        if (File.Exists(Application.persistentDataPath + "/gamestats.json"))
        {
            List<GameStats> gameStatsList = JsonConvert.DeserializeObject<List<GameStats>>(File.ReadAllText(Application.persistentDataPath + "/gamestats.json"));
            gameStatsList.Add(GameStats);
            string jsonData = JsonConvert.SerializeObject(gameStatsList);
            File.WriteAllText(Application.persistentDataPath + "/gamestats.json", jsonData);
        }
        else
        {
            List<GameStats> gameStatsList = new List<GameStats>();
            gameStatsList.Add(GameStats);
            string jsonData = JsonConvert.SerializeObject(gameStatsList);
            File.WriteAllText(Application.persistentDataPath + "/gamestats.json", jsonData);
        }
    }

    void GetNumberOfPlayerBuildings()
    {
        playerBuildings += GameObject.Find("PlayerCommandCenter").transform.childCount;
        playerBuildings += GameObject.Find("PlayerBarracks").transform.childCount;
    }

    void GetNumberOfEnemyBuildings()
    {
        enemyBuildings += GameObject.Find("EnemyCommandCenter").transform.childCount;
        enemyBuildings += GameObject.Find("EnemyBarracks").transform.childCount;
    }

    public void BuildingDestroyed(bool isPlayer)
    {
        if (isPlayer)
            playerBuildings--;
        else
            enemyBuildings--;
    }

    void ShowEndGameScreen(bool isPlayerVictory)
    {
        GameStats.timePlayed = LogController.instance.GetElapsedTime();
        GameStats.timePlayedFormat = SecondsToString(GameStats.timePlayed);
        InputHandler.instance.GamePause(true);
        endScreenPanel.SetActive(true);

        Text victoryText = endScreenPanel.transform.GetChild(0).GetComponent<Text>();
        if (isPlayerVictory)
        {
            victoryText.text = "VICTORY";
            victoryText.color = Color.green;
        }
        else
        {
            victoryText.text = "DEFEAT";
            victoryText.color = Color.red;
        }
    }

    public string GetPropValueToString(string propName)
    {
        return GameStats.GetType().GetField(propName).GetValue(GameStats).ToString();
    }

    string SecondsToString(float elapsedTime)
    {
        TimeSpan t = TimeSpan.FromSeconds(elapsedTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
    }
}
