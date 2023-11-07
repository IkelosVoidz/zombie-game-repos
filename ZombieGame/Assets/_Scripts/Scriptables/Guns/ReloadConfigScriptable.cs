using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config")]
public class ReloadConfigScriptable : ScriptableObject
{

}

[Serializable]
struct AmmoData
{
    int _maxAmmo;
    int _magSize;
    int _currentAmmo;
    int _currentMagAmmo;
}
