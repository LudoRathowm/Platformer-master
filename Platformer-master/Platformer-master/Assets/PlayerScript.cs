using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public enum State {
		Normal,
		Hit,
		Thrown,
		Stunned
	}
	bool facingRight = true;
	float halfdistanceheight;
	public string setstate;
	public bool hitfromleft;
	Animator anim;	
	public bool grounded;
	[SerializeField] LayerMask whatIsGround;	
	float move;
	public int turnright;
	float maxSpeed = 10;
	float jumpForce = 750f;
	float hitforce = 333f;
	float ThrownForce = 1000f;
	float littlejump = 20f;
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
				if (hitfromleft)
					mybody.AddForce(new Vector2(hitforce, hitforce));
			else if (!hitfromleft)
					mybody.AddForce(new Vector2(-hitforce, hitforce));
				yield return new WaitForSeconds(0.2f);
				setstate = "Normal";
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
			}
			yield return null;
		}
	}


	void FixedUpdate(){
		if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.52f, whatIsGround)) //a bit more than the player collider eight
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
		if (grounded)
			move = Input.GetAxis("Horizontal");
		
		if (Input.GetButtonDown("Jump")) 
			jump = true;
		
		if (move != 0 || jump || !nocontrol)
			Move ();
		if(move > 0 && !facingRight)
			Flip();
		else if(move < 0 && facingRight)
			Flip();
	}

	void Move () {
		anim.SetFloat("Speed", Mathf.Abs(move));
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		if (jump) {
			jump = false;
			if (grounded)
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}}
		void Flip ()
		{
			facingRight = !facingRight;
			Vector3 muhscale = transform.localScale;
			muhscale.x *= -1;
			transform.localScale = muhscale;
		}
	}

