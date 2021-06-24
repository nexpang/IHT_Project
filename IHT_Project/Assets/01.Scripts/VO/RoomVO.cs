using UnityEngine;
using System;

[Serializable]
public class RoomVO
{
    public string name;
    public int number;

    public RoomVO(string name, int number)
    {
        this.name = name;
        this.number = number;
    }

    public RoomVO()
    {

    }
}
