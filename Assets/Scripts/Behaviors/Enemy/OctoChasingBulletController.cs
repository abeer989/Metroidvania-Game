using UnityEngine;

public class OctoChasingBulletController : MonoBehaviour
{
    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;

    [SerializeField] float bulletSpeed;
    [SerializeField] float damageTime;
    [SerializeField] float damageDone;

    Transform player;

    private void OnEnable() => player = PlayerHealthController.instance.transform;

    private void Update()
    {
        if (player.gameObject.activeSelf)
        {
            // MOVING TOWARD THE PLAYER:
            //transform.position += -transform.right * moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, bulletSpeed * Time.deltaTime);

            // ROTATING WITH THE PLAYER POSITION:
            // direction to turn in:
            Vector3 direction = transform.position - player.position;

            // the angle we want the enemy to turn:
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // to turn the enemy toward the player at a graceful speed:
            transform.rotation = targetRotation;
        }

        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); 
        
        if (player)
        {
            if (other.CompareTag("Player"))
            {
                // inflicting damage normally, for now.
                // Will implement a TakeDamageOverTime() method in the PlayerHealthController later...
                //PlayerHealthController.instance.TakeDamage(damageDone);
                PlayerHealthController.instance.TakeDamageOverTime(OTDamageTime: damageTime, _OTDamage: damageDone);
            }

            AudioManager.instance.PlaySFX(sfxIndex: 3, adjust: true);
        }
    }

    private void OnDestroy()
    {
        if (impactFX)
            Instantiate(impactFX, transform.position, Quaternion.identity);
    }
}
