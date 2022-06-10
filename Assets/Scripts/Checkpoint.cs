using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool checkPointTextDisplayedOnce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("CHECKPOINT!");

            if (true)
                UIController.instance.CallShowCheckpointTextCR(); 

            RespawnController.instance.SetSpawnPoint(transform.position);
        }
    }
}
