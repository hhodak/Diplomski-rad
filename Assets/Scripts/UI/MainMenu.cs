using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    GameStats gameStats;
    Settings settings;
    public AudioMixer audioMixer;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject statsPanel;

    [Header("Settings")]
    public Toggle fullsrceen;
    Resolution[] resolutions;
    public Dropdown resolution;
    public Toggle mute;
    public Slider volume;
    public Dropdown quality;


    private void Awake()
    {
        instance = this;
        settings = new Settings();
        gameStats = new GameStats();
        resolutions = Screen.resolutions;
        GetGameSettings();
        GetGameStats();
        SetDefinedSettingValues();

    }

    #region Play/Quit
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Show/Hide panels
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
    #endregion

    #region GameStats

    void CreateNewGameStatsFile()
    {
        List<GameStats> gameStatsList = new List<GameStats>();
        string jsonData = JsonConvert.SerializeObject(gameStatsList);
        File.WriteAllText(Application.persistentDataPath + "/gamestats.json", jsonData);
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
            CreateNewGameStatsFile();
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

    public void ResetGameStats()
    {
        gameStats = new GameStats();
        if (File.Exists(Application.persistentDataPath + "/gamestats.json"))
            File.Delete(Application.persistentDataPath + "/gamestats.json");
        CreateNewGameStatsFile();
    }
    #endregion

    #region Settings
    void GetGameSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.json"))
        {
            Settings gameSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(Application.persistentDataPath + "/settings.json"));
            settings.fullscreen = gameSettings.fullscreen;
            settings.mute = gameSettings.mute;
            settings.qualityLevel = gameSettings.qualityLevel;
            settings.resolution = gameSettings.resolution;
            settings.volume = gameSettings.volume;
        }
        else
        {
            Settings gameSettings = new Settings();
            settings = gameSettings;
            settings.resolution = resolutions.Length - 1;
            string jsonData = JsonConvert.SerializeObject(gameSettings);
            File.WriteAllText(Application.persistentDataPath + "/settings.json", jsonData);
        }
    }

    public void SaveGameSettings()
    {
        string jsonData = JsonConvert.SerializeObject(settings);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", jsonData);
    }

    void SetDefinedSettingValues()
    {
        resolution.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].ToString();
            options.Add(option);
        }
        resolution.AddOptions(options);

        fullsrceen.isOn = settings.fullscreen;
        SetFullscreen(settings.fullscreen);
        resolution.value = settings.resolution;
        SetResolution(settings.resolution);
        volume.value = settings.volume;
        SetVolume(settings.volume);
        mute.isOn = settings.mute;
        SetMute(settings.mute);
        quality.value = settings.qualityLevel;
        SetQuality(settings.qualityLevel);
    }

    public void SetMute(bool isMute)
    {
        audioMixer.SetFloat("volume", isMute ? -80 : settings.volume);
        settings.mute = isMute;
    }

    public void SetVolume(float volumeValue)
    {
        audioMixer.SetFloat("volume", volumeValue);
        settings.volume = volumeValue;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, fullsrceen.isOn);
        settings.resolution = resolutionIndex;
        resolution.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        settings.fullscreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        settings.qualityLevel = qualityIndex;
        quality.RefreshShownValue();
    }
    #endregion
}
