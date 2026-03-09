using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    NoShooting,  // 不可射击
    Shooting     // 可射击
}

public class WeaponController : MonoBehaviour
{
    [Header("武器配置")]
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private Transform firePos;// 子弹位置
    [Header("运行时数据")]
    [SerializeField] private WeaponRuntimeData runtimeData; // 武器运行时状态

    private Dictionary<WeaponState, WeaponBaseState> statePool;
    private WeaponBaseState currentState;

    public WeaponDataSO WeaponDataSO { get { return weaponDataSO; } }
    public WeaponRuntimeData RuntimeData { get { return runtimeData; } }
    public Transform FirePos { get { return firePos; } }

    private void Awake()
    {
        // 初始化运行时数据
        runtimeData = new WeaponRuntimeData();
        runtimeData.Init(weaponDataSO);

        statePool = new Dictionary<WeaponState, WeaponBaseState>()
        {
            { WeaponState.NoShooting, new WeaponNoShootingState(this) },
            { WeaponState.Shooting, new WeaponShootingState(this) }
        }
        ;
    }

    private void Start()
    {
        SwitchState(WeaponState.Shooting);
    }

    private void Update()
    {
        currentState.Update();
        CheckGlobalInput();
    }

    public void SwitchState(WeaponState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        if (statePool.TryGetValue(newState, out var state))
        {
            currentState = state;
            currentState.Enter();
        }
        else
        {
            Debug.LogError($"未找到武器状态：{newState}，请检查状态池！");
        }
    }

    #region 换弹逻辑
    private void CheckGlobalInput()
    {
        if (Input.GetKeyDown(KeyCode.R)&& !runtimeData.isReloading && runtimeData.currentClipAmmo < weaponDataSO.clipSize&& runtimeData.currentReserveAmmo > 0)//R手动换弹
        {
            StartCoroutine(ReloadCoroutine());
        }
        if(runtimeData.currentClipAmmo ==0 && runtimeData.currentReserveAmmo > 0)//自动换弹
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        runtimeData.isReloading = true;
        SwitchState(WeaponState.NoShooting);

        if (weaponDataSO.ReloadSound != null)
        {
            AudioSource.PlayClipAtPoint(weaponDataSO.ReloadSound, transform.position);// 播放换弹音效
        }

        EventCenter.TriggerEvent(EventType.PLAY_RELOAD_ANIMATION,weaponDataSO.reloadTime);
        yield return new WaitForSeconds(weaponDataSO.reloadTime);
        EventCenter.TriggerEvent(EventType.NO_PLAY_RELOAD_ANIMATION);

        int ammoToReload = Mathf.Min(weaponDataSO.clipSize - runtimeData.currentClipAmmo, runtimeData.currentReserveAmmo);// 计算换弹数量
        runtimeData.currentClipAmmo += ammoToReload;
        runtimeData.currentReserveAmmo -= ammoToReload;
        runtimeData.isReloading = false;
        SwitchState(WeaponState.Shooting);
    }
    #endregion

    #region 射击核心逻辑
    public void Fire()
    {
        if (runtimeData.isReloading || runtimeData.currentClipAmmo <= 0 || Time.time - runtimeData.lastFireTime < weaponDataSO.fireRate) return;

        runtimeData.lastFireTime = Time.time;
        runtimeData.currentClipAmmo--;

        for (int i = 0; i < weaponDataSO.BulletPerFire; i++)
        {
            float randomAngle = Random.Range(-weaponDataSO.SpreadAngle, weaponDataSO.SpreadAngle);
            Quaternion spreadRotation = firePos.rotation * Quaternion.Euler(0, 0, randomAngle);//射击角度

           
            GameObject bullet = Instantiate(weaponDataSO.BulletPrefab, firePos.position, spreadRotation);//实例化子弹
            bullet.GetComponent<PlayerBullet>().Init(firePos.right, weaponDataSO.Damage, weaponDataSO.BulletSpeed);//初始化子弹

            Destroy(bullet, weaponDataSO.BulletTime);
            CameraFollowMouseMgr.Instance.Shake();//TODO 可增加不同武器射击时的震动时间和强度

            GameObject muzzleEffects =Instantiate(weaponDataSO.MuzzleEffects, firePos.position, firePos.rotation);//特效
            Destroy(muzzleEffects, weaponDataSO.MuzzleEffectsDisappear);

            if (weaponDataSO.FireSound != null)
            {
                AudioSource.PlayClipAtPoint(weaponDataSO.FireSound, transform.position);//音效
            }
        }
        if (runtimeData.currentClipAmmo <= 0)
        {
            SwitchState(WeaponState.NoShooting);
        }

    }
    #endregion

}