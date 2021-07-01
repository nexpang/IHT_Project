using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public bool Keyjump = false;
    public float KeyHorizontalRaw = 0f;
    public bool KeyDash = false;
    public bool KeyAttack1 = false;
    public bool KeyAttack2 = false;
    public bool KeyAttack3 = false;

    public bool isMyPlayer = true;
    public bool isSingle = true;
    public float beforeHorizontalRaw = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (isMyPlayer)
        {

            if (GetComponent<PlayerController>().state != PlayerState.STUNED || !GetComponent<PlayerHealth>().isDead)
            {
                Keyjump = Input.GetButtonDown("Jump");
                KeyHorizontalRaw = Input.GetAxisRaw("Horizontal");
                KeyDash = Input.GetKeyDown(KeyCode.LeftShift);
                KeyAttack1 = Input.GetMouseButtonDown(0);
                KeyAttack2 = Input.GetMouseButtonDown(1);
                KeyAttack3 = Input.GetMouseButtonDown(2);
            }
            else
            {
                KeyHorizontalRaw = 0f;
                Keyjump = false;
                KeyDash = false;
                KeyAttack1 = false;
                KeyAttack2 = false;
                KeyAttack3 = false;
            }
            
            if (!isSingle)
            {
                if (beforeHorizontalRaw != KeyHorizontalRaw)
                {
                    beforeHorizontalRaw = KeyHorizontalRaw;
                    InputsVO vo = new InputsVO(KeyHorizontalRaw);
                    DataVO dataVO = new DataVO();
                    dataVO.type = "INPUTS";
                    dataVO.payload = JsonUtility.ToJson(vo);
                    SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
                }
            }
        }
    }

    public void SetInputs(float KeyHorizontalRaw)
    {
        if (isMyPlayer)
            return;
        //Debug.Log("점프 :" + Keyjump + "대쉬 :" +KeyDash);
        //this.Keyjump = Keyjump;
        this.KeyHorizontalRaw = KeyHorizontalRaw;
        //this.KeyDash = KeyDash;
        //this.KeyAttack1 = KeyAttack1;
        //this.KeyAttack2 = KeyAttack2;
        //this.KeyAttack3 = KeyAttack3;
    }
}
