using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGameManager : MonoBehaviour
{
    public static MultiGameManager instance;

    public object lockObj = new object(); // 데이터 락킹을 위한 오브젝트

    private List<RoomVO> roomList;
    private bool needRoomRefresh = false;
    public GameObject roomPrefab;
    public Transform roomParent;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 게임매니져가 실행되고 있습니다.");
        }
        instance = this;
    }
    void Start()
    {

    }

    public static void SetRefreshData(List<RoomVO> list)
    {
        lock (instance.lockObj)
        {
            instance.roomList = list;
            instance.needRoomRefresh = true;
        }
    }

    void Update()
    {
        if (needRoomRefresh)
        {
            //Debug.Log(roomList[0]);
            foreach (RoomVO rv in roomList)
            {
                Room room = Instantiate(roomPrefab, roomParent).GetComponent<Room>();
                room.SetRoomInfo(rv.name, rv.number);
            }
            needRoomRefresh = false;
        }

    }
}
