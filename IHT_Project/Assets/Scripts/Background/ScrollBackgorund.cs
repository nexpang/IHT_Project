using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackgorund : MonoBehaviour
{
    private new Renderer renderer;
    [SerializeField]
    private Transform trmPlayer;

    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector2 offset;
    private float randOffsetX;
    private float offsetX;
    private float offsetY;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        randOffsetX = Random.Range(-30f, 30f);
    }


    void Update()
    {
        offsetX = (trmPlayer.position.x+randOffsetX) * speed * 0.001f;
        offsetY = 0f; //trmPlayer.position.y* speed *0.01f
        offset = new Vector2(offsetX, offsetY);
        renderer.material.SetTextureOffset("_MainTex", offset);
          
    }
}
