using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour
{
    public static LogController instance;
    float elapsedTime = 0;
    public Text elapsedTimeText;
    public Text messageText;
    float messageDuration = 3;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedTimeText.text = SecondsToString();
    }

    string SecondsToString()
    {
        TimeSpan t = TimeSpan.FromSeconds(elapsedTime);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
    }

    public IEnumerator ShowMessage(string message)
    {
        messageText.text = message + " [" + SecondsToString() + "]";
        yield return new WaitForSeconds(messageDuration);
        messageText.text = "";
    }
}
