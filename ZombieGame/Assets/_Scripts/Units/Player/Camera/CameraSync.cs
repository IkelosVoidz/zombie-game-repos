using UnityEngine;

public class CameraSync : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private Camera _camToSync;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        _cam.fieldOfView = _camToSync.fieldOfView;
        _cam.transform.position = _camToSync.transform.position;
        _cam.transform.rotation = _camToSync.transform.rotation;
    }
}
