using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    public InputField nameInput;

    private Button thisBtn;

    private void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(() =>
        {
            MultiGameManager.CreateRoom(nameInput.text);
            nameInput.text = "";
        });
    }
}
