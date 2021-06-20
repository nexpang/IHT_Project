using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public static EnemyPooling instance;

    [SerializeField]
    private GameObject[] enemyPrefabs;

    //Queue<Enemy> poolingEnemyQueue = new Queue<Enemy>();
    Queue<Enemy>[] poolingEnemyQueues = {new Queue<Enemy>(), new Queue<Enemy>() , new Queue<Enemy>() };

    private void Awake()
    {
        instance = this;

        Init(10, 1);
    }

    private void Init(int initCount, int id)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingEnemyQueues[id].Enqueue(CreateNewEnemy(id));
        }
    }

    private Enemy CreateNewEnemy(int id)
    {
        Enemy newObj = Instantiate(enemyPrefabs[id]).GetComponent<Enemy>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Enemy GetObject(int id) { 
        if (instance.poolingEnemyQueues[id].Count > 0) 
        { 
            Enemy obj = instance.poolingEnemyQueues[id].Dequeue(); 
            obj.transform.SetParent(null); obj.gameObject.SetActive(true); 
            return obj;
        } 
        else 
        { 
            Enemy newObj = instance.CreateNewEnemy(id); 
            newObj.gameObject.SetActive(true); 
            newObj.transform.SetParent(null); 
            return newObj; 
        } 
    }

    public static void ReturnObject(Enemy obj, int id) { 
        obj.gameObject.SetActive(false); 
        obj.transform.SetParent(instance.transform); 
        instance.poolingEnemyQueues[id].Enqueue(obj); 
    }
}
