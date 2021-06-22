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

    //private bool isNight = false;


    [SerializeField]
    private int[] hateEnemies;

    [SerializeField]
    private Image[] iHps = null; 

    [SerializeField]
    private Sprite[] sHp = null;

    [SerializeField]
    private Image coreHPUI = null;

    private WaitForSeconds spawnCool = new WaitForSeconds(5f);

    public int hp;
    private float maxImageWidth;
    public int coreHp;
    public int clearHp;

    [SerializeField]
    private GameObject deadPanel = null;
    [SerializeField]
    private GameObject clearPanel = null;

    public void RestartStage()
    {
        GameManager.RestartStage();
    }
    public void QuitMenu()
    {
        GameManager.QuitMenu();
    }

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
                instance.deadPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else if(enemyId == -1)
        {
            instance.hp--;
            for (int i = 2; i >= instance.hp; i--)
            {
                instance.iHps[i].sprite = instance.sHp[1];
            }
            if (instance.hp <= 0)
            {
                PlayerController.instance.IAmDead();
                instance.deadPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            if (instance.hp >= 3)
                return;
            instance.hp++;
            //Debug.Log(instance.hp);
            for(int i =0; i<instance.hp; i++)
            {
                //Debug.Log(i);
                instance.iHps[i].sprite = instance.sHp[0];
            }
        }
    }
    public static void OnCoreDamage(int enemyId)
    {
        bool hateIt = false;
        for (int i = 0; i < instance.hateEnemies.Length; i++)
        {
            if (enemyId == instance.hateEnemies[i])
                hateIt = true;
        }
        if (hateIt)
        {
            instance.coreHp -= GameManager.GetDamage(enemyId);
            if (instance.coreHp <= 0)
            {
                PlayerController.instance.IAmDead();
                instance.deadPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            instance.coreHp += GameManager.GetDamage(enemyId);
            if (instance.coreHp >= instance.clearHp)
            {
                instance.clearPanel.SetActive(true);
                Time.timeScale = 0;
            }

        }
        instance.SetCoreHPUI();
    }

    private void SetCoreHPUI()
    {
        coreHPUI.rectTransform.sizeDelta = new Vector2(Mathf.Clamp( (float)coreHp /(float)clearHp, 0, 1 ) * maxImageWidth, coreHPUI.rectTransform.rect.height);
    }

    void Start()
    {
        hp = 3;
        maxImageWidth = coreHPUI.rectTransform.rect.width;
        SetCoreHPUI();
        StartCoroutine(ChangeNight());
        StartCoroutine(SpawnRightEnemy(1));
        StartCoroutine(SpawnLeftEnemy(0));
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
        //isNight = true;
    }

    private IEnumerator SpawnRightEnemy(int id)
    {
        id++;
        Enemy e = EnemyPooling.GetObject(id%3);
        e.transform.position = rightSpawnPoint.position;
        e.SetDirection();

        yield return spawnCool;
        StartCoroutine(SpawnRightEnemy(id));
    }
    private IEnumerator SpawnLeftEnemy(int id)
    {
        id++;
        Enemy e = EnemyPooling.GetObject(id%3);
        e.transform.position = leftSpawnPoint.position;
        e.SetDirection(false);

        yield return spawnCool;
        StartCoroutine(SpawnLeftEnemy(id));
    }
}