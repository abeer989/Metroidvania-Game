using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] GameObject pickupFX;
    [SerializeField] int healAmount;
    [SerializeField] TMPro.TMP_Text healthText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController playerHealthController = other.GetComponentInParent<PlayerHealthController>();

            if (playerHealthController && playerHealthController.Health < playerHealthController.MaxHealth)
            {
                // pickup SFX:
                AudioManager.instance.PlaySFX(sfxIndex: 5);

                PlayerHealthController.instance.HealPlayer(healAmount);

                // detach the parent canvas from ITS parent (the pickup object) or it'll get desroyed with it:
                healthText.transform.parent.SetParent(null);
                healthText.transform.parent.position = transform.position; // but keep the transform in place (the PU object's pos.)

                // set display msg for powerup:
                healthText.gameObject.SetActive(true);

                if (pickupFX)
                    Instantiate(pickupFX, transform.position, Quaternion.identity);

                Destroy(gameObject);
                Destroy(healthText.transform.parent.gameObject, 1); 
            }
        }
    }
}
