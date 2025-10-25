using UnityEngine;

public class JoyCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎁 Joy toplandı!");

            // Sayaç sistemine bildir
            if (GameManager.instance != null)
                GameManager.instance.AddJoy();

            // Joy’u yok et
            Destroy(gameObject);
        }
    }
}
