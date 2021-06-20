using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShark : Enemy
{
    [SerializeField]
    private float flyGround = 1; 

    public override void SetDirection(bool isRight = true)
    {
        base.SetDirection(isRight);
        transform.position += Vector3.up * flyGround;
    }

    void Update()
    {
        
    }
}
