using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] GameObject gangsterPrefab;
    [SerializeField] GameObject civilianPrefab;
    [SerializeField] Transform[] spawnPoints; // Array of points where targets can pop up

    [SerializeField] float spawnInterval = 1.0f;
    private float nextSpawnTime;

    void Start() {
        nextSpawnTime = Time.time;
    }
    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameActive)
        {
        return; // Stop here and don't spawn anything
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

        // Randomly choose between Gangster (70%) and Civilian (30%)
        GameObject targetToSpawn = (Random.value < 0.7f) ? gangsterPrefab : civilianPrefab;

        GameObject newTarget = Instantiate(targetToSpawn, spawnPoint.position, spawnPoint.rotation);

        // Set the isGangster flag in the Target script based on which prefab was chosen
        newTarget.GetComponent<Target>().isGangster = (targetToSpawn == gangsterPrefab);
    }
}