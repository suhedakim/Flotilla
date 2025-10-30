using UnityEngine;
using System.Collections;

public class DebugPanelDev : MonoBehaviour
{
    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;

    private int displayedCollected;
    private int displayedDelivered;
    private int displayedTotal;

    private void Start()
    {
        StartCoroutine(WaitForJoyManager());
    }

    private IEnumerator WaitForJoyManager()
    {
        while (JoyManager.Instance == null)
            yield return null;

        JoyManager.Instance.OnJoyValuesChanged += RefreshValues;
        RefreshValues();

        Debug.Log("✅ DebugPanel → JoyManager bağlantısı kuruldu.");
    }

    private void OnDisable()
    {
        if (JoyManager.Instance != null)
            JoyManager.Instance.OnJoyValuesChanged -= RefreshValues;
    }

    private void RefreshValues()
    {
        if (JoyManager.Instance == null) return;

        displayedCollected = JoyManager.Instance.collected;
        displayedDelivered = JoyManager.Instance.delivered;
        displayedTotal = JoyManager.Instance.totalJoys;
    }

    private void OnGUI()
    {
        if (!Application.isPlaying) return;
        if (JoyManager.Instance == null) return;

        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                normal = { textColor = Color.white }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold
            };
        }

        GUI.Box(new Rect(15, 15, 220, 140), "🧭 DEV DEBUG PANEL");
        GUILayout.BeginArea(new Rect(25, 45, 200, 150));

        int totalDisplayed = displayedCollected + displayedDelivered;
        GUILayout.Label($"🍉 Joy: {totalDisplayed} / {displayedTotal}", labelStyle);
        GUILayout.Space(5);
        GUILayout.Label($"📦 Teslim: {displayedDelivered}", labelStyle);
        GUILayout.Label($"🪣 Toplanan: {displayedCollected}", labelStyle);
        GUILayout.Space(10);

        if (GUILayout.Button("🔄 Joy'ları Sıfırla", buttonStyle))
        {
            JoyManager.Instance.ResetLevelProgress();
            RefreshValues();
        }

        GUILayout.EndArea();
    }
}
