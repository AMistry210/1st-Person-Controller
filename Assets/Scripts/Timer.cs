using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isRunning;

    void Start()
    {
        Instance = this;
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;
            DisplayTime(elapsedTime);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
        SaveCompletionTime(Time.time - startTime);
    }

    public float GetCurrentTime() 
    {
        return Time.time - startTime;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SaveCompletionTime(float completionTime)
    {
        // Save completion time using PlayerPrefs or other methods
        PlayerPrefs.SetFloat("CompletionTime", completionTime);
    }
}
