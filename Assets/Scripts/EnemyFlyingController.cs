using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Space]
    [SerializeField] float chaseStartRange;
    [SerializeField] float turnSpeed;
    [SerializeField] float moveSpeed;

    Transform player;
    bool isChasing;

    private void Start() => player = PlayerHealthController.instance.transform;

    private void Update()
    {
        if (!isChasing)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < chaseStartRange)
                isChasing = true;
        }

        else
        {
            if (player.gameObject.activeSelf)
            {
                // MOVING TOWARD THE PLAYER:
                transform.position += -transform.right * moveSpeed * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                // ROTATING WITH THE PLAYER POSITION:
                // direction to turn in:
                Vector3 direction = transform.position - player.position;

                // the angle we want the enemy to turn:
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // to turn the enemy toward the player at a graceful speed:
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }

        animator.SetBool("isChasing", isChasing);
    }
}
