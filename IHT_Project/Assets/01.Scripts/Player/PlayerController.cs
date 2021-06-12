using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	ACTIVE = 0,
	DEAD = 1,
	INVINCIBILITY = 2
}


public class PlayerController : MonoBehaviour {

	static public PlayerController instance;

	[SerializeField]
	private PlayerState state;

	Animator animator;
	public float aniSpeed = 1f;
	private bool isLookRight = true;
	[Header("이동 관련")]
	[SerializeField]
	private float defaltSpeed = 1f;
	private float speed = 1f;
	[SerializeField]
	private float jumpSpeed = 10f;
	[SerializeField]
	private float dashSpeed = 10f;

	private bool isGround = false;
	private bool cantAny = false;

	private bool isAttacking1 = false;
	private bool isAttack1 = false;
	private bool isAttack2 = false;
	private bool isAttack3 = false;

	private bool isDash = false;
	private bool isUsingDash = false;
	private bool iCanDash = true;

	private bool isJump = false;
	private bool isRun = false;

	private SpriteRenderer spriteRenderer = null;
	private Rigidbody2D rigid = null;

	[Header("땅체크")]
	public Transform groundChecker;
	public LayerMask whatIsGround;



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
		if (state == PlayerState.DEAD)
			return;
		if (isGround)
        {
            if (!cantAny && !iCanDash)
                iCanDash = true;
        }

		if (PlayerInputs.Instance.Keyjump)
		{
			isJump = true;
		}

		isRun = (PlayerInputs.Instance.KeyHorizontalRaw != 0);

        if (PlayerInputs.Instance.KeyDash&&iCanDash && !cantAny)
        {
			isDash = true;
			animator.SetTrigger("skill");
		}

        if (!cantAny)
        {
            if (PlayerInputs.Instance.KeyAttack1 && !isAttacking1)
            {
				isAttack1 = true;
				animator.speed = 2f;
				animator.SetTrigger("attack1");
			}
			else if(PlayerInputs.Instance.KeyAttack2)
            {
				isAttack2 = true;
				animator.SetTrigger("attack3");
			}
			else if (PlayerInputs.Instance.KeyAttack3)
			{
				isAttack3 = true;
				animator.SetTrigger ("attack2");
			}
		}

		animator.SetBool("run", isRun);
		animator.SetBool("isGround", isGround);
	}
    private void FixedUpdate()
	{
		if (state == PlayerState.DEAD)
			return;

		if (isUsingDash)
			rigid.velocity = new Vector2(rigid.velocity.x, 0f);
		CheckGround();
		Run();
		if (isJump)
			Jump();
		if(isDash)
			Dash();
		if (isAttack1)
			attack1();
		if (isAttack2)
			attack2();
		if (isAttack3)
			attack3();
	}


    private void Run() {
		if (!isRun||PlayerInputs.Instance.KeyHorizontalRaw == 0)
			return;

		if (isGround)
			speed = defaltSpeed;

        if (!cantAny)
		{
			rigid.velocity = new Vector2(PlayerInputs.Instance.KeyHorizontalRaw * speed, rigid.velocity.y);
			spriteRenderer.flipX = (PlayerInputs.Instance.KeyHorizontalRaw < 0);
		}
		isLookRight = !spriteRenderer.flipX;
	}

	private void CheckGround()
	{
		isGround = Physics2D.OverlapCircle(groundChecker.position, 0.2f, whatIsGround);
	}
	private void Jump() {
        if (isGround && !cantAny)
		{
			
			rigid.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
		}
		isJump = false;
	}

	private void attack1() {
		isAttack1 = false;
		isAttacking1 = true;
		Invoke("IDonAttack1", 0.4f);
		Player.Attack1(isLookRight);
	}

	private void attack3() {
		isAttack3 = false;
		cantAny = true;
		Invoke("IAttack3Enemy", 0.4f);
		Invoke("ICanAnyThing", 0.9f);
	}

	private void attack2() {
		isAttack2 = false;
		cantAny = true;
		DashMove(5f);
		rigid.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
		Invoke("IAttack2Enemy", 0.6f);
		Invoke("ICanAnyThing", 1.3f);
	}

	private void DashMove(float dashSpeed)
    {
		Vector2 direct;
		if (isLookRight)
			direct = Vector2.right;
		else
			direct = Vector2.left;

		if (isGround)
			rigid.AddForce(direct * dashSpeed * 1.8f, ForceMode2D.Impulse);
		else
			rigid.AddForce(direct * dashSpeed, ForceMode2D.Impulse);
	}

	public void IAmDead()
    {
		state = PlayerState.DEAD;
    }
	private void Dash()
	{
		isDash = false;
		cantAny = true;
		isUsingDash = true;
		iCanDash = false;

		DashMove(this.dashSpeed);
		Invoke("IDontDash", 0.6f);
	}

	private void IDonAttack1()
    {
		isAttacking1 = false;
		animator.speed = aniSpeed;
	}
	private void IAttack2Enemy()
	{
		//Debug.Log("탕");
		Player.Attack2(isLookRight);
	}
	private void IAttack3Enemy()
	{
		Player.Attack3(isLookRight);
	}
	private void IDontDash()
	{
		cantAny = false;
		isUsingDash = false;
	}

	private void ICanAnyThing()
	{
		cantAny = false;
    }
}
