using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    public gameTimer gt;
    private Vector3 startPosition;
    private Rigidbody2D rig;
    new private audioManager audio;
    void Start()
    {
        startPosition = transform.position;
        rig = GetComponent<Rigidbody2D>();
        audio = audioManager.instance;
    }

    // Update is called once per frame
    public Grapple grapple;
    public SawAnimation sawAnim;
    public KeyCollect kc;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 3)
        {
            deathActions();
        }
    }
    public void deathActions()
        {
        audio.Play("spike");
        gameObject.transform.position = startPosition;
        rig.linearVelocity = Vector2.zero;
        if (gt != null)
            gt.loseLife();
        grapple.Detatch();

        // Reset ALL saws in the scene
        SawAnimation[] allSaws = FindObjectsByType<SawAnimation>(FindObjectsSortMode.None);
        foreach (SawAnimation saw in allSaws)
        {
            saw.ResetPosition();
        }
        kc.numKeysCollected = 0;
        kc.resetKeys();
        //reset key count
        //reset key positions
    }
}

