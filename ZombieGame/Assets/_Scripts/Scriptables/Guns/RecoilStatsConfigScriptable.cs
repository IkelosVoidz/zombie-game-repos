using UnityEngine;

[CreateAssetMenu(fileName = "RecoilStatsCfg", menuName = "Weapons/RecoilStats", order = 0)]
public class RecoilStatsConfigScriptable : ScriptableObject
{
    [Header("Speed Settings")]
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]

    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;
    [Space(10)]

    [Header("Amount Settings:")]
    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]
    public Vector3 RecoilRotationAim = new Vector3(10, 4, 6);
    public Vector3 RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);
}
