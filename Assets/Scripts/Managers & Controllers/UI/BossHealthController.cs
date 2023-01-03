using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;

    [SerializeField] Slider bossHealthBar;
    [SerializeField] BossBattle1 boss1;

    [SerializeField] int health;

    [Title("Scriptable Events")]
    [SerializeField] SFXDataScriptableEvent sfxEvent;

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
            sfxEvent.Raise(new SFXData(_sfxIndex: 0));
        }

        else
            // boss hit sound:
            sfxEvent.Raise(new SFXData(_sfxIndex: 1));

        bossHealthBar.value = health;
    }
}
