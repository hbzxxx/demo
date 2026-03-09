using UnityEngine;

[System.Serializable]
public class WeaponRuntimeData
{
    [Header("当前弹夹弹药")]
    public int currentClipAmmo;
    [Header("备用弹药")]
    public int currentReserveAmmo;
    [Header("上次开火时间")]
    public float lastFireTime;
    [Header("是否正在换弹")]
    public bool isReloading;

    public void Init(WeaponDataSO data)
    {
        currentClipAmmo = data.clipSize;
        currentReserveAmmo = data.maxReserveAmmo;
        lastFireTime = 0;
        isReloading = false;
    }
}