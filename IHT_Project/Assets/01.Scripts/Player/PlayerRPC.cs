using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRPC : MonoBehaviour
{
    public int playerNum;
    public bool isMyPlayer;
    public Vector3 spawnPoint;
    private PlayerInputs inputs;
    private PlayerHealth health;
    private PlayerController controller;
    private void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        health = GetComponent<PlayerHealth>();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (isMyPlayer)
        {
            StartCoroutine(SendTransform());
        }
    }
    public void SetPos(Vector3 pos)
    {
        if (isMyPlayer) return;
        float distance = Vector3.Distance(transform.position, pos);
        if (distance > 1f)
        {
            transform.position = pos;
        }
    }
    IEnumerator SendTransform()
    {
        int roomNum = MultiGameManager.instance.roomNum;

        while (true)
        {
            yield return 0.1f;
            TransformVO vo = new TransformVO(roomNum, transform.position);

            DataVO dataVO = new DataVO();
            dataVO.type = "TRANSFORM";
            dataVO.payload = JsonUtility.ToJson(vo);
            SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
        }
    }
    public void SetRPC(bool isMyPlayer, Vector3 spawnPoint, int player)
    {
        this.playerNum = player;
        this.isMyPlayer = isMyPlayer;
        this.spawnPoint = spawnPoint;

        inputs.isMyPlayer = isMyPlayer;
        inputs.isSingle = false;
        controller.isSingle = false;
        health.isRemote = !isMyPlayer;
        Spawn();
    }
    public void SetHp(int hp)
    {
        health.hp = hp;
        health.SetHp();
    }
    public void SetStun(float stunTime)
    {
        health.OnStun(stunTime);
    }
    public void Dead()
    {
        health.Dead();
    }
    public void Jump()
    {
        controller.isJump = true;
    }
    public void Dash()
    {
        controller.isDash = true;
        controller.animator.SetTrigger("skill");
    }
    public void Attack1()
    {
        controller.isAttack1 = true;
        controller.animator.speed = 2f;
        controller.animator.SetTrigger("attack1");
    }
    public void Attack2()
    {
        controller.isAttack2 = true;
        controller.animator.SetTrigger("attack3");
    }
    public void Attack3()
    {
        controller.isAttack3 = true;
        controller.animator.SetTrigger("attack2");
    }

    public void Spawn()
    {
        transform.position = spawnPoint;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        health.Spawn();
    }
}
