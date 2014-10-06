using UnityEngine;
using System.Collections;

public class JellyAI : MonoBehaviour {
	bool FacingRight = true;
	private enum State
	{
		Idle,
		Initialization,
		Search,
		Approach,
		PickAction,
		PickAttack,
		AttackLow,
		AttackHigh,
		AttackFront,
		AvoidLow,
		AvoidHigh,
		AvoidFront,
		PlayerOutOfReach,
		AtHome
	}
	[SerializeField] float		jumpForce = 0.075f;
	PolygonCollider2D[] myhitboxes;
	int playerturn;
	Transform _home;
	bool d1 = false;
	bool d2 = false;
	bool dodgepit = false;
	bool playeroutofreach;
	int avoidjumpheight = 25;
	float dist;
	bool grounded;
	int AwakeDistance = 20;
	public string targetStatus;
	RaycastHit2D walao;
	float halfdistanceheight;
	float halfdistance;
	int pitfalldistance = 10;
	int largepitfall = 15;
	int distance = 1;
	public LayerMask mask;
	private const float AADistance = 3f;
	private State _state;
	private Transform _myTransform;
	Transform target;
	float littleamount = 0.1f; //amount to get a little bit closer than pickaction activation range
    bool _alive = true;
	BoxCollider2D _myCollider;
	Vector3 v_diff;
	float atan2;
	Transform groundCheck;	
	float movspeed = 8f;
	[SerializeField] LayerMask whatIsGround;
	Animator anim;
	GameObject myfeet;
	public Rigidbody2D _myRigidBody;

	void Start () {
		_state = JellyAI.State.Initialization;
		StartCoroutine ("FSM");
	}

	private IEnumerator FSM(){
		while (_alive){
						switch (_state) {
						case State.Initialization:
								Init ();
								break;
						case State.Search:	
								Search ();
								break;
		               case State.Approach:
			                    Approach();
			                    break;
		            	case State.PickAction:
				               PickAction ();
			                	break;
			case State.AvoidLow:
				AvoidLow();
				break;
			case State.PlayerOutOfReach:
				BackHome();
				break;
			case State.AtHome:
				SweetHome();
				break;
			case State.PickAttack:
				bool notonair = target.gameObject.GetComponent<PlatformerCharacter2D>().grounded;
				if (!notonair)
					_state = JellyAI.State.AttackHigh;
				int _pickattack = Random.Range (0,3);
				if (_pickattack == 0)
					_state = JellyAI.State.AttackFront;
				if (_pickattack == 1 && notonair)
					_state = JellyAI.State.AttackLow;
				if (_pickattack==2)
					_state = JellyAI.State.AttackHigh;


				break;
			case State.AttackFront:
				cFrontattack();
				yield return new WaitForSeconds(0.3f);
				Frontattack();
				yield return new WaitForSeconds(0.2f);
				pFrontattack();
				_state =JellyAI.State.PickAction;
				break;
			case State.AttackLow:
				cLowattack();
				yield return new WaitForSeconds(0.3f);
				Lowattack();
				yield return new WaitForSeconds(0.2f);
				pLowattack();
				_state =JellyAI.State.PickAction;
				break;
			case State.AttackHigh:
				cHighattack();
				yield return new WaitForSeconds(0.3f);
				Highattack();
				yield return new WaitForSeconds(0.2f);
				pHighattack();
				_state =JellyAI.State.PickAction;

				break;
										}
						yield return null;
		}	
		}

	private void Init (){	
		myhitboxes = gameObject.GetComponentsInChildren<PolygonCollider2D>();
		Debug.Log (myhitboxes[1]);
		_home = transform.parent.transform;
		halfdistanceheight = collider2D.bounds.extents.y;
		halfdistance = collider2D.bounds.extents.x;
		_myTransform = transform;

		anim = GetComponent<Animator>();
		_myCollider = GetComponent<BoxCollider2D> ();
		_myRigidBody = GetComponent<Rigidbody2D>();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		_state = JellyAI.State.Approach;
		dist = Vector3.Distance (target.transform.position, _myTransform.position);
	
	}

