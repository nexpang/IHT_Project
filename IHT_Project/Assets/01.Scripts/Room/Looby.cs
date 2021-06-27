using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Looby : MonoBehaviour
{
    public CanvasGroup LoginPanel;
    [Header("로비 관련")]
    public CanvasGroup cRoomPopup;
    public CanvasGroup loobyPanel;
    [Header("방 관련")]
    public CanvasGroup RoomPanel;

    [Header("에러 팝업 관련")]
    public CanvasGroup errorPopup;
    public Text errorTxt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ErrorPopup(string error)
    {
        UIOpen(errorPopup, true);
        errorTxt.text = error;
        Invoke("CloseError", 3f);
    }
    public void CloseError()
    {
        errorTxt.text = "";
        UIOpen(errorPopup, false);
    }
    public void Popup(bool isOpen)
    {
        UIOpen(cRoomPopup, isOpen);
    }
    public void Login()
    {
        UIOpen(LoginPanel, false);
        UIOpen(loobyPanel, true);
    }
    public void GoLooby()
    {
        UIOpen(RoomPanel, false);
        UIOpen(loobyPanel, true);
    }
    public void GoRoom()
    {
        UIOpen(loobyPanel, false);
        UIOpen(RoomPanel, true);
    }
    public void UIOpen(CanvasGroup cvsGroup, bool isOpen)
    {
        cvsGroup.interactable = isOpen;
        cvsGroup.blocksRaycasts = isOpen;
        DOTween.To(() => cvsGroup.alpha, x => cvsGroup.alpha = x, isOpen ? 1 : 0, 0.3f);
    }
}
