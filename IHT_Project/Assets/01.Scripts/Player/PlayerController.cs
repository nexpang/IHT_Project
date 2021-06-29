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

	public PlayerInputs inputs;
	public Player player;

	[SerializeField]
	private PlayerState state;

	public Animator animator;
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
	public bool isAttack1 = false;
	public bool isAttack2 = false;
	public bool isAttack3 = false;

	public bool isDash = false;
	private bool isUsingDash = false;
	private bool iCanDash = true;

	public bool isJump = false;
	private bool isRun = false;

	private SpriteRenderer spriteRenderer = null;
	private Rigidbody2D rigid = null;

	[Header("땅체크")]
	public Transform groundChecker;
	public LayerMask whatIsGround;

	[SerializeField]
	public AudioClip audioDash;
	private AudioSource audioSource;

	public bool isSingle = true;


	private void Awake()
    {
		instance = this;
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		
		//inputs = GetComponent<PlayerInputs>();
		//player = GetComponent<Player>();
	}

    void Start () {
		animator.speed = aniSpeed;
		audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		if (state == PlayerState.DEAD)
			return;
		if (isGround)
        {
            if (!cantAny && !iCanDash)
                iCanDash = true;
        }

		if (inputs.Keyjump)
		{
			isJump = true;

            if (!isSingle)
			{
				DataVO dataVO = new DataVO();
				dataVO.type = "JUMP";
				SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
			}
		}

		isRun = (inputs.KeyHorizontalRaw != 0);

        if (inputs.KeyDash&&iCanDash && !cantAny)
        {
			isDash = true;
			animator.SetTrigger("skill");

			if (!isSingle)
			{
				DataVO dataVO = new DataVO();
				dataVO.type = "DASH";
				SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
			}
		}

        if (!cantAny)
        {
            if (inputs.KeyAttack1 && !isAttacking1)
            {
				isAttack1 = true;
				animator.speed = 2f;
				animator.SetTrigger("attack1");

				if (!isSingle)
				{
					DataVO dataVO = new DataVO();
					dataVO.type = "ATTACK1";
					SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
				}
			}
			else if(inputs.KeyAttack2)
            {
				isAttack2 = true;
				animator.SetTrigger("attack3");

				if (!isSingle)
				{
					DataVO dataVO = new DataVO();
					dataVO.type = "ATTACK2";
					SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
				}
			}
			else if (inputs.KeyAttack3)
			{
				isAttack3 = true;
				animator.SetTrigger ("attack2");

				if (!isSingle)
				{
					DataVO dataVO = new DataVO();
					dataVO.type = "ATTACK3";
					SocketClient.SendDataToSocket(JsonUtility.ToJson(dataVO));
				}
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
		if (!isRun|| inputs.KeyHorizontalRaw == 0)
			return;

		if (isGround)
			speed = defaltSpeed;

        if (!cantAny)
		{
			rigid.velocity = new Vector2(inputs.KeyHorizontalRaw * speed, rigid.velocity.y);
			spriteRenderer.flipX = (inputs.KeyHorizontalRaw < 0);
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
		player.Attack1(isLookRight);
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

		audioSource.clip = audioDash;
		audioSource.Play();

		rigid.AddForce(direct * dashSpeed, ForceMode2D.Impulse);
		
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
		//GetComponent<CapsuleCollider2D>().enabled = false;
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
		player.Attack2(isLookRight);
	}
	private void IAttack3Enemy()
	{
		player.Attack3(isLookRight);
	}
	private void IDontDash()
	{
		//GetComponent<CapsuleCollider2D>().enabled = true;
		cantAny = false;
		isUsingDash = false;
	}

	private void ICanAnyThing()
	{
		cantAny = false;
    }
}
