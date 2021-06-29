using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        DamagedVO vo = JsonUtility.FromJson<DamagedVO>(payload);
        MultiGameManager.SetDamaged(vo.hp);
    }
}
