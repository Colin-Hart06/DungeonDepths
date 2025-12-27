using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGame : MonoBehaviour
{

    public string sceneName;
    void OnTriggerEnter2D(Collider2D col)
    {
        SceneManager.LoadScene(sceneName);
            
    }
}
