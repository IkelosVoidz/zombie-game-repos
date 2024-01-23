using DG.Tweening;
using UnityEngine;


public class MinimapController : MonoBehaviour
{
    private Camera _cam;
    private float _size, _farClip;


    [SerializeField] private float _defaultSize, _defaultHeight, _defaultFarClip, _newHeight;
    [SerializeField] private Transform _target;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _size = _defaultSize;
    }

    private void Update()
    {
        transform.DOMove(new Vector3(_target.position.x, transform.position.y, _target.position.z), 1);
        transform.DOLocalMoveY(_newHeight, 1);
        transform.eulerAngles = new Vector3(90, 0, -_target.eulerAngles.y);
    }
    public void SetCameraZoomParameters(float size, float height, float farClip)
    {
        _size = _cam.orthographicSize;
        _newHeight = height; //in update
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
        _size = _cam.orthographicSize;
        _cam.farClipPlane = _defaultFarClip;
        _newHeight = _defaultHeight; //in update
        DOTween.To(() => _size, x => _size = x, _defaultSize, 1).OnUpdate(() =>
        {
            _cam.orthographicSize = _size;
        });
    }

}
