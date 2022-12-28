using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    [SerializeField] PlayerHealthController playerPrefab;
    [SerializeField] Vector3 playerPosition;

    [Space]
    [SerializeField] UIController UICPrefab;
    [SerializeField] AudioManager AMPrefab;
    [SerializeField] RespawnController respawnControllerPrefab;
    [SerializeField] MapController mapControllerPrefab;

    private void Awake()
    {
        if (PlayerHealthController.instance == null)
        {
            PlayerHealthController player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
            PlayerHealthController.instance = player;
            DontDestroyOnLoad(player.gameObject);
        }

        if (UIController.instance == null)
        {
            UIController newUIC = Instantiate(UICPrefab);
            UIController.instance = newUIC;
            DontDestroyOnLoad(newUIC.gameObject);
        }

        if (AudioManager.instance == null)
        {
            AudioManager newAM = Instantiate(AMPrefab);
            AudioManager.instance = newAM;
            DontDestroyOnLoad(newAM.gameObject);
        }

        if (RespawnController.instance == null)
        {
            RespawnController newRC = Instantiate(respawnControllerPrefab);
            RespawnController.instance = newRC;
            DontDestroyOnLoad(newRC.gameObject);
        }

        if (MapController.instance == null)
        {
            MapController newMC = Instantiate(mapControllerPrefab);
            MapController.instance = newMC;
            DontDestroyOnLoad(newMC.gameObject);
        }
    }
}
