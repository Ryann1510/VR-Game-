using UnityEngine;
using System.Collections; 

public class HybridShooter : MonoBehaviour
{
    // --- VARIABLES ---
    [SerializeField] private float shootDistance = 100f; 
    [SerializeField] private LayerMask hitMask; 

    [Header("Reticle Feedback")]
    [SerializeField] private MeshRenderer crosshairRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material activeMaterial;

    [Header("Visual Projectile Trail")]
    [SerializeField] private GameObject bulletTrailPrefab; 

    
    void Start()
    {

        // VR Desktop Simulation Setup
        // Cursor.lockState = CursorLockMode.Locked; 
        //Cursor.visible = false;

        //if(startPanel != null) startPanel.SetActive(true);
        // Initialize Reticle
        if (crosshairRenderer != null)
        {
            crosshairRenderer.material = defaultMaterial;
        }
    }

    void Update()
    {
        RaycastHit hit; 
        Ray ray = new Ray(transform.position, transform.forward);

        if (GameManager.Instance != null && !GameManager.Instance.isGameActive) 
        {
        crosshairRenderer.material = defaultMaterial;
        return; 
        }

        // --- 1. RETICLE FEEDBACK (Always Runs) ---
        if (Physics.Raycast(ray, out hit, shootDistance, hitMask))
        {
            if (crosshairRenderer != null) crosshairRenderer.material = activeMaterial; 
        }
        else
        {
            if (crosshairRenderer != null) crosshairRenderer.material = defaultMaterial;
        }

        // --- 2. SHOOTING LOGIC (On Mouse Click) ---
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, shootDistance, hitMask))
            {
                // Target hit: Fire the visual trail
                FireTrail(hit.point);
                
                Target targetScript = hit.collider.GetComponent<Target>();
                if (targetScript != null)
                {
                    // >>> SCORING LOGIC INTEGRATION <<<
                    
                    // Check for the ScoreManager instance before attempting to add score
                    if (GameManager.Instance != null)
                    {
                        // Pass the target's point value to the score manager.
                        // The Target script must have a public 'pointsValue' variable set.
                        GameManager.Instance.AddScore(targetScript.pointsValue);
                    }
                    
                    // Execute target destruction logic
                    targetScript.WasShot();
                }
            }
            else
            {
                // Target missed: Fire the trail to max distance
                FireTrail(transform.position + transform.forward * shootDistance);
            }
        }
    }

    // Handles the trail visual effect using the instantiated prefab
    void FireTrail(Vector3 endPoint)
    {
        if (bulletTrailPrefab == null) 
        {
            Debug.LogError("Bullet Trail Prefab not assigned!");
            return;
        }
        
        // Instantiate the trail object at the start position (camera)
        GameObject trailInstance = Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);

        TrailRenderer trail = trailInstance.GetComponent<TrailRenderer>();

        if (trail == null)
        {
            Debug.LogError("BulletTrailPrefab is missing a TrailRenderer component! Cannot render trail.");
            // We proceed anyway since the hit detection is already done
        }

        Vector3 direction = endPoint - transform.position;
        float distance = direction.magnitude;

        // Reposition and stretch the instantiated object to span the distance
        trailInstance.transform.position = transform.position + direction / 2f; 
        trailInstance.transform.rotation = Quaternion.LookRotation(direction); 
        
        // Scale the trail along its local Z-axis (forward)
        trailInstance.transform.localScale = new Vector3(
            trailInstance.transform.localScale.x,
            trailInstance.transform.localScale.y,
            distance
        );
    }
}