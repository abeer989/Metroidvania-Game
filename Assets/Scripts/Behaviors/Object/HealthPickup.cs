using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healAmount;

    protected override void OnPickup()
    {
        base.OnPickup();

        // Heal player:
        PlayerHealthController.instance.HealPlayer(healAmount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPickup();
    }
}
