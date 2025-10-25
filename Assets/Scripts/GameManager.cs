using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int collectedJoys = 0;
    public int targetJoys = 10;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddJoy()
    {
        collectedJoys++;
        Debug.Log($"🧸 Joy toplandı! ({collectedJoys}/{targetJoys})");

        if (collectedJoys >= targetJoys)
            Debug.Log("🎉 1. Level tamamlandı!");
    }
}
