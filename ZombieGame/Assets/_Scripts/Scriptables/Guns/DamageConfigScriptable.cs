using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Weapons/Damage Config", order = 2)]
public class DamageConfigScriptable : ScriptableObject
{
    public MinMaxCurve DamageCurve;

    private void Reset()
    {
        DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public int GetDamage(float distance = 0)
    {
        return Mathf.CeilToInt(DamageCurve.Evaluate(distance, Random.value));
    }
}
