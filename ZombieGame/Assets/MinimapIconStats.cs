using UnityEngine;

public class MinimapIconStats : MonoBehaviour
{
    public float _defaultSize, _defaultHeight, _zoomedSize, _zoomedHeight;
    public bool _disabled, _neverDisable;

    private void Update()
    {
        gameObject.transform.parent.gameObject.SwapLayer("MinimapVisible", true);
    }
}
