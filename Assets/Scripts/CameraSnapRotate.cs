using UnityEngine;

public class CameraSnapRotate : MonoBehaviour
{
    [SerializeField] private float snapAngle = 15f;

    void Update()
    {
        // Check for "A" key - Rotate Left
        if (Input.GetKeyDown(KeyCode.A))
        {
            SnapRotate(-snapAngle);
        }

        // Check for "D" key - Rotate Right
        if (Input.GetKeyDown(KeyCode.D))
        {
            SnapRotate(snapAngle);
        }
    }

    void SnapRotate(float angle)
    {
        // Rotate around the Y axis (up) in world space
        transform.Rotate(Vector3.up, angle, Space.World);
        
        Debug.Log("Snapped camera by " + angle + " degrees.");
    }
}