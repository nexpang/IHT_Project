using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRoomHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        MultiGameManager.GoRoom();
    }
}