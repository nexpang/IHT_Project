using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public bool isRemote;
    public bool isDead;
    public Image[] iHps = null;
    public Sprite[] sHp = null;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Spawn()
    {
        hp = 3;
        SetHp();
        isDead = false;
    }
    public void SetHp()
    {
        for(int i = 0; i < hp; i++)
        {
            iHps[i].sprite = sHp[0];
        }
        for (int i = 2; i >= hp; i--)
        {
            if (hp < 0)
                break;
            iHps[i].sprite = sHp[1];
        }
    }
    public void OnStuned(float stunTime)
    {

    }
    public void OnDamage(int damage)
    {
        if (isRemote) return;
        hp-= damage;
        if(hp >= 0)
            SetHp();

        DamagedVO vo = new DamagedVO(hp);
        DataVO dataVO = new DataVO();
        dataVO.type = "DAMAGED";
        dataVO.payload = JsonUtility.ToJson(vo);
        SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));

        if (hp <= 0)
        {
            Dead();
        }
    }
    public void Dead()
    {
        if (isDead) return;
        isDead = true;
        PlayerRPC rpc = GetComponent<PlayerRPC>();
        if (!isRemote)
        {
            DataVO dataVO = new DataVO();
            dataVO.type = "DEAD";
            //dataVO.payload = JsonUtility.ToJson(vo);
            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }

        MultiGameManager.DeadPlayer(rpc.playerNum);
        rpc.Spawn();
    }
}
