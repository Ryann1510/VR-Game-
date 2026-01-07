using UnityEngine;
<<<<<<< HEAD
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
=======
using UnityEngine.UI;

public class HybridShooterScratch : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag the 'Reticle_UI' RectTransform here.")]
    [SerializeField] private RectTransform reticleTransform; 
    [Tooltip("Drag the 'Reticle_UI' Image component here.")]
    [SerializeField] private Image reticleImage; 

    [Header("Reticle Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color targetColor = Color.red;

    [Header("Shooting Settings")]
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private GameObject bulletTrailPrefab;

    void Start()
    {
        // Ensure the cursor is visible so you can aim with it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
    }

    void Update()
    {
<<<<<<< HEAD
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
=======
        // 1. Safety check for GameState
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive)
        {
            if(reticleImage != null) reticleImage.enabled = false;
            return;
        }
        
        if(reticleImage != null) reticleImage.enabled = true;

        // 2. Move UI to Mouse Position 
        // This is screen-space, so it stays 'on the glass' of your goggles
        if (reticleTransform != null)
        {
            reticleTransform.position = Input.mousePosition;
        }

        // 3. Target Detection for Color Feedback
        CheckForTarget();

        // 4. Shooting Input
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void CheckForTarget()
    {
        // Project a ray from the camera through the UI position
        Ray ray = Camera.main.ScreenPointToRay(reticleTransform.position);
        
        if (Physics.Raycast(ray, shootDistance, hitMask))
        {
            reticleImage.color = targetColor;
        }
        else
        {
            reticleImage.color = defaultColor;
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(reticleTransform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance, hitMask))
        {
            FireTrail(Camera.main.transform.position, hit.point);
            
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                GameManager.Instance.AddScore(target.pointsValue);
                target.WasShot();
            }
        }
        else
        {
            FireTrail(Camera.main.transform.position, ray.GetPoint(shootDistance));
        }
    }

    void FireTrail(Vector3 start, Vector3 end)
    {
        if (bulletTrailPrefab == null) return;
        
        GameObject trailInstance = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        
        // This logic assumes your prefab has a TrailRenderer or similar setup
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        trailInstance.transform.forward = direction;
        
        // If using the scaling-cube method from your previous script:
        trailInstance.transform.position = start + (direction / 2f);
        Vector3 scale = trailInstance.transform.localScale;
        trailInstance.transform.localScale = new Vector3(scale.x, scale.y, distance);
        
        // Destroy the trail after a short time
        Destroy(trailInstance, 0.5f);
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
    }
}