using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyShipController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 3f;
    public float rotateSpeed = 120f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("EnemyShipController: Player bulunamadı!");
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Oyuncuya yönel
        Vector2 dir = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        float step = rotateSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), step);

        rb.linearVelocity = transform.up * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (JoyManager.Instance != null)
            JoyManager.Instance.LoseOne();

        Destroy(gameObject);
    }
}
