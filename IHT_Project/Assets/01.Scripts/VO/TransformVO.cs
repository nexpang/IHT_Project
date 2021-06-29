using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TransformVO
{
    public int roomNum;
    public Vector3 pos;

    public TransformVO(int roomNum, Vector3 pos)
    {
        this.roomNum = roomNum;
        this.pos = pos;
    }

    public TransformVO()
    {

    }
}
