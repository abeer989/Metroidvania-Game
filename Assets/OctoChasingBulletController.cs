using UnityEngine;

public class OctoChasingBulletController : MonoBehaviour
{
    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;

    [SerializeField] float bulletSpeed;
    [SerializeField] int damageDone;

    Transform player;

    private void OnEnable() => player = PlayerHealthController.instance.transform;

    private void Update()
    {
        if (player)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player)
        {
            if (other.CompareTag("Player"))
            {
                // inflicting damage normally, for now.
                // Will implement a TakeDamageOverTime() method in the PlayerHealthController later...
                PlayerHealthController.instance.TakeDamage(damageDone);
            }

            if (impactFX)
                Instantiate(impactFX, transform.position, Quaternion.identity);

            AudioManager.instance.PlaySFX(sfxIndex: 3, adjust: true);
            Destroy(gameObject); 
        }
    }
}