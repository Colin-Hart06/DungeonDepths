using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SawAnimation : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer spr;
    public Vector2 startPos;
    public Vector2 endPos;
    public float duration;
    private float startTime;
    private bool isMoving = true;
    
    void Start()
    {
        animator.SetBool("isSpinning", true);
        startTime = Time.time;
    }
    
    public Vector2 SmoothMove(Vector2 start, Vector2 end, float t)
    {
        t = Mathf.Clamp01(t);
        float smoothT = (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
        return Vector2.Lerp(start, end, smoothT);
    }
    
    void Update()
    {
        if (isMoving)
        {
            float t = Mathf.PingPong((Time.time - startTime) / duration, 1f);
            float smoothT = (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
            
            Vector2 newPos = Vector2.Lerp(startPos, endPos, smoothT);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }
    
    public void ResetPosition()
    {
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
        startTime = Time.time; // Reset the timer
        // isMoving = false; // Uncomment to stop movement after reset
    }
}