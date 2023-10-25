using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/Trail Configuration", order = 4)]
public class TrailConfigScriptable : ScriptableObject
{
    [Header("Trail Properties")]
    public Material _material;
    public AnimationCurve _widthCurve;
    public float _duration = 0.5f;
    public float _minVertexDistance = 0.1f;
    public Gradient _color;

    [Header("Trail Miss Properties")]
    public float _missDistance = 100f;
    public float _simulationSpeed = 100f;
}
