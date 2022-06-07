using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField] SpriteRenderer[] playerSprites;

    [Space]
    [SerializeField] int maxHealth;
    [SerializeField] float invTime;
    [SerializeField] float flashTime;

    float invCounter;
    float flashCounter;
    int health;

    private void Awake()
    {
        // Singleton:
        if (!instance)
            instance = this;

        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    private void Start()
    {
        health = maxHealth;
        UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);
    }

    private void Update()
    {
        if (invCounter > 0)
        {
            invCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;

            // the player sprite is going to flash for as long as they're invincible:
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                    sr.enabled = !sr.enabled;

                flashCounter = flashTime;
            }

            // make sure that when that at the end, the sprites are surely enabled:
            if (invCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                    sr.enabled = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // the invincibility counter will only be zero when the player hasn't taken any damage recently,
        // and if it IS zero, the player will take damage:
        if (invCounter <= 0)
        {
            health -= damage;
            UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);

            if (health <= 0)
            {
                health = 0;
                gameObject.SetActive(false);
            }

            else
                // when the player is hit once, set the inv timer to max value:
                invCounter = invTime; 
        }
    }
}
