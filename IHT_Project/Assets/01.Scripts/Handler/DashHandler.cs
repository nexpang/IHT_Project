using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetDash();
    }
}