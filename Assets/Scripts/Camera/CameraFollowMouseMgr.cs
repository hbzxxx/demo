using System;
using UnityEngine;

public class CameraFollowMouseMgr : Singleton<CameraFollowMouseMgr>
{
    private Transform _target;
    private Vector3 _cameraOffset = new Vector3(0, 0, -15);
    private Vector3 _mouseOffsetFinal;//随鼠标的偏移量
    private float _maxMouseOffset = 0.3f;//随鼠标的参数
    private float _smoothSpeed = 5f;//镜头移动速度

    private float _shakeDuration=0f;//震动时间
    private float _shakeMagnitude=0.3f;//震动参数
    private Vector3 _shakeOffset;//震动偏移量
    private void Awake()
    {
        _target = GameObject.Find("Player").transform;
        
    }
    private void LateUpdate()
    {
        FollowMouseOffset();
        CameraShake();
        transform.position = Vector3.Lerp(transform.position,
            _target.position + _cameraOffset + _mouseOffsetFinal + _shakeOffset,//目标位置+镜头Z轴偏移+随鼠标偏移+震动偏移
            Time.deltaTime * _smoothSpeed); //让镜头移动的速度在不同帧率一致
       
    }
    public void Shake(float duration=0.2f,float magnitude = 0.2f)
    {
        _shakeDuration = duration;
        _shakeMagnitude = magnitude;
    }

    private void CameraShake()
    {
        if (_shakeDuration > 0)
        {
            _shakeOffset = UnityEngine.Random.insideUnitCircle * _shakeMagnitude;
            _shakeDuration -= Time.deltaTime;
        }
        else
        {
            _shakeOffset = Vector3.zero;
        }
    }

    private void FollowMouseOffset()//随鼠标的偏移量
    {
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector3 mousePos = Input.mousePosition;//鼠标的位置
        Vector2 offsetmouse= new Vector2((mousePos.x- screenCenter.x)/ Screen.width * 0.5f, (mousePos.y-screenCenter.y)/Screen.height * 0.5f);//归一化
        _mouseOffsetFinal = Vector2.ClampMagnitude(offsetmouse,1f)* _maxMouseOffset;
        
    }
}
