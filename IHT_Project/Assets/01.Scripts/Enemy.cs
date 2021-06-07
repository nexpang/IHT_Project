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

    [SerializeField]
    private LayerMask isPlayer;

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

    public void Move()
    {
        if (state == EnemyState.STUN)
            return;
        gameObject.transform.position += Vector3.left * speed * Time.fixedDeltaTime;
    }

    public void OnDamaged(int damage)
    {
        Debug.Log("나 맞음");
    }
    public void OnStuned(float stunTime)
    {
        state = EnemyState.STUN;
        Invoke("Unstun", stunTime);
    }
    public void Unstun()
    {
        if(state == EnemyState.STUN)
            state = EnemyState.ACTIVE;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("나 플레이어한테 닿음");
            Player.OnDamaged();
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("나 플레이어한테 닿음");
            Player.OnDamaged();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Core")
        {
            //Debug.Log("나 닿음");
            Destroy(gameObject);
        }
    }
}
