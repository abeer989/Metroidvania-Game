using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableEvents.Events;

public class BossBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;

    [SerializeField] float moveSpeed;
    [SerializeField] int damageAmount;

    [Title("Scriptable Events")]
    [SerializeField] FloatScriptableEvent damagePlayerEvent; // Calls PlayerHealthController.TakeDamage(float damage). Listener: GameEventsListener
    [SerializeField] SFXDataScriptableEvent sfxEvent;

    private void Start()
    {
        // ROTATING WITH THE PLAYER POSITION:
        // direction to turn in:
        Vector3 direction = transform.position - PlayerHealthController.instance.transform.position;

        // the angle we want the bullet to turn:
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;

        // boss shot sound:
        sfxEvent.Raise(new SFXData(_sfxIndex: 2, _adj: true));
    }

    private void Update() => RB.velocity = -transform.right * moveSpeed;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            damagePlayerEvent.Raise(damageAmount);

        if (impactFX)
            Instantiate(impactFX, transform.position, transform.rotation); 

        Destroy(gameObject);

        // bullet impact sound:
        sfxEvent.Raise(new SFXData(_sfxIndex: 3, _adj: true));
    }
}
