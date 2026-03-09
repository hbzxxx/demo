using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    private Dictionary<string, WeaponData> weaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadWeaponData(WeaponData[] weapons)
    {
        weaponDataDict.Clear();
        foreach (var weapon in weapons)
        {
            weaponDataDict[weapon.ID] = weapon;
        }
        Debug.Log($"加载了 {weaponDataDict.Count} 个武器数据");
    }

    public WeaponData GetWeaponData(string id)
    {
        return weaponDataDict.TryGetValue(id, out WeaponData data) ? data : null;
    }

    public Dictionary<string, WeaponData> GetAllWeaponData()
    {
        return weaponDataDict;
    }
}
