using UnityEngine;

public class MapActivator : MonoBehaviour
{
    [SerializeField] int levelMapToActivate;

    private void Start() => MapController.instance.ActivateMap(levelMapToActivate);
}
