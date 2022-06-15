using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    [SerializeField] AudioManager AMPrefab;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager newAM = Instantiate(AMPrefab);
            AudioManager.instance = newAM;
            DontDestroyOnLoad(newAM.gameObject);
        }
    }
}
