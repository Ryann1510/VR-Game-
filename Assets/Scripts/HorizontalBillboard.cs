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

        // 1. Calculate the horizontal direction from the target to the camera
        Vector3 targetDirection = mainCameraTransform.position - transform.position;
        targetDirection.y = 0; // Lock the vertical component

        // 2. Calculate the rotation to face the camera horizontally
        Quaternion horizontalRotation = Quaternion.LookRotation(targetDirection);

        // 3. Define the orientation offset (Pitch)
        // This rotation tilts the Plane 90 degrees around the X-axis, making it lie flat.
        Quaternion flatOffset = Quaternion.Euler(90, 0, 0); 

        // 4. Apply the flat orientation first, then the horizontal facing.
        // Multiply the rotations: offset * facing
        transform.rotation = horizontalRotation * flatOffset; 
    }
}