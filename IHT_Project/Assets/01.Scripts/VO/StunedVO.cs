using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StunedVO
{
    public float stunTime;
    public StunedVO(float stunTime)
    {
        this.stunTime = stunTime;
    }
}
