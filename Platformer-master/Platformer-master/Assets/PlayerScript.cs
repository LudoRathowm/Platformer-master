using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public enum State {
		Normal,
		Hit,
		Thrown,
		Stunned,
		RollR,
		RollL,
		Combo,
		DoubleCombo,
		SlipL,
		SlipR

	}
public 	float sliptime = 0.3f;
	bool facingRight = true;
	public bool muhflip; //some skills may force you to flip m8
	float halfdistanceheight;
	public string setstate;
	public bool hitfromleft;
	Animator anim;	
	float slipsforce = 9;
	public bool grounded;
	[SerializeField] LayerMask whatIsGround;	
	float move;
	public int turnright;
	float maxSpeed = 10;
	float jumpForce = 750f;
	float hitforce = 150f;
	float ThrownForce = 1000f;
	float littlejump = 20f;
	float rollforce = 2600f;
	float Comboforce = 50;
	float DoubleComboforce = 100;
	public bool SlipR;
	public bool SlipL;
	bool jump;
	Rigidbody2D mybody;
	bool _alive = true;
	State _state;
	public bool nocontrol; //to prevent shitty bug where the move function controls the char and prevents knocks
	void Awake()
	{  mybody = rigidbody2D;
		anim = GetComponent<Animator>();
		Debug.Log (collider2D);
		halfdistanceheight = gameObject.collider2D.bounds.extents.y;	
		_state = PlayerScript.State.Normal;
		StartCoroutine ("FSM");
	}

	private IEnumerator FSM(){
		while (_alive){
			switch (_state) {
			case State.Normal:
				CheckForImput();
				break;
			case State.Hit:
				if (grounded)
					mybody.velocity = (new Vector2(0,0));
				if (hitfromleft)
					mybody.AddForce(new Vector2(hitforce, hitforce));
			else if (!hitfromleft)
					mybody.AddForce(new Vector2(-hitforce, hitforce));
				yield return new WaitForSeconds(0.02f);
				setstate = "Normal";
				GetComponent<PlayerAttackColliders>().lastactionisbasicattack = false;
				GetComponent<PlayerAttackColliders>().lastactioniscomboattack = false;
				_state = PlayerScript.State.Normal;
				break;
			case State.Thrown:
				if (hitfromleft)
					mybody.AddForce(new Vector2(ThrownForce,ThrownForce));
				else if (!hitfromleft)
					mybody.AddForce(new Vector2(-ThrownForce,ThrownForce));
				yield return new WaitForSeconds (1f);
				setstate = "Normal";
				_state = PlayerScript.State.Normal;
				break;
			case State.RollL:
				mybody.AddForce(new Vector2 (-rollforce,0));
				yield return new WaitForSeconds (0.1f);
				_state = State.Normal;
				break;
			case State.RollR:
				mybody.AddForce(new Vector2 (rollforce,0));
				yield return new WaitForSeconds (0.1f);
				_state = State.Normal;
				break;
			case State.Combo:
				if (facingRight)
					mybody.AddForce(new Vector2(Comboforce,Comboforce));
				if (!facingRight)
					mybody.AddForce(new Vector2(-Comboforce,Comboforce));
				yield return new WaitForSeconds (0.2f);
				_state = State.Normal;
				break;
			case State.DoubleCombo:
				if (facingRight)				
					mybody.AddForce(new Vector2(Comboforce,DoubleComboforce));
				if (!facingRight)
					mybody.AddForce(new Vector2(-Comboforce,DoubleComboforce));
				yield return new WaitForSeconds (0.5f);
				_state = State.Normal;
				break;
			case State.SlipL:
				// muh anim
				nocontrol = true;
				if (sliptime > 0){
					sliptime-=Time.deltaTime;
					mybody.transform.Translate(new Vector3 (-slipsforce*Time.deltaTime,0,0));
				}
				if (sliptime < 0)
					sliptime = 0;
				if (sliptime == 0){
					//muh anim
					nocontrol = false;
					SlipL = false;
					_state = State.Normal;
				}
				break;
			case State.SlipR:
				// muh anim
				nocontrol = true;
				if (sliptime > 0){
					sliptime-=Time.deltaTime;
					mybody.transform.Translate(new Vector3 (slipsforce*Time.deltaTime,0,0));
				}
				if (sliptime < 0)
					sliptime = 0;
				if (sliptime == 0){
					//muh anim
					SlipR = false;
					nocontrol = false;
					_state = State.Normal;
				}
				break;


			

			}
			yield return null;
		}
	}


	void FixedUpdate(){
		if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.62f, whatIsGround)) //a bit more than the player collider eight
			grounded = true;
		else grounded = false; 
		Debug.DrawRay (transform.position, Vector3.down*(halfdistanceheight+0.52f));

		if (facingRight == true)
			turnright = 1;
		if (facingRight != true)
			turnright = -1;
		if (grounded)
			anim.SetBool("grounded",true);
		if (!grounded)
			anim.SetBool("grounded",false);

		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		if (setstate == "Hit")
			_state = PlayerScript.State.Hit;
		if (setstate == "Thrown")
			_state = PlayerScript.State.Thrown;

	
	}

	void Start () {
	
	}
	void CheckForImput(){		
		if (SlipL)
			_state = State.SlipL;
		if (SlipR)
			_state = State.SlipR;
		string input = GetComponent<PlayerAttackColliders>().Status4Mob;
		bool imattacking = false;
		if (input != "Deciding")
			imattacking = true;
		else if (input == "Deciding")
			imattacking = false;
		if (input == "RollL")
			_state = State.RollL;
		if (input == "RollR")
			_state = State.RollR;
		if (input == "ComboAttack")
			_state = State.Combo;
		if (input == "DoubleComboAttack")
			_state = State.DoubleCombo;
		if (grounded)
			move = Input.GetAxis("Horizontal");

		if (Input.GetButtonDown("Jump")) 
			jump = true;
		if (move != 0 || jump || !nocontrol )
			if (!imattacking)
			Move ();
		if(move > 0 && !facingRight && !imattacking)
			Flip();
		else if(move < 0 && facingRight && !imattacking)
			Flip();
		if (muhflip){
			muhflip = false;
			Flip();

		}

	}

	void Move () {
		anim.SetFloat("Speed", Mathf.Abs(move));
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		if (jump) {
			jump = false;
			if (grounded)
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}

	}
		void Flip ()
		{
			facingRight = !facingRight;
			Vector3 muhscale = transform.localScale;
			muhscale.x *= -1;
			transform.localScale = muhscale;
		}

	}

