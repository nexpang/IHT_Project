using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 31234;

    public GameObject handers;

    private WebSocket webSocket; // 웹 소켓 인스턴스

    public static SocketClient instance;

    public static void SendDataToSocket(string json)
    {
        instance.SendData(json);
    }
    private Dictionary<string, IMsgHandler> handlerDictionary;
    private void Awake()
    {
        instance = this;
        handlerDictionary = new Dictionary<string, IMsgHandler>();
    }
    void Start()
    {
        handlerDictionary.Add("CHAT", handers.GetComponent<ChatHandler>());
        handlerDictionary.Add("LOGIN", handers.GetComponent<LoginHandler>());
        handlerDictionary.Add("RESET_ROOM", handers.GetComponent<ResetRoomHandler>());
        handlerDictionary.Add("GO_ROOM", handers.GetComponent<GoRoomHandler>());
        handlerDictionary.Add("GO_LOOBY", handers.GetComponent<GoLoobyHandler>());
        handlerDictionary.Add("RESET_USER", handers.GetComponent<ResetUserHandler>());
        handlerDictionary.Add("GameStart", handers.GetComponent<GameStartHandler>());
        handlerDictionary.Add("INPUTS", handers.GetComponent<InputsHandler>());
        handlerDictionary.Add("TRANSFORM", handers.GetComponent<TransformHandler>());
        handlerDictionary.Add("JUMP", handers.GetComponent<JumpHandler>());
        handlerDictionary.Add("DASH", handers.GetComponent<DashHandler>());
        handlerDictionary.Add("ATTACK1", handers.GetComponent<Attack1Handler>());
        handlerDictionary.Add("ATTACK2", handers.GetComponent<Attack2Handler>());
        handlerDictionary.Add("ATTACK3", handers.GetComponent<Attack3Handler>());
        handlerDictionary.Add("DAMAGED", handers.GetComponent<DamagedHandler>());
        handlerDictionary.Add("DEAD", handers.GetComponent<DeadHandler>());
        handlerDictionary.Add("WIN", handers.GetComponent<WinHandler>());
        handlerDictionary.Add("ERROR", handers.GetComponent<ErrorHandler>());


        
        webSocket = new WebSocket($"{url}:{port}");
        /*
        webSocket.Connect();

        //webSocketState.
        webSocket.OnMessage += (sender, e) =>
        {
            ReceiveData((WebSocket)sender, e);
        };*/
    }
    public void ConnectSocket(string ip, string port)
    {
        webSocket = new WebSocket($"ws://{ip}:{port}");
        webSocket.Connect();

        //webSocketState.
        webSocket.OnMessage += (sender, e) =>
        {
            ReceiveData((WebSocket)sender, e);
        };
    }

    private void ReceiveData(WebSocket sender, MessageEventArgs e)
    {
        DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);

        IMsgHandler handler = null;
        if (handlerDictionary.TryGetValue(vo.type, out handler))
        {
            handler.HandleMsg(vo.payload);
        }
        else
        {
            Debug.LogWarning("존재하지 않은 프로토콜 요청" + vo.type);
        }
    }

    private void SendData(string json)
    {
        webSocket.Send(json);
    }

    private void OnDestroy()
    {
        //webSocket.Close();
        //if (webSocket.ReadyState == WebSocketState.Connecting)
        webSocket.Close();
    }
}
