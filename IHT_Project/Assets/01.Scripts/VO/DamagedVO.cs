using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamagedVO
{
    public int hp;

    public DamagedVO(int hp)
    {
        this.hp = hp;
    }
}
