using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class JoyManager : MonoBehaviour
{
    public static JoyManager Instance { get; private set; }

    [Header("Level Ayarları")]
    public int capacity = 10;          // Aynı anda toplanabilecek maksimum joy
    public int totalJoys = 10;         // Level tamamlanması için gerekli toplam joy
    public int collected = 0;          // Üzerinde taşınan joy
    public int delivered = 0;          // Kalıcı olarak teslim edilen joy

    [Header("UI")]
    public TextMeshProUGUI joyCounterText;

    [Header("Ses ve Efektler")]
    public AudioClip loseJoySound;
    private AudioSource audioSource;

    // 🔔 DebugPanel veya başka sistemlerin dinleyeceği event
    public System.Action OnJoyValuesChanged;

    private const string DeliveredKey = "DeliveredJoy";

    private void Awake()
    {
        // 🔒 Tekil instance kontrolü
        var existingManagers = FindObjectsOfType<JoyManager>();
        if (existingManagers.Length > 1)
        {
            Debug.LogWarning("⚠️ İkinci bir JoyManager bulundu, yok ediliyor...");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        delivered = PlayerPrefs.GetInt(DeliveredKey, 0);
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateUI();
        OnJoyValuesChanged?.Invoke();
    }

    public void InitLevel(int total)
    {
        totalJoys = total;
        collected = 0;
        UpdateUI();
        OnJoyValuesChanged?.Invoke();
    }

    public bool CanCollect() => collected < capacity;

    public void CollectOne()
    {
        if (!CanCollect()) return;

        collected++;
        UpdateUI();
        OnJoyValuesChanged?.Invoke();
        Debug.Log($"✨ Joy toplandı! ({collected}/{capacity})");
    }

    public void LoseOne()
    {
        if (collected <= 0) return;

        collected--;
        UpdateUI();
        OnJoyValuesChanged?.Invoke();

        if (loseJoySound != null)
            audioSource.PlayOneShot(loseJoySound);

        StartCoroutine(CameraShake(0.2f, 0.1f));
        Debug.Log($"❌ 1 Joy kaybedildi! Kalan: {collected}/{capacity}");
    }

    public void DeliverCurrent()
    {
        delivered += collected;
        collected = 0;

        PlayerPrefs.SetInt(DeliveredKey, delivered);
        PlayerPrefs.Save();

        UpdateUI();
        OnJoyValuesChanged?.Invoke();

        Debug.Log($"📦 Joy teslim edildi (toplam): {delivered}/{totalJoys}");

        if (delivered >= totalJoys)
        {
            Debug.Log("🏁 Tüm joylar teslim edildi! Level 2'ye geçiliyor...");
            StartCoroutine(NextLevelDelay());
        }
        else
        {
            Debug.Log($"⚠️ Henüz tüm joylar teslim edilmedi ({delivered}/{totalJoys}) – Level tekrar yüklenecek...");
            StartCoroutine(ReloadLevelDelay());
        }
    }

    private IEnumerator NextLevelDelay()
    {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt(DeliveredKey, 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level2");
    }

    private IEnumerator ReloadLevelDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("SampleScene");
    }

    public void ResetLevelProgress()
    {
        collected = 0;
        delivered = 0;

        PlayerPrefs.SetInt(DeliveredKey, 0);
        PlayerPrefs.Save();

        UpdateUI();
        OnJoyValuesChanged?.Invoke();

        Debug.Log("🔄 Joy sayacı tamamen sıfırlandı (yeni level).");
    }

    private void UpdateUI()
    {
        if (joyCounterText != null)
            joyCounterText.text = $"JOY: {delivered + collected}/{totalJoys}";
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        if (Camera.main == null) yield break;

        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalPos;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
