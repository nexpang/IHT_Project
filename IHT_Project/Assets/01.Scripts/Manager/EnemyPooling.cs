using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public static EnemyPooling instance;

    [SerializeField]
    private GameObject enemyPrefab;

    Queue<Enemy> poolingEnemyQueue = new Queue<Enemy>();

    private void Awake()
    {
        instance = this;

        Init(10);
    }

    private void Init(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingEnemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    private Enemy CreateNewEnemy()
    {
        Enemy newObj = Instantiate(enemyPrefab).GetComponent<Enemy>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Enemy GetObject() { 
        if (instance.poolingEnemyQueue.Count > 0) 
        { 
            Enemy obj = instance.poolingEnemyQueue.Dequeue(); 
            obj.transform.SetParent(null); obj.gameObject.SetActive(true); 
            return obj;
        } 
        else 
        { 
            Enemy newObj = instance.CreateNewEnemy(); 
            newObj.gameObject.SetActive(true); 
            newObj.transform.SetParent(null); 
            return newObj; 
        } 
    }

    public static void ReturnObject(Enemy obj) { 
        obj.gameObject.SetActive(false); 
        obj.transform.SetParent(instance.transform); 
        instance.poolingEnemyQueue.Enqueue(obj); 
    }
}
