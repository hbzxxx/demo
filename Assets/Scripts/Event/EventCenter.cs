using System;
using System.Collections.Generic;

public static class EventCenter
{
    private static readonly Dictionary<EventType, Action> eventDict = new Dictionary<EventType, Action>();

    private static readonly Dictionary<EventType, Delegate> paramEventDict= new Dictionary<EventType, Delegate>();//多个参数类型

    #region 无参事件
    public static void AddListener(EventType type, Action callback)
    {
        if (!eventDict.ContainsKey(type))
            eventDict[type] = callback;
        else
            eventDict[type] += callback;
    }

    public static void RemoveListener(EventType type, Action callback)
    {
        if (eventDict.ContainsKey(type))
            eventDict[type] -= callback;
    }

    public static void TriggerEvent(EventType type)
    {
        if (eventDict.TryGetValue(type, out var action))
            action?.Invoke();
    }
    #endregion

    #region 1个参数事件
    public static void AddListener<T>(EventType type, Action<T> callback)
    {
        if (!paramEventDict.ContainsKey(type))
            paramEventDict[type] = callback;
        else
            paramEventDict[type] = (Action<T>)paramEventDict[type] + callback;
    }

    public static void RemoveListener<T>(EventType type, Action<T> callback)
    {
        if (paramEventDict.TryGetValue(type, out var del))
        {
            if (del is Action<T> action)
                paramEventDict[type] = action - callback;
        }
    }

    public static void TriggerEvent<T>(EventType type, T arg)
    {
        if (paramEventDict.TryGetValue(type, out var del) && del is Action<T> action)
            action?.Invoke(arg);
    }
    #endregion

    #region 2个参数事件
    public static void AddListener<T1, T2>(EventType type, Action<T1, T2> callback)
    {
        if (!paramEventDict.ContainsKey(type))
            paramEventDict[type] = callback;
        else
            paramEventDict[type] = (Action<T1, T2>)paramEventDict[type] + callback;
    }

    public static void RemoveListener<T1, T2>(EventType type, Action<T1, T2> callback)
    {
        if (paramEventDict.TryGetValue(type, out var del))
        {
            if (del is Action<T1, T2> action)
                paramEventDict[type] = action - callback;
        }
    }

    public static void TriggerEvent<T1, T2>(EventType type, T1 arg1, T2 arg2)
    {
        if (paramEventDict.TryGetValue(type, out var del) && del is Action<T1, T2> action)
            action?.Invoke(arg1, arg2);
    }
    #endregion

    // 清空所有事件
    public static void Clear()
    {
        eventDict.Clear();
        paramEventDict.Clear();
    }
}