using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class timerInGame : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Image timerBar; // Drag your Image component here
    public float levelTime;
    private float timeReduce;
    private float startTime;
    public bool autoStart = true;
    
    private float currentTime;
    private bool isRunning = false;
    
    void Start()
    {
        timeReduce = 1-(scoreCounter.score*0.03f);
        startTime = levelTime * timeReduce;
        currentTime = startTime;
        
        if (autoStart)
        {
            StartTimer();
        }
        
        UpdateTimerDisplay();
        UpdateTimerBar();
    }
    
    void Update()
    {
        if (isRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                Invoke("OnTimerEnd",1);
            }
            
            UpdateTimerDisplay();
            UpdateTimerBar();
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText == null) return;
        
        int seconds = Mathf.FloorToInt(currentTime);
        int milliseconds = Mathf.FloorToInt((currentTime - seconds) * 100);
        
        timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }
    
    void UpdateTimerBar()
    {
        if (timerBar == null) return;
        
        // Calculate percentage of time remaining
        float timePercent = Mathf.Clamp01(currentTime / startTime);
        
        // Update X scale based on percentage
        Vector3 scale = timerBar.transform.localScale;
        scale.x = timePercent;
        timerBar.transform.localScale = scale;
    }
    
    public void StartTimer()
    {
        isRunning = true;
    }
    
    public void StopTimer()
    {
        isRunning = false;
    }
    
    public void ResetTimer()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
        UpdateTimerBar();
    }
    
    public void SetTime(float time)
    {
        startTime = time;
        currentTime = time;
        UpdateTimerDisplay();
        UpdateTimerBar();
    }
    
    void OnTimerEnd()
    {
        // Called when timer reaches 0
        //Debug.Log("Timer ended!");
        // Add your logic here (game over, etc.)
        lifeCounter.lifeCount--;
        lifeCounter.playLifeLossAnimation = true;
        SceneManager.LoadScene("Arcade Menu");
    }
}