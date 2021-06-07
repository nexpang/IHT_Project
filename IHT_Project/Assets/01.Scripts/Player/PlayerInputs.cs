using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance;

    public bool Keyjump = false;
    public float KeyHorizontalRaw = 0f;
    public bool KeyDash = false;
    public bool KeyAttack1 = false;
    public bool KeyAttack2 = false;
    public bool KeyAttack3 = false;

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
        KeyDash = Input.GetKeyDown(KeyCode.LeftShift);
        KeyAttack1 = Input.GetMouseButtonDown(0);
        KeyAttack2 = Input.GetMouseButtonDown(1);
        KeyAttack3 = Input.GetMouseButtonDown(2);
    }
}
