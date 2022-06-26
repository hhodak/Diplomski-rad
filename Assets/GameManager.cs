using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                Debug.Log("DEFEAT!");                
                //pause game if true
            }
            if (enemyBuildings == 0)
            {
                gameEnded = true;
                Debug.Log("VICTORY!");
                GameStats.gameWon = true;
                //pause game if true
            }
        }
    }

    public void QuitToMainMenu()
    {
        GameStats.timePlayed = LogController.instance.GetElapsedTime();
        SaveCurrentGameStats();
        UpdateGameStats();
        InputHandler.instance.GamePause();
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
}
