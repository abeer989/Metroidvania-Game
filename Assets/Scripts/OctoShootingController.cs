using UnityEngine;

public class OctoShootingController : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    [SerializeField] float fireTime;
    [SerializeField] float shootingRange;

    Transform player;
    float fireCounter;

    void Start()
    {
        player = PlayerHealthController.instance.transform;
        fireCounter = 0;
    }

    void Update()
    {
        if (player.gameObject.activeSelf)
        {
            // turning in the player's direction relative to octo:
            if (player.position.x > transform.position.x)
                transform.localScale = new Vector2(-1, 1);

            else
                transform.localScale = Vector2.one;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < shootingRange)
                Shoot();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(firePoint.position, shootingRange);
        Gizmos.color = Color.red;
    }

    void Shoot()
    {
        fireCounter -= Time.deltaTime;

        if (fireCounter <= 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            fireCounter = fireTime;
        }
    }
}
