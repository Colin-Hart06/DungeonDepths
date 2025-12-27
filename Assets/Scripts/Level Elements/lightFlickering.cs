using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class lightFlickering : MonoBehaviour
{
    private Light2D light2D;
    
    [Header("Intensity Settings")]
    public float minIntensity = 1.0f;
    public float maxIntensity = 2.0f;
    
    [Header("Smooth Flicker")]
    public float flickerSpeed = 2f; // How fast it changes
    
    private float targetIntensity;
    
    void Start()
    {
        light2D = GetComponent<Light2D>();
        
        if (light2D != null)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
    
    void Update()
    {
        if (light2D != null)
        {
            // Smoothly lerp to target intensity
            light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime * flickerSpeed);
            
            // Pick new target when close enough
            if (Mathf.Abs(light2D.intensity - targetIntensity) < 0.05f)
            {
                targetIntensity = Random.Range(minIntensity, maxIntensity);
            }
        }
    }
}