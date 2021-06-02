using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBack : MonoBehaviour
{
    [SerializeField]
    private Transform camTrm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(camTrm.position.x, 0f, 0f);
    }
}
