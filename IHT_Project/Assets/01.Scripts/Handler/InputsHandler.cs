using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        InputsVO vo = JsonUtility.FromJson<InputsVO>(payload);
        MultiGameManager.SetInputsData(vo);
    }
}