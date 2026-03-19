using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    //显示界面
    public virtual void OnEnter()
    {

    }
    //界面停止
    public virtual void OnPause()
    {

    }
    //界面继续
    public virtual void OnResume()
    {

    }
    //退出界面
    public virtual void OnExit()
    {

    }
    public T GetOrAddComponentInChildren<T>(string childName) where T : Component
    {
        // 1. 查找指定名称的子物体（只找直接子物体，多级写路径如"Panel/GoldText"）
        Transform childTrans = transform.Find(childName);

        // 2. 如果子物体不存在，自动创建
        if (childTrans == null)
        {
            GameObject newChild = new GameObject(childName);
            newChild.transform.SetParent(transform, false); // 设为子物体，保持UI缩放
            childTrans = newChild.transform;
        }

        // 3. 获取组件，没有则添加
        T targetComp = childTrans.GetComponent<T>();
        if (targetComp == null)
        {
            targetComp = childTrans.gameObject.AddComponent<T>();
        }

        return targetComp;
    }
}
