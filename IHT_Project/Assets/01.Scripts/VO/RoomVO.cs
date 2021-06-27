using UnityEngine;
using System;

[Serializable]
public class RoomVO
{
    public string name;
    public int roomNum;
    public int number;

    public RoomVO(string name, int roomNum, int number)
    {
        this.name = name;
        this.roomNum = roomNum;
        this.number = number;
    }

    public RoomVO()
    {

    }
}
