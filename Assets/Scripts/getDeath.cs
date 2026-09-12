using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getDeath : MonoBehaviour
{
    public Death d = null;
    void Start()
    {
        d = FindAnyObjectByType<Death>();
    }
    public void useDeath()
    {
        d.deathActions();
    }
}
