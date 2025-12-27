using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timeStopper : MonoBehaviour
{
    // Start is called before the first frame update
    public gameTimer gt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
         Destroy(col.gameObject);
         gt.stopTimer();
         Invoke("endGame",2);

    }
    private void endGame()
    {
        if(gt.speedrunMode==1)
        SceneManager.LoadScene(sceneName:"Try Again Speedrun");
        else
        SceneManager.LoadScene(sceneName:"Back To Menu");
    }
}
