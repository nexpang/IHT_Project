using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketClient : MonoBehaviour
{
    public string url = "ws://localhost";
    public int port = 36589;

    public GameObject handers;

    private WebSocket webSocket; // �� ���� �ν��Ͻ�

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
        handlerDictionary.Add("ERROR", handers.GetComponent<ErrorHandler>());


        webSocket = new WebSocket($"{url}:{port}");
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
            Debug.LogWarning("�������� ���� �������� ��û" + vo.type);
        }
    }

    private void SendData(string json)
    {
        webSocket.Send(json);
    }

    private void OnDestroy()
    {
        webSocket.Close();
        //if (webSocket.ReadyState == WebSocketState.Connecting)
        //    webSocket.Close();
    }
}