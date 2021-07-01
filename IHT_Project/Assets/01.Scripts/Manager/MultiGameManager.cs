using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MultiGameManager : MonoBehaviour
{
    public static MultiGameManager instance;

    [Header("인게임")]
    public GameObject gameObj;
    public CanvasGroup Looby;
    public CanvasGroup loobyPanel;
    public CanvasGroup roomPanel;

    public CanvasGroup winPanel;
    public CanvasGroup losePanel;

    public PlayerRPC myPlayerRPC;
    public PlayerRPC otherPlayerRPC;

    public FollowCam followCam;

    public GameObject playerPrefab;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    public int player1Wim = 0;
    public int player1MaxWim = 5;
    public int player2Wim = 0;
    public int player2MaxWim = 5;

    public Image p1GameImage = null;
    public Image p2GameImage = null;
    //private Dictionary<int, PlayerRPC> playerList = new Dictionary<int, PlayerRPC>();

    [Header("서버")]
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
    private bool needGameStart = false;

    private bool needLoginRefresh = false;

    private bool needJump = false;
    private bool needDash = false;
    private bool needAttack1 = false;
    private bool needAttack2 = false;
    private bool needAttack3 = false;

    private bool needSetDead = false;
    private bool needSetWin = false;

    public Queue<InputsVO> inputsQueue = new Queue<InputsVO>();
    public Queue<Vector3> transformQueue = new Queue<Vector3>();
    public Queue<string> errorQueue = new Queue<string>();
    public Queue<int> damagedQueue = new Queue<int>();
    public Queue<float> stunedQueue = new Queue<float>();
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
    public static void GameStart()
    {
        lock (instance.lockObj)
        {
            instance.needGameStart = true;
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
    public static void SetInputsData(InputsVO vo)
    {
        lock (instance.lockObj)
        {
            instance.inputsQueue.Enqueue(vo);
        }
    }
    public static void SetTransformData(Vector3 pos)
    {
        lock (instance.lockObj)
        {
            instance.transformQueue.Enqueue(pos);
        }
    }
    public static void SetJump()
    {
        lock (instance.lockObj)
        {
            instance.needJump = true;
        }
    }
    public static void SetDash()
    {
        lock (instance.lockObj)
        {
            instance.needDash = true;
        }
    }
    public static void SetAttack1()
    {
        lock (instance.lockObj)
        {
            instance.needAttack1 = true;
        }
    }
    public static void SetAttack2()
    {
        lock (instance.lockObj)
        {
            instance.needAttack2 = true;
        }
    }
    public static void SetAttack3()
    {
        lock (instance.lockObj)
        {
            instance.needAttack3 = true;
        }
    }
    public static void SetDamaged(int hp)
    {
        lock (instance.lockObj)
        {
            instance.damagedQueue.Enqueue(hp);
        }
    }
    public static void SetStuned(float stunTime)
    {
        lock (instance.lockObj)
        {
            instance.stunedQueue.Enqueue(stunTime);
        }
    }
    
    public static void SetDead()
    {
        lock (instance.lockObj)
        {
            instance.needSetDead = true;
        }
    }
    public static void SetWin()
    {
        lock (instance.lockObj)
        {
            instance.needSetWin = true;
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
            looby.GoRoom();
            needGoRoom = false;
        }
        if (needGoLooby)
        {
            looby.GoLooby();
            needGoLooby = false;
        }
        if (needLoginRefresh)
        {
            looby.Login();
            needLoginRefresh = false;
        }
        while (errorQueue.Count > 0)
        {
            string err = errorQueue.Dequeue();
            looby.ErrorPopup(err);
        }
        if (needGameStart)
        {
            OpenGame(true);
            SetPlayersGageUI();

            GameObject myPlayer = Instantiate(playerPrefab);
            myPlayerRPC = myPlayer.GetComponent<PlayerRPC>();
            //myPlayer.transform.position = CheckMaster() ? player1SpawnPoint.position : player2SpawnPoint.position;
            followCam.enabled = true;
            followCam.SetTrm(myPlayer.transform);
            myPlayerRPC.SetRPC(true, CheckMaster() ? player1SpawnPoint.position : player2SpawnPoint.position, CheckMaster() ? 1 : 2);
            myPlayer.tag = CheckMaster() ? "Player1" : "Player2";
            myPlayer.layer = CheckMaster() ? 10 : 11;

            GameObject otherPlayer = Instantiate(playerPrefab);
            otherPlayerRPC = otherPlayer.GetComponent<PlayerRPC>();
            //otherPlayer.transform.position = CheckMaster() ? player2SpawnPoint.position : player1SpawnPoint.position;
            otherPlayerRPC.SetRPC(false, CheckMaster() ? player2SpawnPoint.position : player1SpawnPoint.position, CheckMaster() ? 2 : 1);
            otherPlayer.tag = CheckMaster() ? "Player2" : "Player1";
            otherPlayer.layer = CheckMaster() ? 11 : 10;

            needGameStart = false;
        }
        while (inputsQueue.Count > 0)
        {
            InputsVO vo = inputsQueue.Dequeue();
            otherPlayerRPC.GetComponent<PlayerInputs>().SetInputs(vo.KeyHorizontalRaw);
            //instance.otherPlayerRPC.SetInputs(vo);
        }
        while (transformQueue.Count > 0)
        {
            Vector3 pos = transformQueue.Dequeue();
            otherPlayerRPC.SetPos(pos);
            //instance.otherPlayerRPC.SetInputs(vo);
        }
        if (needJump)
        {
            //ㅁㄴㅇㄹ
            otherPlayerRPC.Jump();
            needJump = false;
        }
        if (needDash)
        {
            //ㅁㄴㅇㄹ
            otherPlayerRPC.Dash();
            needDash = false;
        }
        if (needAttack1)
        {
            //ㅁㄴㅇㄹ
            otherPlayerRPC.Attack1();
            needAttack1 = false;
        }
        if (needAttack2)
        {
            //ㅁㄴㅇㄹ
            otherPlayerRPC.Attack2();
            needAttack2 = false;
        }
        if (needAttack3)
        {
            //ㅁㄴㅇㄹ
            otherPlayerRPC.Attack3();
            needAttack3 = false;
        }
        while (damagedQueue.Count > 0)
        {
            int hp = damagedQueue.Dequeue();
            otherPlayerRPC.SetHp(hp);
        }
        while (stunedQueue.Count > 0)
        {
            float stunTime = stunedQueue.Dequeue();
            otherPlayerRPC.SetStun(stunTime);
        }
        if (needSetDead)
        {
            otherPlayerRPC.Dead();
            needSetDead = false;
        }
        if (needSetWin)
        {
            Win();
            needSetWin = false;
        }
    }
    public void OpenGame(bool game)
    {
        roomPanel.interactable = !game;
        roomPanel.blocksRaycasts = !game;
        Looby.interactable = !game;
        Looby.blocksRaycasts = !game;
        DOTween.To(() => roomPanel.alpha, x => roomPanel.alpha = x, !game ? 1 : 0, 0.3f);
        DOTween.To(() => Looby.alpha, x => Looby.alpha = x, !game ? 1 : 0, 0.3f);
        gameObj.SetActive(true);
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
    public bool CheckMaster()
    {
        foreach (UserVO item in userList)
        {
            if (item.socketId == socketId)
            {
                return item.master;
            }
        }
        return false;
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
        looby.SetMaster(CheckMaster());
    }
    public void OnStartGameBtn()
    {
        RoomVO vo = new RoomVO();
        vo.roomNum = instance.roomNum;
        DataVO dataVO = new DataVO();
        dataVO.type = "GameStart";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
    }
    public void Win()
    {
        followCam.enabled = false;
        ResetGame();
        winPanel.interactable = true;
        winPanel.blocksRaycasts = true;
        DOTween.To(() => winPanel.alpha, x => winPanel.alpha = x, 1, 0.3f);
    }
    public static void DeadPlayer(int player)
    {
        switch (player)
        {
            case 1:
                instance.player2Wim++;
                break;
            case 2:
                instance.player1Wim++;
                break;
            default:
                break;
        }
        instance.SetPlayersGageUI();
        if(instance.player1Wim >= instance.player1MaxWim)
        {
            if(instance.myPlayerRPC.playerNum == 2)
            {
                instance.followCam.enabled = false;
                instance.ResetGame();
                instance.losePanel.interactable = true;
                instance.losePanel.blocksRaycasts = true;
                DOTween.To(() => instance.losePanel.alpha, x => instance.losePanel.alpha = x, 1, 0.3f);

                DataVO dataVO = new DataVO();
                dataVO.type = "LOSE";
                SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
            }
        }
        if(instance.player2Wim >= instance.player2MaxWim)
        {
            if (instance.myPlayerRPC.playerNum == 1)
            {
                instance.followCam.enabled = false;
                instance.ResetGame();
                instance.losePanel.interactable = true;
                instance.losePanel.blocksRaycasts = true;
                DOTween.To(() => instance.losePanel.alpha, x => instance.losePanel.alpha = x, 1, 0.3f);

                DataVO dataVO = new DataVO();
                dataVO.type = "LOSE";
                SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
            }
        }
    }
    private void ResetGame()
    {
        player1Wim = 0;
        player2Wim = 0;
        inputsQueue.Clear();
        transformQueue.Clear();
        Destroy(myPlayerRPC.gameObject);
        Destroy(otherPlayerRPC.gameObject);
    }
    public void ExitGame(bool isWin)
    {
        if (isWin)
        {
            winPanel.DOKill();
            winPanel.interactable = false;
            winPanel.blocksRaycasts = false;
            winPanel.alpha = 0;
        }
        else
        {
            losePanel.DOKill();
            losePanel.interactable = false;
            losePanel.blocksRaycasts = false;
            losePanel.alpha = 0;
        }
        gameObj.SetActive(false);
        this.Looby.interactable = true;
        this.Looby.blocksRaycasts = true;
        this.Looby.alpha = 1;
        loobyPanel.interactable = true;
        loobyPanel.blocksRaycasts = true;
        DOTween.To(() => loobyPanel.alpha, x => loobyPanel.alpha = x, 1, 0.3f);
    }
    private void SetPlayersGageUI()
    {
        p1GameImage.rectTransform.sizeDelta = new Vector2(Mathf.Clamp((float)player1Wim / (float)player1MaxWim, 0, 1) * 500, p1GameImage.rectTransform.rect.height);
        p2GameImage.rectTransform.sizeDelta = new Vector2(Mathf.Clamp((float)player2Wim / (float)player2MaxWim, 0, 1) * 500, p2GameImage.rectTransform.rect.height);
    }
    public void ExitMuti()
    {
        Destroy(SocketClient.instance);
        SceneManager.LoadScene(0);
    }
}
