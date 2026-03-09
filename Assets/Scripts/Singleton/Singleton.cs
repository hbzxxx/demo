using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance 
    {  
        get
        { 
            if(instance == null)
            {
                instance=FindObjectOfType<T>(); 
            }
            if(instance == null )
            {
                GameObject go = new GameObject(typeof(T).Name + "(Singleton)");
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }

            return instance;
        } 
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }
}
