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
        //float w = (float)Screen.width/1920f;
        //gameObject.transform.localScale = new Vector3(w * 17.9f, transform.localScale.y, transform.localScale.z);
        //renderer.material.SetTextureScale("_MainTex", new Vector2((w), 1f));
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
