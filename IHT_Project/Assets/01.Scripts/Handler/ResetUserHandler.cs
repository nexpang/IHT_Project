using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUserHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        ResetUserVO userList = JsonUtility.FromJson<ResetUserVO>(payload);
        MultiGameManager.SetUserRefreshData(userList.dataList);
    }
}