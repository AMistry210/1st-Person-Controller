using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI scoreboardText;

    private List<ScoreboardEntry> scoreboardEntries = new List<ScoreboardEntry>();

    void Start()
    {
        // Load scoreboard entries from PlayerPrefs
        LoadScoreboard();
        SortScoreboard(); // Sort the scoreboard entries
        DisplayScoreboard();
    }

    void LoadScoreboard()
    {
        string json = PlayerPrefs.GetString("Scoreboard", "[]");
        scoreboardEntries = JsonUtility.FromJson<List<ScoreboardEntry>>(json);
    }

    void SaveScoreboard()
    {
        string json = JsonUtility.ToJson(scoreboardEntries);
        PlayerPrefs.SetString("Scoreboard", json);
        PlayerPrefs.Save();
    }

    void DisplayScoreboard()
    {
        scoreboardText.text = "";
        foreach (var entry in scoreboardEntries)
        {
            float minutes = Mathf.FloorToInt(entry.completionTime / 60);
            float seconds = Mathf.FloorToInt(entry.completionTime % 60);

            scoreboardText.text += entry.playerName + ": " + string.Format("{0:00}:{1:00}\n", minutes, seconds);
        }
    }

    public void SaveCompletionTime(float completionTime)
    {
        ScoreboardEntry newEntry = new ScoreboardEntry();
        newEntry.playerName = nameInputField.text;
        newEntry.completionTime = completionTime;
        scoreboardEntries.Add(newEntry);
        SortScoreboard(); // Sort the scoreboard entries
        SaveScoreboard();
        DisplayScoreboard();
    }

    void SortScoreboard()
    {
        scoreboardEntries.Sort((x, y) => x.completionTime.CompareTo(y.completionTime));
    }
}





