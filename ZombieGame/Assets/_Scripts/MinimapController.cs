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
    public void SetCameraSize(float size)
    {
        _size = _cam.orthographicSize;
        DOTween.To(() => _size, x => _size = x, size, 1).OnUpdate(() =>
        {
            _cam.orthographicSize = _size;
        });
    }

    public void SetCameraHeightAndFarClip(float height, float farClip)
    {
        _newHeight = height;

        _farClip = _cam.farClipPlane;
        DOTween.To(() => _farClip, x => _farClip = x, farClip, 0.5f).OnUpdate(() =>
        {
            _cam.farClipPlane = _farClip;
        });
    }


    public void ResetValues()
    {
        SetCameraHeightAndFarClip(_defaultHeight, _defaultFarClip);
        SetCameraSize(_defaultSize);
    }

}
