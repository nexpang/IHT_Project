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


    [SerializeField]
    protected AudioClip audioHit;
    protected AudioSource audioSource;

    protected BoxCollider2D boxCollider;

    protected void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        state = EnemyState.ACTIVE;
        stunEffect.SetActive(false);
    }

    public void Move()
    {
        if (state == EnemyState.STUN || state == EnemyState.DEAD)
            return;
        gameObject.transform.position += moveDirection * speed * Time.fixedDeltaTime;
    }

    public virtual void OnDamaged(int damage, int attackNum)
    {
        audioSource.clip = audioHit;
        audioSource.Play();
        StartCoroutine(WaitDead());
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

    public IEnumerator WaitDead()
    {
        state = EnemyState.DEAD;
        boxCollider.isTrigger = true;
        yield return new WaitForSeconds(2f);
        boxCollider.isTrigger = false;
        if (state == EnemyState.STUN)
            Unstun();
        EnemyPooling.ReturnObject(this, id);
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
        if (state == EnemyState.DEAD)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == EnemyState.STUN)
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
