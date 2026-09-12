using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 3f; // Start from 3
    public bool freezeOnStart = true;
    public bool startCountdown;
    
    void Start()
    {
        
        if(!startCountdown)
        {
        countdownText.enabled = false;
        return;
        }
        if (freezeOnStart)
        {
            player.inputDisabled= true;
            Time.timeScale = 0f; // Freeze everything
        }
        
        StartCoroutine(Countdown());
    }
    public Player player;
    IEnumerator Countdown()
    {
        float timer = countdownTime;
        
        while (timer > 0)
        {
            // Display the countdown
            countdownText.text = Mathf.Ceil(timer).ToString();
            
            // Wait for 1 second using unscaled time (ignores timeScale)
            yield return new WaitForSecondsRealtime(1f);
            
            timer -= 1f;
            player.inputDisabled= true;
        }
        player.inputDisabled = false;
        // Show "GO!" or similar
        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);
        
        // Hide text
        countdownText.gameObject.SetActive(false);
        
        // Unfreeze everything
        Time.timeScale = 1f;
        
        // Optional: Start your game timer here
        gameTimer gameTimerScript = FindAnyObjectByType<gameTimer>();
        if (gameTimerScript != null)
        {
            gameTimerScript.startTimer();
        }
    }
}