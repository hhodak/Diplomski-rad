using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject statsPanel;

    GameStats gameStats;

    private void Awake()
    {
        instance = this;
        gameStats = new GameStats();
        GetGameStats();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenStatsMenu()
    {
        mainMenuPanel.SetActive(false);
        statsPanel.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        statsPanel.SetActive(false);

        mainMenuPanel.SetActive(true);
    }

    void GetGameStats()
    {
        if (File.Exists(Application.persistentDataPath + "/gamestats.json"))
        {
            List<GameStats> gameStatsList = JsonConvert.DeserializeObject<List<GameStats>>(File.ReadAllText(Application.persistentDataPath + "/gamestats.json"));
            int numOfWins = 0;
            for (int i = 0; i < gameStatsList.Count; i++)
            {
                gameStats.buildingsConstructed += gameStatsList[i].buildingsConstructed;
                gameStats.buildingsDestroyed += gameStatsList[i].buildingsDestroyed;
                gameStats.buildingsLost += gameStatsList[i].buildingsLost;
                gameStats.resourcesMined += gameStatsList[i].resourcesMined;
                gameStats.resourcesSpent += gameStatsList[i].resourcesSpent;
                gameStats.unitsBuilt += gameStatsList[i].unitsBuilt;
                gameStats.unitsKilled += gameStatsList[i].unitsKilled;
                gameStats.unitsLost += gameStatsList[i].unitsLost;
                gameStats.timePlayed += gameStatsList[i].timePlayed;
                if (gameStatsList[i].gameWon)
                {
                    numOfWins++;
                }
            }
            gameStats.winPlayedRatio = numOfWins.ToString() + "/" + gameStatsList.Count.ToString();
            gameStats.timePlayedFormat = SecondsToString(gameStats.timePlayed);
        }
        else
        {
            List<GameStats> gameStatsList = new List<GameStats>();
            string jsonData = JsonConvert.SerializeObject(gameStatsList);
            File.WriteAllText(Application.persistentDataPath + "/gamestats.json", jsonData);
        }
    }

    public string GetPropValueToString(string propName)
    {
        return gameStats.GetType().GetField(propName).GetValue(gameStats).ToString();
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
