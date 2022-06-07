using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] bool destroyOnContact;
    [SerializeField] GameObject enemyDestroyFX;

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
        PlayerHealthController.instance.TakeDamage(damageAmount);

        if (destroyOnContact)
        {
            Destroy(gameObject);

            if (enemyDestroyFX)
                Instantiate(enemyDestroyFX, transform.position, Quaternion.identity);
        }
    }
}
