using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventType.MAINSKILL, HideSelf);
        EventCenter.AddListener<string>(EventType.TEST2, DebugFunction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickBtn()
    {
        EventCenter.BroadCast(EventType.MAINSKILL);
        EventCenter.BroadCast<string>(EventType.TEST2, "TestFunction");
    }

    public void DebugFunction(string str)
    {
        Debug.Log(str);
    }

    public void HideSelf()
    {
        Debug.Log("1111111111");
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.MAINSKILL, HideSelf);
        EventCenter.RemoveListener<string>(EventType.TEST2, DebugFunction);
    }
}
