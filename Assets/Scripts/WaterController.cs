using UnityEngine;

public class WaterController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.LogError("DEATH");
    }
}
