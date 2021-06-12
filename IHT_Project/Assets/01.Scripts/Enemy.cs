using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    ACTIVE=0,
    DEAD=1,
    STUN=2
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyState state;
    [SerializeField]
    int hp;

    [SerializeField]
    float speed;

    private Vector3 moveDirection = Vector3.left;

    [SerializeField]
    private GameObject stunEffect = null;
    [SerializeField]
    private LayerMask isPlayer;

    [SerializeField]
    private int id = 1;

    

    void Start()
    {

    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Move();
    }

    public void SetDirection(bool isRight = true)
    {
        if (isRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            moveDirection = Vector3.left;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            moveDirection = Vector3.right;
        }
    }

    public void Move()
    {
        if (state == EnemyState.STUN || state == EnemyState.DEAD)
            return;
        gameObject.transform.position += moveDirection * speed * Time.fixedDeltaTime;
    }

    public void OnDamaged(int damage)
    {
        Dead();
        Debug.Log("나 맞음");
    }
    public void OnStuned(float stunTime)
    {
        state = EnemyState.STUN;
        stunEffect.SetActive(true);
        Invoke("Unstun", stunTime);
    }
    public void Unstun()
    {
        if(state == EnemyState.STUN)
        {
            state = EnemyState.ACTIVE;
            stunEffect.SetActive(false);
        }
    }

    private void Dead()
    {
        if (state == EnemyState.STUN)
            stunEffect.SetActive(false);
        state = EnemyState.DEAD;
        EnemyPooling.ReturnObject(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == EnemyState.STUN || state == EnemyState.DEAD)
            return;
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("나 플레이어한테 닿음");

            StageManager.OnPlayerDamage(id);
            Dead();
        }
        if (collision.gameObject.tag == "Core")
        {
            //Debug.Log("나 닿음");
            Dead();
        }
    }
}
