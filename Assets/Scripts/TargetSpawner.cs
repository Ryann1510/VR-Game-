using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] GameObject gangsterPrefab;
    [SerializeField] GameObject civilianPrefab;
    [SerializeField] Transform[] spawnPoints; 

    [SerializeField] float spawnInterval = 1.0f;
    private float nextSpawnTime;

    void Start() {
        nextSpawnTime = Time.time;
    }
    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameActive)
        {
        return; 
        }
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnTarget();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnTarget()
    {
        int pointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[pointIndex];

        GameObject targetToSpawn = (Random.value < 0.7f) ? gangsterPrefab : civilianPrefab;

        GameObject newTarget = Instantiate(targetToSpawn, spawnPoint.position, spawnPoint.rotation);

        newTarget.GetComponent<Target>().isGangster = (targetToSpawn == gangsterPrefab);
    }
}