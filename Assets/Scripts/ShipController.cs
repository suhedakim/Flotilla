using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float forwardSpeed = 5f;
    public float turnSpeed = 120f;
    public float minRotationZ = -85f;
    public float maxRotationZ = 85f;

    [Header("Efektler")]
    public ParticleSystem collectEffect;   // 🎇 Joy toplama efekti
    public AudioClip collectSound;         // 🔊 Ses efekti (isteğe bağlı)
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private float inputTurn;
    private bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        if (collectEffect != null)
        {
            collectEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        if (!canMove)
        {
            inputTurn = 0f;
            return;
        }

        inputTurn = -Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        // --- Dönüş ---
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f)
            zRotation -= 360f;

        float newZ = Mathf.Clamp(
            zRotation + (inputTurn * turnSpeed * Time.fixedDeltaTime),
            minRotationZ,
            maxRotationZ
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newZ);

        // --- İleri Hareket ---
        Vector2 forwardDir = transform.up;
        rb.linearVelocity = forwardDir * forwardSpeed;
    }

    // 🎇 Joy toplama efekti
    public void PlayCollectEffect()
    {
        if (collectEffect == null)
        {
            Debug.LogWarning("⚠ collectEffect referansı atanmadı!");
            return;
        }

        // Efekti durdurup temizle
        collectEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        collectEffect.Clear(true);

        // Efekti çalıştır
        collectEffect.Play(true);
        Debug.Log("✨ Joy toplama efekti tetiklendi!");

        // Ses efekti varsa oynat
        if (collectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }

    // 🚫 Gemi durdurma (örneğin teslim sonrası)
    public void StopMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        Debug.Log("🛑 Gemi durdu (normal).");
    }

    // 🚫 Gemi ANINDA durur (temas anı için)
    public void StopMovementImmediate()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;

        Debug.Log("⛔ Gemi ANINDA durduruldu.");
    }

    // 🔓 Yeni levelde yeniden aktif et
    public void EnableMovement()
    {
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;

        Debug.Log("✅ Gemi yeniden hareket ediyor.");
    }

    // 💥 Çarpışma kontrolü (isteğe bağlı)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("💢 Düşmanla çarpışıldı!");
        }
    }

    // 🌀 İsteğe bağlı: efekt pozisyonunu manuel oynatmak için
    public void PlayCollectEffectAt(Vector3 position)
    {
        if (collectEffect == null) return;

        collectEffect.transform.position = position;
        collectEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        collectEffect.Play(true);
    }
}
