using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.5f;

    void Start()
    {
        // Destroy this GameObject after its lifetime has passed.
        Destroy(gameObject, lifetime);
    }
}