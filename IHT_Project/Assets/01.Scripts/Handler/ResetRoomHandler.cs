using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRoomHandler : MonoBehaviour, IMsgHandler {
    public void HandleMsg(string payload)
    {
        Debug.Log(payload);
        ResetRoomVO roomList = JsonUtility.FromJson<ResetRoomVO>(payload);
        Debug.Log(roomList.dataList.Count);
        foreach (var item in roomList.dataList)
        {
            Debug.Log(item.name);
        }
        MultiGameManager.SetRefreshData(roomList.dataList); // ¹ö±×
        Debug.Log(roomList.dataList);
    }
}
