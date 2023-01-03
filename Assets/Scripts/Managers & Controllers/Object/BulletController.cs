using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class BulletController : MonoBehaviour
{
    public Vector2 moveDir;

    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;

    [Space]
    [SerializeField] float bulletSpeed;
    [SerializeField] int damageDone;

    [Title("Scriptable Events")]
    [SerializeField] SFXDataScriptableEvent sfxEvent;

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
        sfxEvent.Raise(new SFXData(_sfxIndex: 3, _adj: true));

        Destroy(gameObject);
    }

    private void OnBecameInvisible() => Destroy(gameObject);
}
