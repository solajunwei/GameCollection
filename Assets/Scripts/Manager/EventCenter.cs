
using System;
using System.Collections.Generic;



public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    #region ��Ӽ���

    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }

        Delegate d = m_EventTable[eventType];
        if(d != null && d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("����Ϊ�¼�{0}��Ӳ�ͬ���͵�ί�У���ǰ�¼�����Ӧ��ί����{1},Ҫ��ӵ�ί������Ϊ{2}", eventType, d.GetType(), callBack.GetType()));
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


    #region �Ƴ�����

    public static void OnListenerRemoving(EventType eventType, Delegate callback)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate d = m_EventTable[eventType];
            if (d == null)
            {
                throw new Exception(string.Format("�Ƴ���������: �¼�{0}û�ж�Ӧ��ί��", eventType));
            }
            else if(d.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("�Ƴ���������: ����Ϊ�¼�{0}�Ƴ���ͬ���͵�ί�У���ǰί������Ϊ{1}��Ҫ�Ƴ�������Ϊ{2}", eventType, d.GetType(), callback.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("�Ƴ���������: û���¼���{0}", eventType));
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

    #region �㲥�¼�

    /// <summary>
    /// �㲥�¼�
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
                throw new Exception(string.Format("�㲥�¼�����: �¼�{0}��Ӧ��ί���в�ͬ������", eventType));
            }
        }
    }

    #endregion

}