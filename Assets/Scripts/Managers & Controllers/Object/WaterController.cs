using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class WaterController : MonoBehaviour
{
    [SerializeField] int damageAmount;

    [Title("Scriptable Events")]
    [SerializeField] FloatScriptableEvent damagePlayerEvent; // Calls PlayerHealthController.TakeDamage(float damage). Listener: GameEventsListener

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            damagePlayerEvent.Raise(damageAmount);
            RespawnController.instance.CallRespawnCR(_refillHealth: false);
        }
    }
}
