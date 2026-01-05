using UnityEngine;
using Unity.XR.CoreUtils;

public class Teleporting : MonoBehaviour
{
    [Header("References")]
    public XROrigin xrOrigin;         // XR Origin root
    public Camera mainCamera;         // Main Camera
    public LayerMask teleportMask;    // Floor layer
    public float maxDistance = 100f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, teleportMask))
            {
                TeleportTo(hit.point);
            }
        }
    }

    void TeleportTo(Vector3 targetPosition)
    {
        // Get the Camera Offset Transform (camera parent inside XR Origin)
        Transform cameraOffsetTransform = xrOrigin.CameraFloorOffsetObject;

        if (cameraOffsetTransform == null)
        {
            Debug.LogWarning("CameraFloorOffsetObject is null! Teleport may fail.");
            return;
        }

        // Calculate XR Origin move amount
        Vector3 offset = mainCamera.transform.position - cameraOffsetTransform.position;

        // Move XR Origin root so camera ends up exactly at target
        xrOrigin.transform.position = targetPosition + offset;

        Debug.Log("Teleported to: " + targetPosition);
    }
}
