using UnityEngine;

public class Target : MonoBehaviour
{
<<<<<<< HEAD
    public int pointsValue = 10; // 10 for Gangster, -20 for Civilian
=======
    public int pointsValue = 10; // 10 for Gangster, -10 for Civilian
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
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
<<<<<<< HEAD
=======
            // TODO: Add scoring logic here
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
        }
        else // It's a Civilian
        {
            Debug.Log("Hit Civilian! -1 Miss");
<<<<<<< HEAD
=======
            // TODO: Add Miss penalty logic here
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
        }
        // Retract the target after the action is registered
        RetractTarget();
    }

    private void RetractTarget()
    {
<<<<<<< HEAD
=======
        // TODO: Play retraction animation/sound
>>>>>>> d8d1f59 (independent mouse controll in pretty environment and turning with keyboard)
        Destroy(gameObject);
    }
}