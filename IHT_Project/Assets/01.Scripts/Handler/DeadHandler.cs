using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetDead();
    }
}