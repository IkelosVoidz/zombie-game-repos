using UnityEngine;

public class MinimapZoomIn : MonoBehaviour
{
    [SerializeField] float _camHeight, _camZoom, _camFarPlaneClip;

    [SerializeField] MinimapController _minimap;

    private void OnTriggerEnter(Collider other)
    {
        _minimap.SetCameraHeightAndFarClip(_camHeight, _camFarPlaneClip);
        _minimap.SetCameraSize(_camZoom);
    }

    private void OnTriggerExit(Collider other)
    {
        _minimap.ResetValues();
    }
}
