using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {

            GameManager.OnBtnSolo();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
