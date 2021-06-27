using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        //Debug.Log(payload);
        LoginVO login = JsonUtility.FromJson<LoginVO>(payload);
        MultiGameManager.SetLoginData(login.name, login.socketId);
    }
}
