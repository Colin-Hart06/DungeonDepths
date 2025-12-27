using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    public gameTimer gt;
    private Vector3 startPosition;
    private Rigidbody2D rig;
    void Start()
    {
        startPosition = transform.position;
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public Grapple grapple;
    void Update()
    {
        
    }
    public SawAnimation sawAnim;
    public KeyCollect kc;
    void OnCollisionEnter2D(Collision2D col)
{
    if (col.gameObject.layer == 3)
    {
        gameObject.transform.position = startPosition;
        rig.velocity = Vector2.zero;
        if(gt != null)
            gt.loseLife();
        grapple.Detatch();
        
        // Reset ALL saws in the scene
        SawAnimation[] allSaws = FindObjectsOfType<SawAnimation>();
        foreach(SawAnimation saw in allSaws)
        {
            saw.ResetPosition();
        }
        kc.numKeysCollected = 0;
        kc.resetKeys();
        //reset key count
        //reset key positions
    }
}
}
