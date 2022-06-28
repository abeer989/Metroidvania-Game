using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] int maxHealth;

    EnemyOscillate oscillateComp;

    int health;

    private void OnEnable()
    {
        health = maxHealth;
        oscillateComp = GetComponent<EnemyOscillate>();
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (deathFX)
                Instantiate(original: deathFX, position: gameObject.transform.position, rotation: Quaternion.identity);

            // enemy explode sound:
            AudioManager.instance.PlaySFX(sfxIndex: 4);

            if (oscillateComp)
            {
                if (oscillateComp.MovePoints.Length > 0)
                    Destroy(oscillateComp.MovePoints[0].transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }
}
