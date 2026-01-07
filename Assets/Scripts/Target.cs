using UnityEngine;

public class Target : MonoBehaviour
{
    public int pointsValue = 10; 
    public bool isGangster = true;
    [SerializeField] private float lifetime = 1.5f; 

    void Start()
    {
        Invoke("RetractTarget", lifetime);
    }

    public void WasShot()
    {
        CancelInvoke("RetractTarget"); 

        if (isGangster)
        {
            Debug.Log("Hit Gangster! +1 Score");
        }
        else 
        {
            Debug.Log("Hit Civilian! -1 Miss");
        }
        RetractTarget();
    }

    private void RetractTarget()
    {
        Destroy(gameObject);
    }
}