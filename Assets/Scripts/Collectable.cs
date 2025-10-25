using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
    // Aynı anda tek zamanlayıcı çalışsın ve her toplamada süre yenilensin
    private static Coroutine activeEffectCoroutine;
    private static ParticleSystem playerParticle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sadece Player
        if (!other.CompareTag("Player")) return;

        // Sayaç
        if (JoyManager.Instance != null)
        {
            JoyManager.Instance.CollectOne();
            Debug.Log($"✨ JoyManager: {JoyManager.Instance.collected}/{JoyManager.Instance.capacity} joy toplandı.");
        }
        else
        {
            Debug.LogWarning("⚠ JoyManager sahnede bulunamadı!");
        }

        // Player’daki Particle System'i al ve çalıştır
        if (playerParticle == null)
            playerParticle = other.GetComponent<ParticleSystem>();

        if (playerParticle != null)
        {
            playerParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            playerParticle.Play();
            Debug.Log("💫 ParticleSystem: Efekt tetiklendi!");
        }
        else
        {
            Debug.LogWarning("⚠ Player üzerinde ParticleSystem bulunamadı!");
        }

        // Coroutine'i player üzerindeki ShipController'dan yönet
        ShipController ship = other.GetComponent<ShipController>();
        if (ship != null)
        {
            // Önceki zamanlayıcı varsa iptal et (süre yenilensin)
            if (activeEffectCoroutine != null)
                ship.StopCoroutine(activeEffectCoroutine);

            // 2.5 sn sonra otomatik kapat
            activeEffectCoroutine = ship.StartCoroutine(StopEffectAfterDelay(2.5f));
        }
        else
        {
            Debug.LogWarning("⚠ ShipController yok; süreli kapatma çalışmayacak.");
        }

        // Bu joy'u yok et
        Destroy(gameObject);
    }

    private static IEnumerator StopEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerParticle != null)
        {
            playerParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Debug.Log("🕓 Efekt otomatik kapatıldı (2.5 sn).");
        }

        activeEffectCoroutine = null;
    }
}
