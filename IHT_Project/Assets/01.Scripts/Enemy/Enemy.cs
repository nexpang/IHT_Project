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
    protected EnemyState state;
    [SerializeField]
    int hp;

    [SerializeField]
    float speed;

    protected Vector3 moveDirection = Vector3.left;

    [SerializeField]
    protected GameObject stunEffect = null;
    [SerializeField]
    protected LayerMask isPlayer;

    protected Rigidbody2D rigid;

    [SerializeField]
    public int id = 1;

    

    protected void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        state = EnemyState.ACTIVE;
    }

    void Update()
    {
        
    }
    protected void FixedUpdate()
    {
        Move();
    }

    public virtual void SetDirection(bool isRight = true)
    {
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
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

    public virtual void OnDamaged(int damage, int attackNum)
    {
        Dead();
        //Debug.Log("죽음");
    }
    public virtual void OnStuned(float stunTime)
    {
        state = EnemyState.STUN;
        stunEffect.SetActive(true);
        Invoke("Unstun", stunTime);
    }
    public virtual void Unstun()
    {
        if(state == EnemyState.STUN)
        {
            state = EnemyState.ACTIVE;
            stunEffect.SetActive(false);
        }
    }

    public void Dead()
    {
        if (state == EnemyState.STUN)
            Unstun();
        state = EnemyState.DEAD;
        EnemyPooling.ReturnObject(this, id);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == EnemyState.STUN || state == EnemyState.DEAD)
                return;
            //Debug.Log("나 플레이어한테 닿음");

            StageManager.OnPlayerDamage(id);
            Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == EnemyState.DEAD)
            return;
        //if (collision.gameObject.tag == "Player")
        //{
        //    //Debug.Log("나 플레이어한테 닿음");

        //    StageManager.OnPlayerDamage(id);
        //    Dead();
        //}
        if (collision.CompareTag("Core"))
        {
            //Debug.Log("나 닿음");
            StageManager.OnCoreDamage(id);
            Dead();
        }
        if(collision.CompareTag("DeathZone"))
        {
            //Debug.Log("???");
            Dead();
        }
    }
}
