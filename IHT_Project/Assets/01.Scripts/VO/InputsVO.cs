using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InputsVO
{
    //public bool Keyjump = false;
    public float KeyHorizontalRaw = 0f;
    //public bool KeyDash = false;
    //public bool KeyAttack1 = false;
    //public bool KeyAttack2 = false;
    //public bool KeyAttack3 = false;
    public InputsVO()
    {

    }

    public InputsVO(float KeyHorizontalRaw)
    {
        this.KeyHorizontalRaw = KeyHorizontalRaw;
    }
}
