using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunedHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        StunedVO vo = JsonUtility.FromJson<StunedVO>(payload);
        MultiGameManager.SetStuned(vo.stunTime);
    }
}