using UnityEngine;
using System.Collections.Generic;

public class JoySpawner : MonoBehaviour
{
    [Header("Prefab ve Sayı")]
    public GameObject joyPrefab;
    public int joyCount = 10;

    [Header("Spawn Alanı Sınırları")]
    public float startY = -2100f;
    public float maxY = -1050f;
    public float minX = -72f;
    public float maxX = 61f;

    [Header("Minimum / Maksimum Mesafeler")]
    public float minYDistance = 78.6f;
    public float maxYDistance = 120f;
    public float minXDistance = 23f;

    private readonly List<Vector3> spawnedPositions = new();

    void Start()
    {
        if (joyPrefab == null)
        {
            Debug.LogError("❌ Joy prefab atanmadı!");
            return;
        }

        // 🔹 Sayaç toplamını bildir
        if (JoyManager.Instance)
            JoyManager.Instance.InitLevel(joyCount);

        SpawnJoys();
    }

    void SpawnJoys()
    {
        float currentY = startY;
        int created = 0;

        for (int i = 0; i < joyCount; i++)
        {
            float x = Random.Range(minX, maxX);
            float yOffset = Random.Range(minYDistance, maxYDistance);

            currentY = (i == 0) ? startY : currentY + yOffset;
            if (currentY > maxY) break;

            var pos = new Vector3(x, currentY, 0f);
            Instantiate(joyPrefab, pos, Quaternion.identity, transform);
            spawnedPositions.Add(pos);
            created++;
        }

        Debug.Log($"✅ {created} joy yerleştirildi.");
    }
}
