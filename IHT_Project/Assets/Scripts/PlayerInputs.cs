using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance;

    public bool Keyjump = false;
    public float KeyHorizontalRaw = 0f;
    public bool KetDash = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        Keyjump = Input.GetButtonDown("Jump");
        KeyHorizontalRaw = Input.GetAxisRaw("Horizontal");
        KetDash = Input.GetButtonDown("Fire3");
    }
}
