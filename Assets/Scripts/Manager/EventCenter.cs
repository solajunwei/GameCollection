
using System;
using System.Collections.Generic;



public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    #region 添加监听

    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }

        Delegate d = m_EventTable[eventType];
        if(d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1},要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    public static void AddListener(EventType eventType, CallBack callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callback;
    }

    public static void AddListener<T>(EventType eventType, CallBack<T> callback)
    {
        OnListenerAdding(eventType, callback);
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callback;
    }

    #endregion


    #region 移除监听

    public static void OnListenerRemoving(EventType eventType, Delegate callback)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误: 事件{0}没有对应的委托", eventType));
            }
            else if(d.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("移除监听错误: 尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的类型为{2}", eventType, d.GetType(), callback.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误: 没有事件码{0}", eventType));
        }
    }

    public static void OnListenerRemoved(EventType eventType)
    {
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }

    public static void RemoveListener(EventType eventType, CallBack callback)
    {
        OnListenerRemoving(eventType, callback);
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callback;
        OnListenerRemoved(eventType);
    }

    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    #endregion

    #region 广播事件

    /// <summary>
    /// 广播事件
    /// </summary>
    /// <param name="eventType"></param>
    public static void BroadCast(EventType eventType)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack callback = d as CallBack;
            if (callback != null)
            {
                callback();
            }
        }
    }

    public static void BroadCast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if(m_EventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callback = d as CallBack<T>;
            if(callback != null)
            {
                callback(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误: 事件{0}对应的委托有不同的类型", eventType));
            }
        }
    }

    #endregion

}