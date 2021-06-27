using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomExit : MonoBehaviour
{
    private Button thisBtn;

    private void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(() =>
        {
            MultiGameManager.ExitRoom();
        });
    }
}
