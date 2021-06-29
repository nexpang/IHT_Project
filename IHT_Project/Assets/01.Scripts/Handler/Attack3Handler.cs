using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Handler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetAttack3();
    }
}
