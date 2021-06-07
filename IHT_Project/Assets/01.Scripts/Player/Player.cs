using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance = null;

    [SerializeField]
    private int hp;

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
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void OnDamaged(int damage =1)
    {
        instance.hp -= damage;
        Debug.Log(instance.hp);
        if (instance.hp <= 0)
        {
            //게임오버
            PlayerController.instance.IAmDead();
        }
    }

    public static void Attack1(bool isRight)
    {
        if (isRight)
            instance.direction = Vector2.one;
        else
            instance.direction = new Vector2(-1f,1f);
        instance.AttackCollision(atkOffset: instance.attack1offset*instance.direction, atkSize: instance.attack1size, 1);
    }
    public static void Attack2(bool isRight)
    {
        //Debug.Log("공격2");
        if (isRight)
            instance.direction = Vector2.one;
        else
            instance.direction = new Vector2(-1f, 1f);
        instance.AttackCollision(atkOffset: instance.attack2offset * instance.direction, atkSize: instance.attack2size, 2);
    }
    public static void Attack3(bool isRight)
    {
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
                    enemy.OnDamaged(attackDamage[attackNum - 1]);
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
