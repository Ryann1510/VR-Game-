using UnityEngine;
using Unity.XR.CoreUtils;

public class Teleporting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private Camera mainCamera;

    [Header("Teleport Settings")]
    [SerializeField] private float floorY = 0f;
    [SerializeField] private float maxDistance = 100f;

    [SerializeField] private Collider floorCollider; // assign your floor here

    void Start()
    {
        if (xrOrigin == null)
            Debug.LogError("Teleporting: XR Origin not assigned!");

        if (mainCamera == null)
            Debug.LogError("Teleporting: Main Camera not assigned!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryTeleport();
        }
    }

    void TryTeleport()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane floorPlane = new Plane(Vector3.up, new Vector3(0f, floorY, 0f));
        if (!floorPlane.Raycast(ray, out float enter))
            return;

        Vector3 hitPoint = ray.GetPoint(enter);
        hitPoint.y = floorY;

        // Clamp to floor collider bounds
        if (floorCollider != null)
        {
            Vector3 min = floorCollider.bounds.min;
            Vector3 max = floorCollider.bounds.max;

            hitPoint.x = Mathf.Clamp(hitPoint.x, min.x, max.x);
            hitPoint.z = Mathf.Clamp(hitPoint.z, min.z, max.z);
        }

        Vector3 flatCameraPos = new Vector3(mainCamera.transform.position.x, floorY, mainCamera.transform.position.z);
        if (Vector3.Distance(flatCameraPos, hitPoint) > maxDistance)
            return;

        TeleportXR(hitPoint);
    }


    void TeleportXR(Vector3 targetPosition)
    {
        Transform cameraOffset = xrOrigin.CameraFloorOffsetObject.transform;

        Vector3 offset = mainCamera.transform.position - cameraOffset.position;
        xrOrigin.transform.position = targetPosition + offset;

        Debug.Log("Teleported to: " + targetPosition);
    }
}
