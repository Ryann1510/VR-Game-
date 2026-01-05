using UnityEngine;

public class MouseLookSimulator : MonoBehaviour
{
    // Adjust this value in the Inspector to control rotation speed
    [SerializeField]
    private float sensitivity = 2.0f; 

    // Used to clamp vertical rotation (prevents flipping upside down)
    private float rotationX = 0f;
    
    // Start is called once before the first execution of Update
    void Start()
    {
        // We ensure the cursor is locked here again, as it's critical for this script too.
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void Update()
    {
        // 1. Get Mouse Input
        // Input.GetAxis("Mouse X") tracks horizontal mouse movement
        // Input.GetAxis("Mouse Y") tracks vertical mouse movement
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 2. Handle Vertical Rotation (Looking Up/Down)
        // We use rotationX to store the current vertical angle and clamp it.
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Apply the rotation directly to the camera (or the object this script is on).
        // Vertical rotation should only affect the local X-axis.
        transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);

        // 3. Handle Horizontal Rotation (Looking Left/Right)
        // We apply horizontal rotation to the parent object (Camera Offset or XR Origin)
        // or the camera itself if no parent is suitable.
        // Horizontal rotation should affect the global Y-axis.
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}