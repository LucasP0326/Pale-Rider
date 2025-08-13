using UnityEngine;
using TMPro;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class TimeManager : MonoBehaviour
{
    public float timeScale = 1.0f; // Can be used to speed up/slow down time if desired
    public float currentTime = 0.0f; // Current time in seconds
    public int startHour = 8; // Example: start at 8:00
    public TMP_Text timeDisplay; // Assign in Inspector
    public TMP_Text dayDisplay; // Assign in Inspector

    private int hours;
    private int minutes;
    private int currentDay = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load time from Articy global variable (assumed to be in minutes)
        int totalMinutes = ArticyGlobalVariables.Default.GlobalVariables.Time;
        if (totalMinutes <= 0)
            totalMinutes = startHour * 60; // fallback to startHour if not set

        currentTime = totalMinutes * 60f; // convert minutes to seconds

        // Calculate current day
        currentDay = (totalMinutes / (24 * 60)) + 1;

        if (dayDisplay != null)
            dayDisplay.text = $"Day {currentDay}";
    }

    // Update is called once per frame
    void Update()
    {
        // Advance time in real time
        currentTime += Time.deltaTime * timeScale;

        // Calculate hours and minutes
        int totalMinutes = Mathf.FloorToInt(currentTime / 60f);
        int newHours = (totalMinutes / 60) % 24;
        minutes = totalMinutes % 60;

        // Check for day rollover
        if (newHours < hours)
        {
            currentDay++;
            if (dayDisplay != null)
                dayDisplay.text = $"Day {currentDay}";
        }
        hours = newHours;

        // Update the UI
        if (timeDisplay != null)
            timeDisplay.text = $"{hours:00}:{minutes:00}";
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
        Update(); // Refresh the display after adding time
    }

    // Save current time (in minutes) to Articy global variable
    public void SaveTimeToArticy()
    {
        int totalMinutes = Mathf.FloorToInt(currentTime / 60f);
        ArticyGlobalVariables.Default.GlobalVariables.Time = totalMinutes;
    }
}
