using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

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

    [Title("Scriptable Events")]
    [SerializeField] FloatScriptableEvent healthUIUpdateEvent; // Calls UIController.UpdateHealth(float currentHealth). Listener: UIGameCanvas
    [SerializeField] FloatScriptableEvent maxHealthUIUpdateEvent; // Calls UIController.UpdateMaxHealth(float maxHealth). Listener: UIGameCanvas
    [SerializeField] SFXDataScriptableEvent sfxEvent;
    [SerializeField] BoolScriptableEvent respawnPlayerEvent;

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
        maxHealthUIUpdateEvent.Raise(maxHealth);
        healthUIUpdateEvent.Raise(health);

        //if (UIController.instance && UIController.instance.gameObject.activeInHierarchy)
        //    UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);
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

        if (transform.position.y < -15) // if player falls down
        {
            TakeDamage(2);
            respawnPlayerEvent.Raise(false); // respawn player and don't restore health
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
            healthUIUpdateEvent.Raise(health);
            //UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);

            if (health <= 0)
            {
                // when health reaches zero -- death and respawn:
                health = 0;

                if (playerDeathFX)
                    Instantiate(playerDeathFX, transform.position, Quaternion.identity);

                // death SFX:
                sfxEvent.Raise(new SFXData(_sfxIndex: 8));
                respawnPlayerEvent.Raise(true);
            }

            else
            {
                // damage taken SFX:
                sfxEvent.Raise(new SFXData(_sfxIndex: 11, _adj: true));
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
        healthUIUpdateEvent.Raise(health);
        //UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        if (health < maxHealth)
        {
            health += healAmount;

            if (health >= maxHealth)
                health = maxHealth;

            healthUIUpdateEvent.Raise(health);
            //UIController.instance.UpdateHealth(currentHealth: health, maxHealth: maxHealth); 
        }
    }

    #region Test Functions
    [ContextMenu("Take 1 Damage")]
    public void TakeXDamage()
    {
        TakeDamage(1);
    }
    #endregion
}
