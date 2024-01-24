using DG.Tweening;
using UnityEngine;


public class MinimapController : MonoBehaviour
{
    private Camera _cam;
    private float _size, _farClip, _percent;
    [SerializeField] public bool _zoomedIn = false;


    [SerializeField] public float _defaultSize, _defaultHeight, _defaultFarClip, _newHeight, _newSize;
    [SerializeField] private Transform _target;
    [SerializeField] private MinimapIconsHandler _iconsHandler;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _size = _defaultSize;
    }

    private void Update()
    {
        transform.DOMove(new Vector3(_target.position.x, _target.position.y + _newHeight, _target.position.z), 1);
        transform.eulerAngles = new Vector3(90, 0, -_target.eulerAngles.y);
    }
    public void SetCameraZoomParameters(float size, float height, float farClip)
    {
        _size = _cam.orthographicSize;
        _newHeight = height; //in update
        _zoomedIn = true;
        _iconsHandler.ZoomIcons(height - 1);
        DOTween.To(() => _size, x => _size = x, size, 1).OnUpdate(() =>
        {
            _cam.orthographicSize = _size;
        }).OnComplete(() =>
        {
            _cam.farClipPlane = farClip;
        });
    }
    public void ResetCameraZoomParameters()
    {
        _zoomedIn = false;
        _size = _cam.orthographicSize;
        _cam.farClipPlane = _defaultFarClip;
        _newHeight = _defaultHeight; //in update
        _iconsHandler.ResetZoomIcons();
        DOTween.To(() => _size, x => _size = x, _defaultSize, 1).OnUpdate(() =>
        {
            _cam.orthographicSize = _size;
        });

        _zoomedIn = false;
    }

}
