using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandler : MonoBehaviour, IMsgHandler
{
    public void HandleMsg(string payload)
    {
        TransformVO vo = JsonUtility.FromJson<TransformVO>(payload);
        MultiGameManager.SetTransformData(vo.pos);
    }
}
