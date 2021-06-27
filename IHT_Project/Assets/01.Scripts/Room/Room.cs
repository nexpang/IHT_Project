using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Text nameTxt;
    public Text numberTxt;
    public int roomNum;

    private Button thisBtn;
    void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(() =>
        {
            MultiGameManager.JoinRoom(roomNum);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomInfo(string name, int number, int roomNum)
    {
        nameTxt.text = name;
        numberTxt.text = $"{number} / 2";
        this.roomNum = roomNum;
    }
}
