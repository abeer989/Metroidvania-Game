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

    public void CallRespawnCR() => StartCoroutine(RespawnCR());

    // the coroutine responsible for player respawn. The player will get deactivated
    // and then respawn at the location that they started from, with everything being reset to its
    // initial state (dead enemies, broken obstacles, etc.) and full health.
    IEnumerator RespawnCR()
    {
        player.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        player.transform.position = respawnPoint;
        PlayerHealthController.instance.RefillHealth();

        player.SetActive(true);
    }
}
