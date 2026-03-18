using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
}

public class PlayerController : Singleton<PlayerController>
{
    [Header("移动设置")]
    public PlayerData playerData;
    public Rigidbody2D Rigidbody;
    public GameObject Visual;//玩家模型

    public SpriteRenderer sr;//玩家图片
    public Color originalColor;//原始颜色
    [HideInInspector]
    public Animator Animator;
    public Animator ReloadAnimation;

    private WeaponController weaponController;//武器
    private bool isFacingRight = true; // 初始朝右

    private PlayerBaseState currentState;//当前玩家状态
    private Dictionary<PlayerState, PlayerBaseState> statePool;

    private void Awake()
    {
        Animator = Visual.GetComponent<Animator>();
        sr = Visual.GetComponent<SpriteRenderer>();
        ReloadAnimation.gameObject.SetActive(false);
        originalColor = sr.color;
        weaponController = GetComponentInChildren<WeaponController>();//子寻找对象组件
        statePool = new Dictionary<PlayerState, PlayerBaseState>()
        {
            { PlayerState.Idle, new PlayerIdleState(this) },
            { PlayerState.Run, new PlayerRunState(this) },
        };
    }
    private void Start()
    {
        EventCenter.AddListener<float>(EventType.PLAY_RELOAD_ANIMATION, PlayReloadAnimation);
        EventCenter.AddListener(EventType.NO_PLAY_RELOAD_ANIMATION, NoPlayReloadAnimation);
        SwitchState(PlayerState.Idle);
    }

    private void Update()
    {
        UpdateWeaponAim();
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }
    private void PlayReloadAnimation(float reloadPlayTime)
    {
        ReloadAnimation.speed=1 / reloadPlayTime;
        ReloadAnimation.gameObject.SetActive(true);
        Debug.Log("执行换弹动画");
    }
    private void NoPlayReloadAnimation()
    {
        ReloadAnimation.gameObject.SetActive(false);
        Debug.Log("执行关闭换弹动画");
    }
    #region 人物与武器的旋转
    private void UpdateWeaponAim()
    {
        if (weaponController == null) return;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将鼠标的屏幕坐标转换为世界坐标
        mouseWorldPos.z = 0;
        Vector2 aimDir = mouseWorldPos - weaponController.transform.position;
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;//旋转角度
        weaponController.transform.rotation = Quaternion.Euler(0, 0, aimAngle);//设置武器的旋转角度

        bool newFacingRight = mouseWorldPos.x > Visual.transform.position.x;//鼠标是否在人物的右边
        if (newFacingRight != isFacingRight)
        {
            FlipPlayer(newFacingRight);
            isFacingRight = newFacingRight;
        }
    }
    private void FlipPlayer(bool newFacingRight)
    {
        Vector3 parent = weaponController.transform.parent.localPosition;//武器放置点的局部位置
        parent.x = -parent.x;
        weaponController.transform.parent.localPosition = parent;

        var scale1 = transform.localScale;
        scale1.y = Mathf.Abs(scale1.x) * (newFacingRight ? 1 : -1);
        weaponController.transform.localScale = scale1;

        Vector3 scale = Visual.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (newFacingRight ? 1 : -1);//处理物体翻转时保持缩放值一致
        Visual.transform.localScale = scale;
    }
    #endregion

    public void Hit(int damage)//玩家受伤
    {
        StartCoroutine(HitFlash(damage));
    }
    IEnumerator HitFlash(int damage)
    {
        if (playerData.isDead)
        {
            Debug.Log("玩家死亡");
            yield break;
        }
        if (!playerData.invincibility)
        {
            yield return new WaitForSeconds(0.5f);
            sr.color = new Color32(255, 85, 85, 255);
            yield return new WaitForSeconds(0.2f);
            playerData.curHealth = Math.Max(playerData.curHealth - damage, 0);
            Debug.Log($"受到敌人的{damage}点伤害");
            if (playerData.curHealth == 0)
            {
                playerData.isDead = true;
                sr.color = Color.white;
                //播放死亡动画
                //结束游戏，返回主界面
                //重置游戏
            }
            if (playerData.isDead) yield break;
            sr.color = new Color32(119, 93, 93, 255);
            playerData.invincibility = true;
            yield return new WaitForSeconds(playerData.invincibilityTime);
            playerData.invincibility = false;
            sr.color = originalColor;
            yield break;
        }
    }
    public void SwitchState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        if (statePool.TryGetValue(newState, out PlayerBaseState state))
        {
            currentState = state;
            currentState.EnterState();
        }
        else
        {
            Debug.LogError($"未找到状态：{newState}，请检查状态池！");
        }
    }
    public void MovePlayer(Vector2 moveDir)
    {
        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();//归一化，保存角度
        }
        Rigidbody.MovePosition(Rigidbody.position + moveDir * playerData.moveSpeed * Time.fixedDeltaTime);//当前位置 + 移动方向 * 速度 * 固定时间
    }
}