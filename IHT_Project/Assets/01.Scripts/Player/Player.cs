using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private static Player instance = null;
    public FollowCam followCam;
    public Scrolling[] scrolling;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioAttack1;
    [SerializeField] private AudioClip audioAttack2;
    [SerializeField] private AudioClip audioAttack3;

    [Header("공격 관련 변수"), SerializeField]
    private int[] attackDamage;
    [SerializeField]
    private float sturnTime;
    [SerializeField]
    private Vector2 attack1size;
    [SerializeField]
    private Vector2 attack1offset;
    [SerializeField]
    private Vector2 attack2size;
    [SerializeField]
    private Vector2 attack2offset;
    [SerializeField]
    private Vector2 attack3size;
    [SerializeField]
    private Vector2 attack3offset;
    [SerializeField]
    private LayerMask attackedLayer; 

    private Vector2 direction = Vector2.one;

    private void Awake()
    {
        //instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlaySound(AudioClip ac)
    {
        audioSource.clip = ac;
        audioSource.Play();
    }

    public void Attack1(bool isRight)
    {
        PlaySound(audioAttack1);
        if (isRight)
            direction = Vector2.one;
        else
            direction = new Vector2(-1f,1f);
        AttackCollision(atkOffset: attack1offset*direction, atkSize: attack1size, 1);
    }
    public void Attack2(bool isRight)
    {
        PlaySound(audioAttack2);
        //Debug.Log("공격2");
        if (isRight)
            direction = Vector2.one;
        else
            direction = new Vector2(-1f, 1f);
        AttackCollision(atkOffset: attack2offset * direction, atkSize: attack2size, 2);
    }
    public void Attack3(bool isRight)
    {
        PlaySound(audioAttack3);
        //Debug.Log("공격3");
        if (isRight)
            direction = Vector2.one;
        else
            direction = new Vector2(-1f, 1f);
        AttackCollision(atkOffset: attack3offset * direction, atkSize: attack3size, 3);
    }


    private void AttackCollision(Vector3 atkOffset, Vector2 atkSize, int attackNum)
    {
        Vector2 attackPoint = gameObject.transform.position + atkOffset;

        if (GetComponent<PlayerInputs>().isSingle)
        {
            Collider2D hitEnemie = Physics2D.OverlapBox(attackPoint, atkSize, 0, attackedLayer);

            if (hitEnemie != null)
            {
                Enemy enemy = hitEnemie.GetComponent<Enemy>();
                switch (attackNum)
                {
                    case 1:
                    case 2:
                        enemy.OnDamaged(attackDamage[attackNum - 1], attackNum);
                        break;
                    case 3:
                        enemy.OnStuned(2.5f);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            LayerMask layer = gameObject.layer == 10 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1");
            //Debug.Log(layer);
            Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint, atkSize, 0, layer);
            //Debug.Log(hitPlayer);

            if (hitPlayer != null)
            {
                PlayerHealth player = hitPlayer.GetComponent<PlayerHealth>();
                if (player == GetComponent<PlayerHealth>())
                    return;
                switch (attackNum)
                {
                    case 1:
                    case 2:
                        player.OnDamage(attackDamage[attackNum - 1]);
                        break;
                    case 3:
                        player.OnStuned(1.5f);
                        break;
                    default:
                        break;
                }
            }
        }
        /*
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint, atkSize, 0, attackedLayer);

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            Enemy enemy = hitEnemies[i].GetComponent<Enemy>();
            switch (attackNum)
            {
                case 1:
                case 2:
                    enemy.OnDamaged(attackDamage[attackNum-1]);
                    break;
                case 3:
                    enemy.OnStuned(0f); 
                    break;
                default:
                    break;
            }
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone") )
        {
            if (GetComponent<PlayerInputs>().isSingle)
            {
                StageManager.OnPlayerDamage(-1);
                transform.position = Vector3.zero;
            }
            else if(!(GetComponent<PlayerHealth>().isRemote))
            {
                GetComponent<PlayerHealth>().OnDamage(3);
            }
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 playerPos = gameObject.transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playerPos + (attack1offset*direction), attack1size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerPos + (attack2offset * direction), attack2size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playerPos + (attack3offset * direction), attack3size);
    }
}
