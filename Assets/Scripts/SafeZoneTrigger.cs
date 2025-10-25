using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Güvenli bölgeden çıkıldı. Düşmanlar doğmaya başlayabilir!");
            // Burada düşman spawn sistemi aktif hale getirilebilir
            EnemySpawner.instance.BeginSpawning();
        }
    }
}
