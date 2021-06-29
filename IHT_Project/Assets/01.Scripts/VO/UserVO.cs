using UnityEngine;
using System;

[Serializable]
public class UserVO
{
    public string name;
    public int roomNum;
    public int socketId;
    public bool master;

    public UserVO(int roomNum, string name, int socketId, bool master)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.socketId = socketId;
        this.master = master;
    }

    public UserVO()
    {

    }
}
