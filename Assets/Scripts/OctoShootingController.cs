using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoShootingController : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float fireTime;

    Transform player;
    float fireCounter;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        fireCounter = 0;
    }

    void Update()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector2(-1, 1);

        else
            transform.localScale = Vector2.one;
    }
}
