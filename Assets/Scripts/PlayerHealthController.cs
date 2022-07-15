using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [SerializeField] SpriteRenderer[] playerSprites;

    [Space]
    [SerializeField] GameObject playerDeathFX;

    [Space]
    [SerializeField] float maxHealth;
    [SerializeField] float invTime;
    [SerializeField] float flashTime;

    float invCounter;
    float flashCounter;
    float OTDamageCounter;
    float OTDamage;
    float health;
    bool isPoisoned;

    // Public properties:
    public float Health { get { return health; } }    
    
    public float MaxHealth { get { return maxHealth; } }

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

        if (UIController.instance && UIController.instance.gameObject.activeInHierarchy)
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

        if (transform.position.y < -15)
        {
            TakeDamage(2);
            RespawnController.instance.CallRespawnCR(_refillHealth: false);
        }

        // Taking damage over time:
        if (OTDamageCounter > 0)
        {
            foreach (SpriteRenderer sr in playerSprites) sr.color = Color.green;
            isPoisoned = true;

            OTDamageCounter -= Time.deltaTime;
            TakeDamage(damage: OTDamage);

            if (OTDamageCounter <= 0 && isPoisoned)
            {
                foreach (SpriteRenderer sr in playerSprites) sr.color = Color.white;
                isPoisoned = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // the invincibility counter will only be zero when the player hasn't taken any damage recently,
        // and if it IS zero, the player will take damage:
        if (invCounter <= 0)
        {
            health -= damage;
            UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);

            if (health <= 0)
            {
                // when health reaches zero -- death and respawn:
                health = 0;

                if (playerDeathFX)
                    Instantiate(playerDeathFX, transform.position, Quaternion.identity);

                // death SFX:
                AudioManager.instance.PlaySFX(sfxIndex: 8);
                RespawnController.instance.CallRespawnCR();
            }

            else
            {
                // damage taken SFX:
                AudioManager.instance.PlaySFX(sfxIndex: 11, adjust: true);
                // when the player is hit once, set the inv timer to max value:
                invCounter = invTime; 
            }
        }
    }

    public void ResetOTDamage()
    {
        OTDamageCounter = 0;
        foreach (SpriteRenderer sr in playerSprites) sr.color = Color.white;
        isPoisoned = false;
    }

    public void TakeDamageOverTime(float OTDamageTime, float _OTDamage)
    {
        OTDamageCounter = OTDamageTime;
        OTDamage = _OTDamage;
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
