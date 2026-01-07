using UnityEngine;

public class Target : MonoBehaviour
{
    public int pointsValue = 10; // 10 for Gangster, -20 for Civilian
    public bool isGangster = true;
    [SerializeField] private float lifetime = 1.5f; // How long target stays up

    void Start()
    {
        // Set a timer for the target to retract automatically
        Invoke("RetractTarget", lifetime);
    }

    // Called by the shooter script when hit
    public void WasShot()
    {
        CancelInvoke("RetractTarget"); // Stop the timer

        if (isGangster)
        {
            Debug.Log("Hit Gangster! +1 Score");
        }
        else // It's a Civilian
        {
            Debug.Log("Hit Civilian! -1 Miss");
        }
        // Retract the target after the action is registered
        RetractTarget();
    }

    private void RetractTarget()
    {
        Destroy(gameObject);
    }
}