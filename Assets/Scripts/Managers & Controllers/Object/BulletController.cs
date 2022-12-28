using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector2 moveDir;

    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;

    [Space]
    [SerializeField] float bulletSpeed;
    [SerializeField] int damageDone;

    void Update() => RB.velocity = moveDir * bulletSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyHealthController>().DamageEnemy(damage: damageDone);

        else if (other.CompareTag("BOSS"))
            BossHealthController.instance.TakeDamage(damage: damageDone);

        if (impactFX)
            Instantiate(impactFX, transform.position, Quaternion.identity);

        // bullet impact sound:
        AudioManager.instance.PlaySFX(sfxIndex: 3, adjust: true);

        Destroy(gameObject);
    }

    private void OnBecameInvisible() => Destroy(gameObject);
}
