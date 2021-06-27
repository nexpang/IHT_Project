using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLoobyHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.GoLooby();
    }
}