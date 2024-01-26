using DG.Tweening;
using UnityEngine;

public class ObjectiveVisualizer : MonoBehaviour
{

    void Start()
    {
        transform.DOMoveY(transform.position.y + 0.5f, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
