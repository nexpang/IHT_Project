using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int stageIdx;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NextStage()
    {
        stageIdx++;
    }

    private void OnDamage()
    {

    }

    void Update()
    {
        
    }
    void Start()
    {
        
    }
}
