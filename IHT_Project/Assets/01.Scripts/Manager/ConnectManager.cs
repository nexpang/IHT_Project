using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    public InputField txtIp;
    public InputField txtport;

    public Button btnConnect;
    public CanvasGroup connetPanel;

    bool isConnected = false;

    public Looby looby;
    void Start()
    {
        btnConnect.onClick.AddListener(() =>
        {
            if (isConnected) return;
            if (txtport.text == "" || txtIp.text == "")
            {
                looby.ErrorPopup("필수 값이 빠져있습니다");
                return;
            }

            SocketClient.instance.ConnectSocket(txtIp.text, txtport.text);
            connetPanel.alpha = 0;
            connetPanel.interactable = false;
            connetPanel.blocksRaycasts = false;
            looby.UIOpen(looby.LoginPanel,true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
