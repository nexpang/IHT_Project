using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJellyBear : Enemy
{
    [SerializeField]
    private float jumpForce = 1f;
    [SerializeField]
    private float jumpDelay = 1f;
    private bool canJump = true;
    [SerializeField]
    private GameObject guard;

    [SerializeField]
    private AudioClip audioGuard;
    private new void FixedUpdate()
    {
        Move();
    }
    public override void SetDirection(bool isRight = true)
    {
        base.SetDirection(isRight);
        guard.SetActive(true);
    }

    public override void OnDamaged(int damage, int attackNum)
    {
        if(state == EnemyState.STUN || attackNum==2)
        {
            audioSource.clip = audioHit;
            audioSource.Play();
            StartCoroutine(WaitDead());
        }
        else
        {
            audioSource.clip = audioGuard;
            audioSource.Play();
        }
    }
    public override void OnStuned(float stunTime)
    {
        base.OnStuned(stunTime);
        if(state != EnemyState.DEAD)
            guard.SetActive(false);
    }
    public override void Unstun()
    {
        base.Unstun();
        guard.SetActive(true);
    }

    public new void Move()
    {
        if (state == EnemyState.STUN || state == EnemyState.DEAD)
            return;
        base.Move();
        if (!canJump)
        {
            return;
        }
        canJump = false;
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Invoke("JumpDelay", jumpDelay);
        //StartCoroutine(JumpMove());
    }

    private void JumpDelay()
    {
        canJump = true;
    }
}
