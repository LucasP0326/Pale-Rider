using UnityEngine;
using TMPro;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;

public class TimeManager : MonoBehaviour
{
    public float timeScale = 1.0f; // Can be used to speed up/slow down time if desired
    public float currentTime = 0.0f; // Current time in seconds (cumulative)
    public int startHour = 8; // Example: start at 8:00
    public TMP_Text timeDisplay; // Assign in Inspector
    public TMP_Text dayDisplay; // Assign in Inspector

    private int hours;
    private int minutes;
    private int currentDay = 1;

    private const int SECONDS_PER_DAY = 24 * 3600;

    public GameObject directionalLight;
    
    private const int SUNRISE_HOUR = 6;
    private const int SUNSET_HOUR = 20; // 8 PM
    private const int NOON_HOUR = 12;
    private const float NOON_X_ROTATION = 90f;
    private const float SUNSET_X_ROTATION = -90f; // X rotation at sunset

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load time from Articy global variable (in seconds)
        int totalSeconds = ArticyGlobalVariables.Default.GlobalVariables.Time;
        if (totalSeconds <= 0)
            totalSeconds = startHour * 3600; // fallback to startHour if not set

        currentTime = totalSeconds; // currentTime is cumulative seconds since game start

        // Calculate current day (1-based)
        currentDay = Mathf.FloorToInt(currentTime / (float)SECONDS_PER_DAY) + 1;

        UpdateTimeFromCurrentTime();
    }

    // Update is called once per frame
    void Update()
    {
        // Advance time in real time
        currentTime += Time.deltaTime * timeScale;

        // Update derived values (hours, minutes, day) and visuals
        UpdateTimeFromCurrentTime();
    }

    public void LoadTime()
    {
        // Load cumulative seconds and day from Articy. If not set, fall back to startHour.
        int savedSeconds = ArticyGlobalVariables.Default.GlobalVariables.Time;
        int savedDay = ArticyGlobalVariables.Default.GlobalVariables.Day;

        if (savedSeconds > 0)
            currentTime = savedSeconds;
        else
            currentTime = startHour * 3600;

        if (savedDay > 0)
            currentDay = savedDay;
        else
            currentDay = Mathf.FloorToInt(currentTime / (float)SECONDS_PER_DAY) + 1;

        UpdateTimeFromCurrentTime();
    }

    // Update hours, minutes, day and visuals based on currentTime
    private void UpdateTimeFromCurrentTime()
    {
        int totalMinutes = Mathf.FloorToInt(currentTime / 60f);
        int newHours = (totalMinutes / 60) % 24;
        int newMinutes = totalMinutes % 60;

        int newDay = Mathf.FloorToInt(currentTime / (float)SECONDS_PER_DAY) + 1;
        if (newDay != currentDay)
        {
            currentDay = newDay;
            if (dayDisplay != null)
                dayDisplay.text = $"Day {currentDay}";
        }

        hours = newHours;
        minutes = newMinutes;

        if (timeDisplay != null)
            timeDisplay.text = $"{hours:00}:{minutes:00}";

        UpdateLightRotation(hours, minutes);
    }

    private void UpdateLightRotation(int hour, int minute)
    {
        if (directionalLight == null)
            return;

        float xRotation;
        float hourProgress = hour + (minute / 60f);

        if (hourProgress >= SUNRISE_HOUR && hourProgress < SUNSET_HOUR)
        {
            // Daytime: 6 AM to 8 PM
            // 6 AM (6.0) = 0°, 12 PM (12.0) = 90°, 8 PM (20.0) = 180°
            float timeIntoDaylight = hourProgress - SUNRISE_HOUR; // 0 to 14
            float totalDaylight = SUNSET_HOUR - SUNRISE_HOUR;     // 14 hours
            float normalizedProgress = timeIntoDaylight / totalDaylight; // 0 to 1
            
            // Interpolate from 0° (sunrise) to 180° (sunset)
            xRotation = Mathf.Lerp(0f, 180f, normalizedProgress);
        }
        else
        {
            // Nighttime: 8 PM to 6 AM
            if (hourProgress >= SUNSET_HOUR)
            {
                // 8 PM to midnight (20 to 24)
                float timeSinceSunset = hourProgress - SUNSET_HOUR;
                float nightDuration = 24f - SUNSET_HOUR + SUNRISE_HOUR; // 10 hours
                float normalizedProgress = timeSinceSunset / nightDuration;
                
                // Interpolate from 180° to 360° (then reset to 0°)
                xRotation = Mathf.Lerp(180f, 360f, normalizedProgress);
            }
            else
            {
                // Midnight to 6 AM (0 to 6)
                float timeAfterMidnight = hourProgress; // 0 to 6
                float nightDuration = 24f - SUNSET_HOUR + SUNRISE_HOUR; // 10 hours
                float normalizedProgress = (24f - SUNSET_HOUR + timeAfterMidnight) / nightDuration;
                
                // Interpolate from 180° to 360°
                xRotation = Mathf.Lerp(180f, 360f, normalizedProgress);
            }
            
            // Reset to 0 if we've passed 360
            if (xRotation >= 360f)
                xRotation -= 360f;
        }

        directionalLight.transform.eulerAngles = new Vector3(xRotation, 180f, 0f);
    }

    public void AddTime(float seconds)
    {
        currentTime += seconds;
        UpdateTimeFromCurrentTime(); // refresh display immediately
    }

    // Save current time (in seconds) to Articy global variable
    public void SaveTimeToArticy()
    {
        // Write cumulative seconds and 1-based day to Articy globals.
        int currentTimeInt = Mathf.FloorToInt(currentTime);
        int currentDayInt = Mathf.FloorToInt(currentTime / (float)SECONDS_PER_DAY) + 1;
        ArticyGlobalVariables.Default.GlobalVariables.Time = currentTimeInt;
        ArticyGlobalVariables.Default.GlobalVariables.Day = currentDayInt;
    }
}
