using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] GameObject bossBattle;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // trigger the boss as soon the player
            // enters this trigger:
            bossBattle.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
