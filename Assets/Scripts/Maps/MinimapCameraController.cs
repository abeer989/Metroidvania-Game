using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    Vector3 offset;

    // maintaning the z pos:
    void Start() => offset.z = transform.position.z;

    // the minimap is going to be boundless
    void Update() => transform.position = PlayerHealthController.instance.transform.position + offset;
}
