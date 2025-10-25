using UnityEngine;

public class SpawnDirector : MonoBehaviour
{
    [Header("Düşman Prefab'ları")]
    public GameObject[] enemyPrefabs;     // Enemy1, Enemy2, Enemy3 ... buraya sürükle
    public Transform parentForEnemies;    // (ops.) Hierarchy temizliği için container

    [Header("Spawn Sıklığı")]
    public float spawnInterval = 3f;      // Kaç saniyede bir
    public int maxAlive = 0;              // 0 ise limitsiz, >0 ise aynı anda en fazla şu kadar

    [Header("Spawn Alanı (Dünya Koordinatı)")]
    public float minX = -350f;
    public float maxX = -250f;
    public float minY = 256f;
    public float maxY = 660f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            TrySpawn();
        }
    }

    private void TrySpawn()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("SpawnDirector: enemyPrefabs boş!");
            return;
        }

        // Maksimum canlı düşman limiti (opsiyonel)
        if (maxAlive > 0)
        {
            int alive = (parentForEnemies != null) ? parentForEnemies.childCount : CountAliveByTag();
            if (alive >= maxAlive) return;
        }

        // Rastgele prefab + pozisyon
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(x, y, 0f);

        // Üret
        Instantiate(prefab, pos, Quaternion.identity, parentForEnemies);
    }

    // parent kullanmıyorsan ve Tag=Enemy ise sayım için
    private int CountAliveByTag()
    {
        var all = GameObject.FindGameObjectsWithTag("Enemy");
        return all.Length;
    }

    // Sahne içinde alanı görsel olarak çizmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
