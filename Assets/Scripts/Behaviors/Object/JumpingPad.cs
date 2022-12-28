using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    [SerializeField] float padJumpForce;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<PlayerController>())
                other.gameObject.GetComponentInParent<PlayerController>().JumpOffPad(jumpForce: padJumpForce);
        }
    }
}
