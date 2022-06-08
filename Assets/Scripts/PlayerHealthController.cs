using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField] SpriteRenderer[] playerSprites;

    [Space]
    [SerializeField] GameObject playerDeathFX;

    [Space]
    [SerializeField] int maxHealth;
    [SerializeField] float invTime;
    [SerializeField] float flashTime;

    float invCounter;
    float flashCounter;
    int health;

    // Public properties:
    public int Health { get { return health; } }    
    
    public int MaxHealth { get { return maxHealth; } }

    private void Awake()
    {
        // Singleton:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
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

            // make sure that at the end, the sprites are surely enabled:
            if (invCounter <= 0)
            {
                foreach (SpriteRenderer sr in playerSprites)
                    sr.enabled = true;

                flashCounter = 0;
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

                if (playerDeathFX)
                    Instantiate(playerDeathFX, transform.position, Quaternion.identity); 

                RespawnController.instance.CallRespawnCR();
            }

            else
                // when the player is hit once, set the inv timer to max value:
                invCounter = invTime; 
        }
    }

    public void RefillHealth()
    {
        health = maxHealth;
        UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        health += healAmount;

        if (health >= maxHealth)
            health = maxHealth;

        UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);
    }
}
