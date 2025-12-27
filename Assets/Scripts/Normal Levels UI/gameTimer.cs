using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameTimer : MonoBehaviour
{
    public bool timerOn;
    public float currentTime;
    public TextMeshProUGUI timerText;
    public RawImage img;
    public GameObject timeStopper;
    public int speedrunMode; //bool 1 = true, 0 = false
    private int livesLeft = 100; // Set default here
    public TextMeshProUGUI lifeText;
    
    void Start()
    {
        if(PlayerPrefs.HasKey("timeValue"))
        {
            currentTime = PlayerPrefs.GetFloat("timeValue");
        }
        if(PlayerPrefs.HasKey("speed"))
        {
            speedrunMode = PlayerPrefs.GetInt("speed");
        }
        if(PlayerPrefs.HasKey("lives"))
        {
            livesLeft = PlayerPrefs.GetInt("lives");
        }
        else
        {
            // Set default if doesn't exist
            livesLeft = 100;
            PlayerPrefs.SetInt("lives", livesLeft);
        }
        
        updateLifeText(); // Update display on start
    }
    
    void Update()
    {
        PlayerPrefs.SetInt("speed", speedrunMode);
        
        if(speedrunMode == 1)
        {
            if(img != null)
                img.enabled = false;
            if(lifeText != null)
                lifeText.enabled = false;
            if(timerOn)
            {
                currentTime += Time.deltaTime;
                PlayerPrefs.SetFloat("timeValue", currentTime);
            }
            updateTimer(currentTime);
        }
        else
        {
            if(img != null)
                img.enabled = true;
            timerText.enabled = false;
            updateLifeText();
        }
        if(SceneManager.GetActiveScene().name!="Out of Lives!"&&livesLeft<=0&&speedrunMode==0)
        SceneManager.LoadScene(sceneName:"Out of Lives!");
    }
    
    void updateTimer(float timer)
    {
        timer += 1;
        float min = Mathf.FloorToInt(timer / 60);
        float sec = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00} : {1:00}", min, sec);
    }
    
    private void updateLifeText()
    {
        if(lifeText != null)
            lifeText.text = "x" + livesLeft;
    }
    
    public void startTimer()
    {
        timerOn = true;
    }
    
    public void pauseUnpause()
    {
        timerOn = !timerOn;
    }
    
    public void stopTimer()
    {
        timerOn = false;
    }
    
    public void nextScene()
    {
        SceneManager.LoadScene(sceneName: "1-1");
    }
    
    public void mainScreen()
    {
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
    
    public void resetTimer()
    {
        currentTime = 0;
        PlayerPrefs.SetFloat("timeValue", currentTime);
    }
    
    public void enabledSpeedRunMode()
    {
        speedrunMode = 1;
    }
    
    public void disableSpeedRunMode()
    {
        speedrunMode = 0;
    }
    
    public void loseLife()
    {
        livesLeft -= 1;
        PlayerPrefs.SetInt("lives", livesLeft);
        PlayerPrefs.Save();
        updateLifeText();
    }
    
    public void setLives(int a)
    {
        livesLeft = a;
        PlayerPrefs.SetInt("lives", livesLeft);
        PlayerPrefs.Save();
        updateLifeText();
    }
    public void arcade()
    {
        SceneManager.LoadScene("Arcade Menu");
    }
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }
}