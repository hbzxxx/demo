using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector2 _firePos;
    private float _bulletSpeed;
    private float _damage;
    public void Init(Vector2 firePos,float damage,float bulletSpeed)
    {
        _firePos = firePos;
        _bulletSpeed = bulletSpeed;
        _damage = damage;
    }
    void Update()
    {
        ShootingDirection();
    }

    #region ×Óµ¯Éä»÷·½Ïò
    public void ShootingDirection()
    {
        transform.position += (Vector3)_firePos * _bulletSpeed * Time.deltaTime;
    }
    #endregion
}
