using UnityEngine;

// 可在编辑器中创建的武器配置文件
[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Game/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("基础属性")]
    public string weaponName;// 武器名称
    public float fireRate;// 射击间隔
    public float reloadTime;// 换弹时间
    public int clipSize;// 弹夹容量
    public int maxReserveAmmo;// 最大备弹量

    [Header("子弹属性")]
    public string BulletName;//子弹名字
    public float BulletSpeed;//子弹速度
    public GameObject BulletPrefab;//子弹预制体
    public float BulletTime;//子弹存在时间
    public float Damage;//伤害值

    [Header("特效")]
    public GameObject MuzzleEffects;// 枪口火焰特效
    public float MuzzleEffectsDisappear;//特效消失时间

    [Header("音效")]
    public AudioClip FireSound;// 射击音效
    public AudioClip ReloadSound;// 换弹音效

    [Header("特殊属性")]
    public int BulletPerFire;// 每次射击发射子弹数
    public float SpreadAngle;// 子弹散布角度
}