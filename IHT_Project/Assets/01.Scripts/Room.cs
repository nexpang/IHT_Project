using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Text nameTxt;
    public Text numberTxt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomInfo(string name, int number)
    {
        nameTxt.text = name;
        numberTxt.text = $"{number} / 2";
    }
}
