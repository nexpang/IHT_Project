using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	static public PlayerController instance;
	Animator animator;
	public float aniSpeed = 1f;
	public bool isLookRight = true;
	[SerializeField, Range(1f,10f)]
	private float defaltSpeed = 1f;
	private float speed = 1f;
	[SerializeField]
	private float jumpSpeed = 10f;
	[SerializeField]
	private float dashSpeed = 10f;
	private bool isGround = false;
	private bool isUsingDash = false;
	private bool iCanDash = true;
	private SpriteRenderer spriteRenderer = null;
	private Rigidbody2D rigid = null;

    private void Awake()
    {
		instance = this;
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
	}

    void Start () {
		animator.speed = aniSpeed;
	}

	void Update () {
		Move();
		Dash();
	}

    private void Move()
    {
		Run();
        if (PlayerInputs.Instance.Keyjump)
        {
			Jump();
        }
    }

    private void Run() {
		if (PlayerInputs.Instance.KeyHorizontalRaw != 0)
		{
			animator.SetBool("run", true);
        }
        else
        {
			animator.SetBool("run", false);
			return;
		}

		if (isGround)
			speed = defaltSpeed;
		else
			speed = defaltSpeed / 5f * 3f;

        if (!isUsingDash)
		{
			rigid.velocity = new Vector2(PlayerInputs.Instance.KeyHorizontalRaw * speed, rigid.velocity.y);
			spriteRenderer.flipX = (PlayerInputs.Instance.KeyHorizontalRaw < 0);
		}
		isLookRight = !spriteRenderer.flipX;
	}

	private void Jump() {
        if (isGround && !isUsingDash)
		{
			rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
			animator.SetBool("jump", true);
        }
	}

	private void attack1() {
		animator.SetTrigger ("attack1");
	}

	private void attack2() {
		animator.SetTrigger ("attack2");
	}

	private void attack3() {
		animator.SetTrigger ("attack3");
	}

	private void Dash()
	{
		if (isUsingDash)
			rigid.velocity = new Vector2(rigid.velocity.x, 0f);
		if (!iCanDash || !PlayerInputs.Instance.KetDash)
			return;
		isUsingDash = true;
		iCanDash = false;
		Vector2 direct;
		if (isLookRight)
			direct = Vector2.right;
		else
			direct = Vector2.left;
		if(isGround)
			rigid.AddForce(direct * dashSpeed * 1.8f, ForceMode2D.Impulse);
		else
			rigid.AddForce(direct * dashSpeed, ForceMode2D.Impulse);
		animator.SetTrigger ("skill");
		Invoke("IAmNotDashing", 0.6f);
	}

	private void IAmNotDashing()
    {
		isUsingDash = false;
		if(isGround)
			iCanDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
			isGround = true;
			animator.SetBool("jump", false);
			if (!isUsingDash && !iCanDash)
				iCanDash = true;
		}
	}
    private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Ground")
		{
			isGround = false;
		}
	}
}
