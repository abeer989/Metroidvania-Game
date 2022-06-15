using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    [SerializeField] Slider bossHealthBar;
    [SerializeField] BossBattle1 boss1;

    [SerializeField] int health;

    public int Health
    {
        get { return health; }
    }

    private void Awake()
    {
        // Singleton:
        if (instance == null)
            instance = this;

        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void OnEnable()
    {
        bossHealthBar.maxValue = health;
        bossHealthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            boss1.EndBossBattle();

            // boss death sound:
            AudioManager.instance.PlaySFX(0);
        }

        else
            // boss hit sound:
            AudioManager.instance.PlaySFX(1);

        bossHealthBar.value = health;
    }
}
