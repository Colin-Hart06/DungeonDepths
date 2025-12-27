using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                unpause();
            }
            else
            pause();
        }
    }
    public void unpause()
    {
        pauseUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }
    public void pause()
    {
        pauseUI.SetActive(true);
        isPaused=true;
        Time.timeScale = 0f;
    }
}
