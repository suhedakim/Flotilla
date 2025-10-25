using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private bool hasDelivered = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasDelivered) return;
        if (!other.CompareTag("Player")) return;
        if (JoyManager.Instance == null) return;

        hasDelivered = true;
        Debug.Log("🚨 İlk temas anında hedefe ulaşıldı!");

        // 💥 Gemi hemen dursun
        ShipController controller = other.GetComponent<ShipController>();
        if (controller != null)
        {
            controller.StopMovementImmediate();
        }

        // 🎁 Joy teslimi
        JoyManager.Instance.DeliverCurrent();
    }
}
