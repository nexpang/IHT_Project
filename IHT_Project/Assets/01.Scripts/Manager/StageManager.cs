using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    [SerializeField]
    private Transform leftSpawnPoint;
    [SerializeField]
    private Transform rightSpawnPoint;

    [SerializeField]
    private float chaginNightTime;

    public SpriteRenderer[] backSky;
    public SpriteRenderer[] backGround;

    public Sprite[] spBackGround;

    private bool isNight = false;


    [SerializeField]
    private int[] hateEnemies;

    [SerializeField]
    private Image[] iHps = null; 

    [SerializeField]
    private Sprite[] sHp = null;

    private WaitForSeconds spawnCool = new WaitForSeconds(5f);

    public int hp;

    private void Awake()
    {
        instance = this;
    }
    public static void OnPlayerDamage(int enemyId)
    {
        bool hateIt = false;
        for(int i =0; i<instance.hateEnemies.Length; i++)
        {
            if (enemyId == instance.hateEnemies[i])
                hateIt = true;
        }
        if (hateIt)
        {
            instance.hp--;
            for (int i = 2; i >= instance.hp; i--)
            {
                instance.iHps[i].sprite = instance.sHp[1];
            }
            if (instance.hp <= 0)
            {
                PlayerController.instance.IAmDead();
            }
        }
        else
        {
            if (instance.hp >= 3)
                return;
            instance.hp++;
            Debug.Log(instance.hp);
            for(int i =0; i<instance.hp; i++)
            {
                Debug.Log(i);
                instance.iHps[i].sprite = instance.sHp[0];
            }
        }
    }
    void Start()
    {
        hp = 3;
        StartCoroutine(ChangeNight());
        StartCoroutine(SpawnRightEnemy());
        StartCoroutine(SpawnLeftEnemy());
    }

    void Update()
    {

    }
    IEnumerator ChangeNight()
    {
        yield return new WaitForSeconds(chaginNightTime);
        //화면 가리기
        for (int i = 0; i < backSky.Length; i++)
        {
            backSky[i].sprite = spBackGround[2];
        }
        for (int i = 0; i < backGround.Length; i++)
        {
            backGround[i].sprite = spBackGround[3];
        }
        isNight = true;
    }

    private IEnumerator SpawnRightEnemy()
    {
        Enemy e = EnemyPooling.GetObject();
        e.transform.position = rightSpawnPoint.position;
        e.SetDirection();

        yield return spawnCool;
        StartCoroutine(SpawnRightEnemy());
    }
    private IEnumerator SpawnLeftEnemy()
    {
        Enemy e = EnemyPooling.GetObject();
        e.transform.position = leftSpawnPoint.position;
        e.SetDirection(false);

        yield return spawnCool;
        StartCoroutine(SpawnLeftEnemy());
    }
}