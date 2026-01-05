using UnityEngine;
using UnityEngine.UI;

public class HybridShooterScratch : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform reticleTransform; 
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Shooter Script Started. HitMask value: " + hitMask.value);
    }

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.isGameActive) return;

        reticleTransform.position = Input.mousePosition;

        CheckForTarget();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked at Screen Position: " + Input.mousePosition);
            Shoot();
        }
    }

    void CheckForTarget()
    {
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

        // Visualize the ray in the Scene view while playing
        Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.yellow, 1f);

        if (Physics.Raycast(ray, out hit, shootDistance, hitMask))
        {
            Debug.Log("HIT! Object: " + hit.collider.gameObject.name + " on Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            
            FireTrail(Camera.main.transform.position, hit.point);
            
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                Debug.Log("Target script found! Calling WasShot().");
                GameManager.Instance.AddScore(target.pointsValue);
                target.WasShot();
            }
            else
            {
                Debug.LogWarning("Hit object has no 'Target' script attached.");
            }
        }
        else
        {
            Debug.Log("Shot missed everything on the HitMask layer.");
            FireTrail(Camera.main.transform.position, ray.GetPoint(shootDistance));
        }
    }

    void FireTrail(Vector3 start, Vector3 end)
    {
        if (bulletTrailPrefab == null)
        {
            Debug.LogError("No BulletTrailPrefab assigned in Inspector!");
            return;
        }

        GameObject trailInstance = Instantiate(bulletTrailPrefab, start, Quaternion.identity);
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        trailInstance.transform.forward = direction;
        trailInstance.transform.position = start + (direction / 2f);
        
        // Ensure scale is applied correctly based on distance
        Vector3 currentScale = trailInstance.transform.localScale;
        trailInstance.transform.localScale = new Vector3(currentScale.x, currentScale.y, distance);
        
        Destroy(trailInstance, 0.5f);
    }
}