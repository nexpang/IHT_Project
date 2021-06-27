using UnityEngine;
using System;

[Serializable]
public class UserVO
{
    public string name;
    public int roomNum;
    public int socketId;

    public UserVO(int roomNum, string name, int socketId)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.socketId = socketId;
    }

    public UserVO()
    {

    }
}
