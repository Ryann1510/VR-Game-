using UnityEngine;

public class MouseLookSimulator : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 2.0f; 

    private float rotationX = 0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void Update()
    {
        // 1. Get Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 2. Handle Vertical Rotation (Looking Up/Down)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y, 0f);

        // 3. Handle Horizontal Rotation (Looking Left/Right)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}