using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public Transform cam;
    public float scale = 0.1f;
    public float lerpSpeed = 4f;
    private Vector3 beforePos;

    void Start()
    {
        //cam = Camera.main.transform;
        beforePos = cam.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 delta = cam.position - beforePos;
        transform.position = Vector3.Lerp(transform.position, transform.position+(delta * scale), lerpSpeed * Time.deltaTime);
        //transform.position += delta * scale;
        beforePos = cam.position;

    }
}
