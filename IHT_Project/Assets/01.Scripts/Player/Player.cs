using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance = null;

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
        instance = this;
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

    public static void Attack1(bool isRight)
    {
        instance.PlaySound(instance.audioAttack1);
        if (isRight)
            instance.direction = Vector2.one;
        else
            instance.direction = new Vector2(-1f,1f);
        instance.AttackCollision(atkOffset: instance.attack1offset*instance.direction, atkSize: instance.attack1size, 1);
    }
    public static void Attack2(bool isRight)
    {
        instance.PlaySound(instance.audioAttack2);
        //Debug.Log("공격2");
        if (isRight)
            instance.direction = Vector2.one;
        else
            instance.direction = new Vector2(-1f, 1f);
        instance.AttackCollision(atkOffset: instance.attack2offset * instance.direction, atkSize: instance.attack2size, 2);
    }
    public static void Attack3(bool isRight)
    {
        instance.PlaySound(instance.audioAttack3);
        //Debug.Log("공격3");
        if (isRight)
            instance.direction = Vector2.one;
        else
            instance.direction = new Vector2(-1f, 1f);
        instance.AttackCollision(atkOffset: instance.attack3offset * instance.direction, atkSize: instance.attack3size, 3);
    }


    private void AttackCollision(Vector3 atkOffset, Vector2 atkSize, int attackNum)
    {
        Vector2 attackPoint = gameObject.transform.position + atkOffset;
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
        if (collision.CompareTag("DeathZone"))
        {
            StageManager.OnPlayerDamage(-1);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = Vector3.zero;
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
