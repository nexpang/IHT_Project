using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiGameManager : MonoBehaviour
{
    public static MultiGameManager instance;

    public int socketId = -1;
    public string socketName;
    public int roomNum;

    public object lockObj = new object(); // 데이터 락킹을 위한 오브젝트

    public Looby looby;

    private List<RoomVO> roomList;
    private bool needRoomRefresh = false;
    public GameObject roomPrefab;
    public Transform roomParent;

    private List<UserVO> userList;
    private bool needUserRefresh = false;
    public GameObject userPrefab;
    public Transform userParent;

    private bool needGoRoom = false;
    private bool needGoLooby = false;

    private bool needLoginRefresh = false;

    public Queue<string> errorQueue = new Queue<string>();
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
        if(socketId == -1)
        {
            instance.looby.Login();
        }
    }

    public static void SetRoomRefreshData(List<RoomVO> list)
    {
        lock (instance.lockObj)
        {
            instance.roomList = list;
            instance.needRoomRefresh = true;
        }
    }
    public static void SetUserRefreshData(List<UserVO> list)
    {
        lock (instance.lockObj)
        {
            foreach (UserVO item in list)
            {
                if(item.socketId == instance.socketId)
                {
                    instance.roomNum = item.roomNum;
                }
            }
            instance.userList = list;
            instance.needUserRefresh = true;
        }
    }
    public static void GoRoom()
    {
        lock (instance.lockObj)
        {
            instance.needGoRoom = true;
        }
    }
    public static void GoLooby()
    {
        lock (instance.lockObj)
        {
            instance.needGoLooby = true;
        }
    }
    public static void SetLoginData(string name, int socketId)
    {
        lock (instance.lockObj)
        {
            instance.needLoginRefresh = true;
            instance.socketName = name;
            instance.socketId = socketId;
        }
    }
    public static void SetPopupError(string vo)
    {
        lock (instance.lockObj)
        {
            instance.errorQueue.Enqueue(vo);
        }
    }

    void Update()
    {
        if (needRoomRefresh)
        {
            //Debug.Log(roomList[0]);
            //roomParent.GetChildCount;
            ResetRoom();
            needRoomRefresh = false;
        }
        if (needUserRefresh)
        {
            //Debug.Log(roomList[0]);
            //roomParent.GetChildCount;
            ResetUser();
            needRoomRefresh = false;
        }
        if (needGoRoom)
        {
            instance.looby.GoRoom();
            needGoRoom = false;
        }
        if (needGoLooby)
        {
            instance.looby.GoLooby();
            needGoLooby = false;
        }
        if (needLoginRefresh)
        {
            instance.looby.Login();
            needLoginRefresh = false;
        }
        while (errorQueue.Count > 0)
        {
            string err = errorQueue.Dequeue();
            instance.looby.ErrorPopup(err);
        }

    }
    public static void Login(string name)
    {
        LoginVO vo = new LoginVO();
        vo.name = name;
        DataVO dataVO = new DataVO();
        dataVO.type = "LOGIN";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public static void CreateRoom(string name)
    {
        Debug.Log("버튼 생성");
        RoomVO vo = new RoomVO();
        vo.name = name;
        DataVO dataVO = new DataVO();
        dataVO.type = "CREATE_ROOM";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        //instance.looby.GoRoom();
    }
    public static void JoinRoom(int roomNum)
    {
        RoomVO vo = new RoomVO();
        vo.roomNum = roomNum;
        DataVO dataVO = new DataVO();
        dataVO.type = "JOIN_ROOM";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        //instance.looby.GoRoom();
    }
    public static void ExitRoom()
    {
        RoomVO vo = new RoomVO();
        vo.roomNum = instance.roomNum;
        instance.roomNum = 0;
        DataVO dataVO = new DataVO();
        dataVO.type = "EXIT_ROOM";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void ResetRoom()
    {
        for (int i = 0; i < roomParent.childCount; i++)
        {
            Destroy(roomParent.GetChild(i).gameObject);
        }
        foreach (RoomVO rv in roomList)
        {
            Room room = Instantiate(roomPrefab, roomParent).GetComponent<Room>();
            room.SetRoomInfo(rv.name, rv.number, rv.roomNum);
        }
    }
    public void ResetUser()
    {
        for (int i = 0; i < userParent.childCount; i++)
        {
            Destroy(userParent.GetChild(i).gameObject);
        }
        foreach (UserVO rv in userList)
        {
            User user = Instantiate(userPrefab, userParent).GetComponent<User>();
            user.SetUserInfo(rv.name);
        }
    }
    public void ExitMuti()
    {
        SceneManager.LoadScene(0);
    }
}
