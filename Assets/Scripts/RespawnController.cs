using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    [SerializeField] float respawnDelay;

    Vector3 respawnPoint;
    GameObject player;

    private void Awake()
    {
        // Singleton:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        player = PlayerHealthController.instance.gameObject;
        respawnPoint = player.transform.position;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint) => respawnPoint = newSpawnPoint;

    public void CallRespawnCR(bool _refillHealth = true) => StartCoroutine(RespawnCR(refillHeath: _refillHealth));

    // the coroutine responsible for player respawn. The player will get deactivated
    // and then respawn at the location that they started from, with everything being reset to its
    // initial state (dead enemies, broken obstacles, etc.) and full health.
    IEnumerator RespawnCR(bool refillHeath = true)
    {
        player.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        player.transform.position = respawnPoint;

        if (refillHeath)
            PlayerHealthController.instance.RefillHealth(); 

        player.SetActive(true);
    }
}
