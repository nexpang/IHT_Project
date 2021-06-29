using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.SetJump();
    }
}
