using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] bool destroyOnContact;
    
    [Space]
    [SerializeField] GameObject enemyDestroyFX;

    [SerializeField] int damageAmount;

    [Title("Scriptable Events")]
    [SerializeField] FloatScriptableEvent damagePlayerEvent; // Calls PlayerHealthController.TakeDamage(float damage). Listener: GameEventsListener

    // Damage the player in all cases (collision & trigger):
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ApplyDamage();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            ApplyDamage();
    }

    void ApplyDamage()
    {
        damagePlayerEvent.Raise(damageAmount);

        if (destroyOnContact)
        {
            Destroy(gameObject);

            if (enemyDestroyFX)
                Instantiate(enemyDestroyFX, transform.position, Quaternion.identity);
        }
    }
}
