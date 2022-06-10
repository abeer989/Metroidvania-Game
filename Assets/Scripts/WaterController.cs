using UnityEngine;

public class WaterController : MonoBehaviour
{
    [SerializeField] int damageAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController.instance.TakeDamage(damageAmount);
            RespawnController.instance.CallRespawnCR(_refillHealth: false);
        }
    }
}
