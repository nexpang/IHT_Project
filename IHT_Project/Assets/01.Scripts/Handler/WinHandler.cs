using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetWin();
    }
}

