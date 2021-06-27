using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRoomHandler : MonoBehaviour, IMsgHandler {
    public void HandleMsg(string payload)
    {
        //Debug.Log(payload);
        ResetRoomVO roomList = JsonUtility.FromJson<ResetRoomVO>(payload);
        MultiGameManager.SetRoomRefreshData(roomList.dataList);
    }
}
