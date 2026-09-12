using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    // Start is called before the first frame update
    new audioManager audio;
    void Start()
    {
        audio = audioManager.instance;
    }
public List<GameObject> keysCollected = new List<GameObject>();
    
    void Update()
    {
        //Debug.Log(numKeysCollected);
    }

    public int numKeysCollected = 0;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 7)
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            keysCollected.Add(col.gameObject);
            numKeysCollected++;
            audio.Play("Key Collect");
        }        
    }
    public void resetKeys()
    {
        foreach(GameObject obj in keysCollected)
        {
            obj.SetActive(true);
        }
    }
}
