using UnityEngine;

public class MinimapZoomIn : MonoBehaviour
{
    [SerializeField] float _camHeight, _camZoom, _camFarPlaneClip;

    [SerializeField] MinimapController _minimap;

    private void OnTriggerEnter(Collider other)
    {
        _minimap.SetCameraZoomParameters(_camZoom, _camHeight, _camFarPlaneClip);
    }

    private void OnTriggerExit(Collider other)
    {
        _minimap.ResetCameraZoomParameters();
    }
}
