using UnityEngine;

public class FullscreenMapCameraController : MonoBehaviour
{
    [SerializeField] MinimapCameraController minimapCam;
    [SerializeField] Camera camComp;

    [Space]
    [SerializeField] float scrollZoomSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveModifier;
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;

    float defaultZoomLevel;

    private void Start() => defaultZoomLevel = camComp.orthographicSize;

    private void OnEnable()
    {
        transform.position = minimapCam.transform.position;
        //camComp.orthographicSize = defaultZoomLevel;
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * camComp.orthographicSize * moveModifier * Time.unscaledDeltaTime;
        transform.position += movement;

        camComp.orthographicSize -= Input.mouseScrollDelta.y * scrollZoomSpeed * Time.unscaledDeltaTime;
        camComp.orthographicSize = Mathf.Clamp(camComp.orthographicSize, minZoom, maxZoom);
    }
}
