using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Tile Ayarları")]
    public GameObject[] tiles;           // 4 adet tile objesi
    public float scrollSpeed = 2f;       // Arka plan kayma hızı
    public float exactTileHeight = 55f;  // Sprite world unit yüksekliği (Scene'de ölçtüğün değerle eşleşmeli)

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogError("Tile dizisi boş!");
            return;
        }

        // Başlangıçta hiçbir şekilde otomatik hizalama yapma — senin sahne düzenin korunur.
        Debug.Log("🎯 ScrollingBackground: Tile pozisyonları manuel olarak korunuyor.");
    }

    void Update()
    {
        // Tüm tile’ları aşağıya kaydır
        foreach (GameObject tile in tiles)
        {
            tile.transform.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        // Kamera altına düşen tile’ları yukarı taşı
        foreach (GameObject tile in tiles)
        {
            float cameraBottomY = mainCam.transform.position.y - mainCam.orthographicSize;
            float tileTopY = tile.transform.position.y + exactTileHeight / 2f;

            if (tileTopY < cameraBottomY)
            {
                float highestY = FindHighestTileY();
                tile.transform.position = new Vector3(
                    tile.transform.position.x,
                    highestY + exactTileHeight,
                    tile.transform.position.z
                );
            }
        }
    }

    float FindHighestTileY()
    {
        float highestY = tiles[0].transform.position.y;
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position.y > highestY)
                highestY = tile.transform.position.y;
        }
        return highestY;
    }
}
