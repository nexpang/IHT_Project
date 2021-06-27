using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int stageIdx;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if(obj.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void RestartStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void QuitMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void NextStage()
    {
        Time.timeScale = 1;
        stageIdx++;
    }

    public static int GetDamage(int enemyId)
    {
        switch (enemyId)
        {
            case 0:
                return 2;
            case 1:
                return 2;
            case 2:
                return 3;
            default:
                return 0;
        }
    }

    void Update()
    {

    }
    void Start()
    {

    }

    public static void OnBtnSolo()
    {
        SceneManager.LoadScene(1);
    }

    public static void OnBtnMulti()
    {
        SceneManager.LoadScene(2);
    }
}
