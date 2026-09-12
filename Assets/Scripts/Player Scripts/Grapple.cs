using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class Grapple : MonoBehaviour
{
    private Camera cam;
    public LineRenderer lr;
    public GameObject reticle;  // Drag your reticle GameObject here
    public GameObject hookEnd;
    public LayerMask grappleMask;
    public LayerMask ungrappableMask;  // Surfaces that block grapple but show reticle
    public float moveSpeed = 2;
    public float grappleLength = 5;
    public float hookSpeed = 20f;  // Speed at which the hook travels
    public float endpointRadius = 0.5f;  // Distance considered "at endpoint" for jumping
    [Min(1)]
    public int maxPoints = 3;
    //private bool updateHookRotation;
    private Vector2 hitPoint;
    private SpriteRenderer spr;
    public Sprite hookOpen;
    public Sprite hookClosed;

    private Rigidbody2D rig;
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> hookPositions = new List<Vector2>();
    private List<Vector2> targetPositions = new List<Vector2>();
    private List<bool> hooksConnected = new List<bool>();
    private bool isGrappling = false;
    private float lastDetachTime = -999f;
    
    // Track the grappled object and offset for moving targets
    private Transform grappledObject;
    private Vector2 grappleOffset;  // Track when we last detached
    new private audioManager audio;
    private string grappledTag;

    private void Start()
    {
        cam = Camera.main;
        spr = hookEnd.GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        lr.positionCount = 0;
        audio = audioManager.instance;

        if (reticle != null)
        {
            reticle.SetActive(false);
        }
    }

    void Update()
    {
        // Check for valid grapple point and update reticle
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        // Check both surfaces
        RaycastHit2D grappleHit = Physics2D.Raycast(transform.position, direction, grappleLength, grappleMask);
        RaycastHit2D blockedHit = Physics2D.Raycast(transform.position, direction, grappleLength, ungrappableMask);

        // Determine which surface is closer and if we can grapple
        bool canGrapple = false;
        Vector2 reticlePosition = (Vector2)transform.position + direction * grappleLength;

        if (grappleHit.collider != null && blockedHit.collider != null)
        {
            // Both hits - check which is closer
            if (grappleHit.distance < blockedHit.distance)
            {
                // Grappleable surface is closer
                canGrapple = true;
                reticlePosition = grappleHit.point;
            }
            else
            {
                // Ungrappleable surface is blocking
                canGrapple = false;
                reticlePosition = blockedHit.point;
            }
        }
        else if (grappleHit.collider != null)
        {
            // Only grappleable hit
            canGrapple = true;
            reticlePosition = grappleHit.point;
        }
        else if (blockedHit.collider != null)
        {
            // Only ungrappleable hit
            canGrapple = false;
            reticlePosition = blockedHit.point;
        }

        // Update reticle
        if (reticle != null && !pauseMenu.isPaused)
        {
            reticle.SetActive(true);
            reticle.transform.position = reticlePosition;

            if (canGrapple)
            {
                SetReticleColor(Color.white, 1f);
            }
            else if (blockedHit.collider != null)
            {
                SetReticleColor(Color.red, 0.5f);
            }
            else
            {
                SetReticleColor(Color.gray, 0.3f);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canGrapple && grappleHit.collider != null)
            {
                hitPoint = grappleHit.point;

                hookPositions.Clear();
                targetPositions.Clear();
                hooksConnected.Clear();
                points.Clear();

                grappledObject = grappleHit.collider.transform;
                grappleOffset = hitPoint - (Vector2)grappledObject.position;

                hookPositions.Add((Vector2)transform.position);
                targetPositions.Add(hitPoint);
                hooksConnected.Add(false);

                grappledTag = grappleHit.collider.tag; // NEW - store tag at fire time
            }
        }

        // Update hook positions
        for (int i = 0; i < hookPositions.Count; i++)
        {
            if (!hooksConnected[i])
            {
                hookPositions[i] = Vector2.MoveTowards(hookPositions[i], targetPositions[i], hookSpeed * Time.deltaTime);

                if (Vector2.Distance(hookPositions[i], targetPositions[i]) < 0.1f)
                {
                    hookPositions[i] = targetPositions[i];
                    hooksConnected[i] = true;
                    points.Add(targetPositions[i]);

                    if (i == 0)
                    {
                        PlayGrappleHitSound(grappledTag);
                    }
                }
            }
        }

        // Update grapple point if attached to a moving object
        if (hooksConnected.Count > 0 && hooksConnected[0] && grappledObject != null)
        {
            Vector2 newGrapplePoint = (Vector2)grappledObject.position + grappleOffset;
            targetPositions[0] = newGrapplePoint;
            hookPositions[0] = newGrapplePoint;
            if (points.Count > 0)
            {
                points[0] = newGrapplePoint;
            }
        }

        // Pull player with velocity instead of MovePosition to preserve momentum
        if (points.Count > 0)
        {
            isGrappling = true;
            Vector2 moveTo = centriod(points.ToArray());
            float distanceToTarget = Vector2.Distance(transform.position, moveTo);

            // Only pull if we're not already at the grapple point
            if (distanceToTarget > endpointRadius)
            {
                Vector2 pullDirection = (moveTo - (Vector2)transform.position).normalized;
                rig.linearVelocity = pullDirection * moveSpeed;
            }
            else
            {
                rig.linearVelocity = Vector2.zero;
                rig.MovePosition(moveTo);
            }
        }
        else
        {
            isGrappling = false;
        }

        UpdateLineRenderer();


        if (Input.GetKeyDown(KeyCode.Space) && points.Count > 0 && !IsAtEndpoint())
        {
            Detatch();
        }

        //hookEnd Update
        if (lr.positionCount == 2)
        {
            hookEnd.SetActive(true);
            hookEnd.transform.position = lr.GetPosition(1);
            Vector2 dir = (lr.GetPosition(1) - lr.GetPosition(0)).normalized;
            hookEnd.transform.up = dir;
        }
        else
        {
            hookEnd.SetActive(false);
        }
        for (int i = 0; i < hookPositions.Count; i++)
        {
            if (hooksConnected[i])
            {
                spr.sprite = hookClosed;
            }
            else
            {
                spr.sprite = hookOpen;
            }
        }
    }  
    void UpdateLineRenderer()
    {
        lr.positionCount = hookPositions.Count * 2;
        
        for (int i = 0; i < hookPositions.Count; i++)
        {
            lr.SetPosition(i * 2, transform.position);
            lr.SetPosition(i * 2 + 1, hookPositions[i]);
        }
    }

    public void Detatch()
    {
        lr.positionCount = 0;
        points.Clear();
        hookPositions.Clear();
        targetPositions.Clear();
        hooksConnected.Clear();
        lastDetachTime = Time.time;  // Record when we detached
        
        // Reticle will be updated in next Update cycle
    }

    public bool RecentlyDetached(float timeWindow = 0.5f)
    {
        return Time.time - lastDetachTime < timeWindow;
    }

    void SetReticleColor(Color color, float alpha)
    {
        if (reticle == null) return;
        
        Color finalColor = new Color(color.r, color.g, color.b, alpha);
        
        // Try SpriteRenderer with material.color
        SpriteRenderer spr = reticle.GetComponent<SpriteRenderer>();
        if (spr != null && spr.material != null)
        {
            spr.material.color = finalColor;
            return;
        }
        
        // Try MeshRenderer with material.color
        MeshRenderer mesh = reticle.GetComponent<MeshRenderer>();
        if (mesh != null && mesh.material != null)
        {
            mesh.material.color = finalColor;
            return;
        }
        
        // Try UI Image
        UnityEngine.UI.Image img = reticle.GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.color = finalColor;
            return;
        }
        
        // Try all children
        foreach (Transform child in reticle.transform)
        {
            SpriteRenderer childSpr = child.GetComponent<SpriteRenderer>();
            if (childSpr != null && childSpr.material != null)
            {
                childSpr.material.color = finalColor;
            }
            
            MeshRenderer childMesh = child.GetComponent<MeshRenderer>();
            if (childMesh != null && childMesh.material != null)
            {
                childMesh.material.color = finalColor;
            }
            
            UnityEngine.UI.Image childImg = child.GetComponent<UnityEngine.UI.Image>();
            if (childImg != null)
            {
                childImg.color = finalColor;
            }
        }
    }

    public bool IsGrappling()
    {
        return isGrappling;
    }

    public bool IsAtEndpoint()
    {
        if (points.Count > 0)
        {
            Vector2 moveTo = centriod(points.ToArray());
            float distanceToTarget = Vector2.Distance(transform.position, moveTo);
            return distanceToTarget <= endpointRadius;
        }
        return false;
    }

    Vector2 centriod(Vector2[] points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Length;
        return center;
    }

    private void OnDrawGizmos()
    {
        if (cam == null) return;
        
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir);
        
        foreach (Vector2 point in points)
        {
            Gizmos.DrawLine(transform.position, point);
        }
    }
    void PlayGrappleHitSound(string tag)
    {
        if (audio == null) return;

        switch (tag)
        {
            case "Grass":
                audio.Play("Hook Grass");
                break;
            case "Wood":
                audio.Play("Hook Wood");
                break;
            case "Stone":
                audio.Play("Hook Stone");
                break;
            case "Brick":
                audio.Play("Hook Brick");
                break;
            default:
                audio.Play("Hook Brick"); // fallback/default sound
                break;
        }
    }
}