	private void Search (){
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		_state = JellyAI.State.Approach;
	}
	private void Approach (){
		if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.1f, mask))
			grounded = true;
		else grounded = false;	
		

		if (!grounded)
			anim.SetBool("isGrounded", false);
		if (grounded)
			if (grounded)
				anim.SetBool("isGrounded", true);
	 dist = Vector3.Distance (target.transform.position, _myTransform.position);
		float distx = (target.transform.position.x - _myTransform.position.x);
		if (distx>0 && FacingRight != true){
			Flip();
			FacingRight = true;
		}
		if (distx<0 && FacingRight == true){
			Flip ();
			FacingRight = false;
		}

		if (distx > AADistance-littleamount && !playeroutofreach){		
			anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(movspeed*Time.deltaTime,0,0));}
		if (distx < -AADistance+littleamount && !playeroutofreach){
			anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(-movspeed*Time.deltaTime,0,0));}


		//AVOID STOPPING THE MOVEMENT IF THE PLAYER IS JUST AFTER A PIT
		if (distx > 0 && !grounded && distx < AADistance-littleamount && dodgepit)
			_myTransform.Translate (new Vector3(movspeed*Time.deltaTime,0,0));
		if (distx<0 && !grounded && distx > -AADistance+littleamount && dodgepit)
			_myTransform.Translate (new Vector3(-movspeed*Time.deltaTime,0,0));


		Vector2 rightpit = new Vector2 (1,-2);
		Vector2 leftpit = new Vector2 (-1,-2);
		Vector2 rlargepit = new Vector2 (1,-1);
		Vector2 llargepit = new Vector2 (-2, -2);
	if (distx<AADistance && grounded)	
			if (target != _home)
			_state = JellyAI.State.PickAction;
		else {

			_state = JellyAI.State.AtHome;
			anim.SetFloat("Speed",0);
		}
		//AVOID OBSTACLE
		if (Physics2D.Raycast(transform.position,Vector3.right, halfdistance + distance, mask) && FacingRight){
			Jump ();

		}
		if (Physics2D.Raycast(transform.position, Vector3.left, halfdistance + distance, mask) && !FacingRight){
			Jump ();

		}
		//AVOID PITFALL
		if (!Physics2D.Raycast(transform.position, rightpit, halfdistance + pitfalldistance, mask) && FacingRight ){
			Jump ();
			dodgepit = true;

		}
		if (!Physics2D.Raycast(transform.position, leftpit, halfdistance + pitfalldistance, mask) && !FacingRight){
			Jump ();
			dodgepit = true;
		}
		//STOP IF PITS2BIG
		if (!Physics2D.Raycast(transform.position, rlargepit, halfdistance + largepitfall, mask) && FacingRight){
			Debug.Log ("WATDAPUK");
			_state = JellyAI.State.PlayerOutOfReach;
		}
		if (!Physics2D.Raycast(transform.position, llargepit, halfdistance + largepitfall, mask) && !FacingRight){
			Debug.Log ("WATDAPUK");
			_state = JellyAI.State.PlayerOutOfReach;
		}

	}
	private void PickAction(){
		//ALL THE STUFF FOR POSITIONING,ETC NOT ACTUAL ACTION PICKING
			if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.1f, mask))
			grounded = true;
		else grounded = false;
		if (!grounded)
			anim.SetBool("isGrounded", false);
		if (grounded)
			if (grounded)
				anim.SetBool("isGrounded", true);
		float distx = (target.transform.position.x - _myTransform.position.x);
		if (distx>0 && FacingRight != true){
			Flip();
			FacingRight = true;
		}
		if (distx<0 && FacingRight == true){
			Flip ();
			FacingRight = false;
		}
		anim.SetFloat("Speed",0);
		dodgepit = false;


		//CHECK THE DIRECTION OF THE PLAYER AND THEN DECIDE IF YOU WANT TO DODGE A LOWHIT
		targetStatus = target.gameObject.GetComponent<PlayerAttackColliders>().Status4Mob;
		playerturn = target.gameObject.GetComponent<PlatformerCharacter2D>().turnright;
		if (targetStatus == "LowSlash" && playerturn == 1 && distx < 0)
			_state = JellyAI.State.AvoidLow;
		if (targetStatus == "LowSlash" && playerturn == -1 && distx > 0)
			_state = JellyAI.State.AvoidLow;

		//DECIDE TO ATTACK
		if (targetStatus == "Deciding")
			_state = JellyAI.State.PickAttack;

		 //distx = (target.transform.position.x - _myTransform.position.x);

		if (distx>AADistance || distx < -AADistance)
			_state = JellyAI.State.Approach;
	}
	void Flip (){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void Jump () {
		if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.1f, mask))
			grounded = true;
		else grounded = false;

		if (grounded){
			 // Debug.Log(rigidbody2D.velocity.y);
			Vector3 velocity = new Vector3 (0,avoidjumpheight,0);
			float walala = rigidbody2D.velocity.y ;
			_myRigidBody.velocity = velocity;
			//rigidbody2D.AddForce(new Vector2(0f, jumpForce+walala*-1));
			grounded = false;}
			if (!grounded)
				anim.SetBool("isGrounded", false);
			if (grounded)
					anim.SetBool("isGrounded", true);
		}
	void BackHome (){
		target = _home;
		_state = JellyAI.State.Approach;
	}
	void SweetHome (){
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		float distx = (target.transform.position.x - _myTransform.position.x);
		if (distx < AwakeDistance && distx > -AwakeDistance)
			_state = JellyAI.State.Approach;

	}
	void AvoidLow(){

		Jump();
		targetStatus = target.gameObject.GetComponent<PlayerAttackColliders>().Status4Mob;
		if (targetStatus != "low")
			_state = JellyAI.State.PickAction;
	}
	void cFrontattack(){
		//put all the animation and sound here but alèssiò didnt do shit
	}
	void Frontattack (){
		myhitboxes[1].enabled = true;
	}
	void pFrontattack(){
		myhitboxes[1].enabled = false;
	}
	void cLowattack(){
		//put all the animation and sound here but alèssiò didnt do shit
	}
	void Lowattack (){
		myhitboxes[0].enabled = true;
	}
	void pLowattack(){
		myhitboxes[0].enabled = false;
	}
	void cHighattack(){
		//put all the animation and sound here but alèssiò didnt do shit
	}
	void Highattack (){
		myhitboxes[2].enabled = true;
	}
	void pHighattack(){
		myhitboxes[2].enabled = false;
	}

	void Update () {
	
	}}