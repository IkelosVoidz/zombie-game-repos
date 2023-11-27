using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : StaticSingleton<PlayerInventory>
{
    [SerializeField] private List<InventoryObjectCluster> _inventory;

    //esto es O(n*2) creo xdddd pero no va a ocurrir muy a menudo asi qq que mas da 
    public WeaponScriptable GetWeapon(string weaponName)
    {
        return (WeaponScriptable)_inventory
           .Select(w => w.obj)
           .FirstOrDefault(w => w._type == "Weapon" && w._name == weaponName);
    }

    public WeaponScriptable[] GetAllWeapons()
    {
        return (WeaponScriptable[])_inventory
             .Select(w => w.obj)
             .Where(w => w._type == "Weapon");
    }

    public void AddItem(InventoryObjectSO obj)
    {
    }

    public void RemoveItem()
    {
        //por hacer
    }
}

[Serializable]
public class InventoryObjectCluster
{
    [SerializeField] public InventoryObjectSO obj;
    [SerializeField] public int amount;

    public InventoryObjectCluster(InventoryObjectSO obj, int amt)
    {
        this.obj = obj;
        amount = amt;
    }
};
