using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1Handler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetAttack1();
    }
}
