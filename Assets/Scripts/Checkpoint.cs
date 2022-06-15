using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("CHECKPOINT!");

            // whenever a checkpoint is reached, the level that is to be loaded gets set to the current active scnene
            // and the load pos. gets set to the position of the checkpoint itself for when the player chooses to continue the game
            // from the main menu:
            PlayerPrefs.SetInt("continue_level_index", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetString("continue_position", transform.position.ToString());
            UIController.instance.CallShowCheckpointTextCR(); 
            RespawnController.instance.SetSpawnPoint(transform.position);

            StartCoroutine(DisableCheckpointCR());
        }
    }

    IEnumerator DisableCheckpointCR()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(10);
        GetComponent<Collider2D>().enabled = true;

        yield break;
    }
}
