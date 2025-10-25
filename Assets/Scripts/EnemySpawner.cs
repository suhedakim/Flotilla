using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    private bool spawning = false;

    void Awake()
    {
        instance = this;
    }

    public void BeginSpawning()
    {
        if (!spawning)
        {
            spawning = true;
            InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
            return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
