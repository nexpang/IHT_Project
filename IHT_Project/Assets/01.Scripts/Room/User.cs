using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public Text nameTxt;
    public void SetUserInfo(string name)
    {
        nameTxt.text = name;
    }
}
