using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class BombController : MonoBehaviour
{
    [SerializeField] float timeToExplode = -.5f;
    [SerializeField] float blastRange;
    [SerializeField] int enemyDamageAmount;
    [SerializeField] GameObject explosionFX;
    [SerializeField] LayerMask whatIsDestructible;
    [SerializeField] LayerMask whatIsEnemy;

    [Title("Scriptable Events")]
    [SerializeField] SFXDataScriptableEvent sfxEvent;

    void Update()
    {
        timeToExplode -= Time.deltaTime;

        if (timeToExplode <= 0)
        {
            if (explosionFX)
                Instantiate(explosionFX, transform.position, transform.rotation);

            sfxEvent.Raise(new SFXData(_sfxIndex: 4, _adj: true));

            Destroy(gameObject);

            // get every destructible in the blast radius in an array:
            Collider2D[] objectsToDestroy = Physics2D.OverlapCircleAll(point: transform.position, radius: blastRange, layerMask: whatIsDestructible);

            // get every ENEMY in the blast radius in an array:
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(point: transform.position, radius: blastRange, layerMask: whatIsEnemy);

            // destroy each of those objects:
            if (objectsToDestroy.Length > 0)
            {
                foreach (Collider2D c in objectsToDestroy)
                    Destroy(c.gameObject);
            }            
            
            if (enemiesToDamage.Length > 0)
            {
                foreach (Collider2D e in enemiesToDamage)
                {
                    EnemyHealthController enemyHealthController = e.GetComponent<EnemyHealthController>();

                    if (enemyHealthController)
                        enemyHealthController.DamageEnemy(enemyDamageAmount);

                    if (e.name.ToLower().Contains("boss"))
                        BossHealthController.instance.TakeDamage(enemyDamageAmount);
                }
            }
        }
    }
}
