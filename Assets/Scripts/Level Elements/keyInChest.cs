using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class keyInChest : MonoBehaviour
{
    // Start is called before the first frame update
    public Light2D lightpf;
    private SpriteRenderer spr;
    void Start()
    {
        lightpf.enabled = false;
        spr = GetComponent<SpriteRenderer>();
        KeyData = FindObjectOfType<KeyCollect>();
    }
    public Rigidbody2D rig;
    public Sprite chest;
    public Sprite key;
    public int reqKeys;
    private KeyCollect KeyData;
    void Update()
    {
        if(KeyData.numKeysCollected>=reqKeys)
        {
            spr.sprite = key;
            rig.simulated=true;
            lightpf.enabled = true;
        }
        else
        {
            lightpf.enabled = false;
            spr.sprite = chest;
            rig.simulated = false;
        }
    }
}
