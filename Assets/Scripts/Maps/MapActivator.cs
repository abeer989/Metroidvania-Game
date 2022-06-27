using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] int levelMapToActivate;

    // activate the level map at the very beginning:
    private void Start() => MapController.instance.ActivateMap(levelMapToActivate);
}
