using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private KeyCollect KeyData;
    public int numKeys;
    public string sceneName;
    void Start()
    {
        KeyData = player.GetComponent<KeyCollect>();
        tilemap = GetComponentInParent<TilemapRenderer>();
    }

        

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject == player && KeyData.numKeysCollected>=numKeys)
            {
                scoreCounter.score++;
                SceneManager.LoadScene(sceneName);
            }
    }
    TilemapRenderer tilemap;
    
    void Update()
    {
        if(KeyData.numKeysCollected>=numKeys)
        {
            tilemap.enabled = false;
        }
        else
        {
             tilemap.enabled = true;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(sceneName:"Main Menu");
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            KeyData.numKeysCollected++;
        }
    }
}
