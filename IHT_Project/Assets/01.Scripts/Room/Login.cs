using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    public InputField nameInput;

    private Button thisBtn;

    private void Start()
    {
        thisBtn = GetComponent<Button>();
        thisBtn.onClick.AddListener(() =>
        {
            MultiGameManager.Login(nameInput.text);
            nameInput.text = "";
        });
    }
}
