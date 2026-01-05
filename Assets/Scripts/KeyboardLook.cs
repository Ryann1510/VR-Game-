using UnityEngine;

public class KeyboardCameraLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60f;

    void Update()
    {
        // Get input from Arrow Keys (or WASD if you prefer)
        float horizontal = Input.GetAxis("Horizontal"); // Left/Right Arrows
        float vertical = Input.GetAxis("Vertical");     // Up/Down Arrows

        // Calculate rotation based on time and speed
        float rotationX = horizontal * rotationSpeed * Time.deltaTime;
        float rotationY = vertical * rotationSpeed * Time.deltaTime;

        // Apply rotation to the camera
        // We rotate the Y axis (yaw) for horizontal and X axis (pitch) for vertical
        transform.Rotate(Vector3.up, rotationX, Space.World);
        transform.Rotate(Vector3.left, rotationY, Space.Self);
    }
}