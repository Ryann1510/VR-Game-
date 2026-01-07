using UnityEngine;
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
    }

    void Update()
    {
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
    }
}