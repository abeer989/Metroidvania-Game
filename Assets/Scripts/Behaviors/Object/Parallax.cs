using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float depth = 1;
    public float minX;
    public float newX;
    [SerializeField] PlayerController player;

    private void Awake() => player = PlayerHealthController.instance.GetComponent<PlayerController>();

    void FixedUpdate()
    {
        float realVelocity = player.RigidBody.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (pos.x <= minX)
            pos.x = newX;

        transform.position = pos;
    }
}
