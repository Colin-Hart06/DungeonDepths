using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timeStopper : MonoBehaviour
{
    // Start is called before the first frame update
    public gameTimer gt;
    new private audioManager audio;
    private void Awake()
    {
        audio = audioManager.instance;
        audio.FadeOut("Dungeon Theme", 2);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
         Destroy(col.gameObject);
         gt.stopTimer();
         Invoke("endGame",2);

    }
    private void endGame()
    {
        audio.StopPlaying("Dungeon Theme");
        if(gt.speedrunMode==1)
        SceneManager.LoadScene(sceneName:"Try Again Speedrun");
        else
        SceneManager.LoadScene(sceneName:"Back To Menu");
    }
}
