using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform exitPoint;
    [SerializeField] Animator animator;
    [SerializeField] int sceneToLoadIndex;
    [SerializeField] float openRange;
    [SerializeField] float playerExitSpeed;

    PlayerController player;

    bool playerExiting;

    private void Start() => player = PlayerHealthController.instance.GetComponent<PlayerController>();

    private void Update()
    {
        // calc. the distance betsween the player and the door:
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // if the distance is lesser than the open range, the opening anim will play
        if (distance < openRange)
            animator.SetBool("isDoorOpen", true);

        // else, the closing anim will play:
        else
            animator.SetBool("isDoorOpen", false);

        if (playerExiting)
            // move player toward the exitPoint:
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.position, playerExitSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // if the player isn't currently exiting through the door:
            if (!playerExiting)
            {
                // turn it's camMove bool off, so all movement it frozen:
                player.CanMove = false;
                StartCoroutine(DoorCR());
            }
        }
    }

    IEnumerator DoorCR()
    {
        // turn the exiting bool to true:
        playerExiting = true;

        // freeze the animator, so whatever the last frame is, the player gets sucked into the door with that:
        player.StandingSpriteAnimator.enabled = false;

        UIController.instance.SetFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawnPoint(exitPoint.position);
        player.CanMove = true;
        player.StandingSpriteAnimator.enabled = true;
        SceneManager.LoadScene(sceneBuildIndex: sceneToLoadIndex);

        UIController.instance.SetFadeFromBlack();

        yield break;
    }
}
