using UnityEngine;

public class HorizontalBillboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (mainCameraTransform == null) return;

        // Calculate the horizontal direction from the target to the camera
        Vector3 targetDirection = mainCameraTransform.position - transform.position;
        targetDirection.y = 0; // Lock the vertical component

        // Calculate the rotation to face the camera horizontally
        Quaternion horizontalRotation = Quaternion.LookRotation(targetDirection);

        // Define the orientation offset (Pitch)
        Quaternion flatOffset = Quaternion.Euler(90, 0, 0); 

        // Apply the flat orientation first, then the horizontal facing.
        transform.rotation = horizontalRotation * flatOffset; 
    }
}