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
		Guard,
		PlayerOutOfReach,
		AttackKnock,
		AtHome
	}
	public bool knockattack;
	bool repositioning;
	bool obstaclebehind; //so you dont try to get away and you have your back to a wall/pit and can't move/fall into it
	bool pitbehind;
	Vector2 rightpit = new Vector2 (1,-2);
	Vector2 leftpit = new Vector2 (-1,-2);
	Vector2 rlargepit = new Vector2 (1,-1);
	Vector2 llargepit = new Vector2 (-2, -2);
	[SerializeField] float jumpForce = 0.075f;
	bool needtorun;
	public bool isguarding;
	const int shortmelee = 2;
	const int longmelee = 10;
	public bool islowhealth = false;
	float distx;
	float disty;
	float myheight = 1.6f; //how big taller the mob is compared to the player to prevent bugs with youaretoohigh
	float myfat = 2.3f; //how big the hitbox of the mob is, to avoid getting stuck on platforms to move a bit extra
	bool toohighright;
	bool toohighleft;
	bool youaretoohigh = false;
	PolygonCollider2D[] myhitboxes;
	int playerturn;
	Transform _home;
	bool d1 = false;
	float highatkrange = 1.5f;
	bool d2 = false;
	bool dodgepit = false;
	bool playeroutofreach;
	int avoidjumpheight = 25;
	float dist;
	bool grounded;
	int AwakeDistance = 20;
	string targetStatus;
	RaycastHit2D walao;
	float halfdistanceheight;
	float halfdistance;
	int pitfalldistance = 10;
	int largepitfall = 15;
	int distance = 1;
	public LayerMask mask;
	private float AADistance = 3f;
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
				//zz else he just spams attacks and rapes you
				//      yield return new WaitForSeconds (0.2f);
				Pickattack();
				break;
			case State.AttackFront:
				anim.SetBool("midspit",true);
				yield return new WaitForSeconds(0.4f);               
				GetComponentInChildren<JellySpitter>().shoot = true;
				anim.SetBool("midspit",false);
				_state = JellyAI.State.PickAction;
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
			case State.Guard:
				isguarding = true;
				anim.SetBool("defending", true);
				//anim.blabla guarding animation i wish i had it
				yield return new WaitForSeconds (0.5f);
				anim.SetBool("defending", false);
				isguarding = false;
				_state = JellyAI.State.PickAction;
				break;                 
			case State.AttackKnock:
				cKnockattack();
				yield return new WaitForSeconds (0.2f);
				Knockattack ();
				yield return new WaitForSeconds (0.2f);
				pKnockattack ();
				_state =JellyAI.State.PickAction;
				break;
			}
			yield return null;
		}      
	}
	
	private void Init (){  
		myhitboxes = gameObject.GetComponentsInChildren<PolygonCollider2D>();
		
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
		disty =  (target.transform.position.y - _myTransform.position.y);
		distx = (target.transform.position.x - _myTransform.position.x);
		if (disty < 0  && distx > 0 || distx < 0)
			youaretoohigh = true;
		
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


		if (distx > 0 && distx<AADistance && grounded) 
		if (target != _home ){

			_state = JellyAI.State.PickAction;}
		else {
			
			_state = JellyAI.State.AtHome;
			anim.SetFloat("Speed",0);
		}
		if (distx <0 && distx> -AADistance && grounded) {
			if (target != _home ){

				_state = JellyAI.State.PickAction;}
			else {
				
				_state = JellyAI.State.AtHome;
				anim.SetFloat("Speed",0);
			}}
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
		if (!Physics2D.Raycast(transform.position, llargepit, halfdistance + largepitfall, mask) && !FacingRight) {
			Debug.Log ("WATDAPUK");
			_state = JellyAI.State.PlayerOutOfReach;}
		if (!Physics2D.Raycast(transform.position, rlargepit, halfdistance + largepitfall, mask) && FacingRight)
			_state = JellyAI.State.PlayerOutOfReach;

		
	}
	private void PickAction(){

		anim.SetFloat("Speed",0);
		targetStatus = target.gameObject.GetComponent<PlayerAttackColliders>().Status4Mob;
		//ALL THE STUFF FOR POSITIONING,ETC NOT ACTUAL ACTION PICKING
		if (Physics2D.Raycast(transform.position,Vector3.down, halfdistanceheight + 0.1f, mask))
			grounded = true;
		else grounded = false;
		if (!grounded)
			anim.SetBool("isGrounded", false);
		if (grounded)
			if (grounded)
				anim.SetBool("isGrounded", true);
		distx = (target.transform.position.x - _myTransform.position.x);
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
		
		
		
		disty =  (target.transform.position.y - _myTransform.position.y);
		
		if (disty < -1.6f  && distx > 0 || disty < -1.6f && distx < 0)
			youaretoohigh = true;
		
		//CHECK THE DIRECTION OF THE PLAYER AND THEN DECIDE IF YOU WANT TO DODGE A LOWHIT

		playerturn = target.gameObject.GetComponent<PlayerScript>().turnright;
		if (targetStatus == "LowSlash" && playerturn == 1 && distx < 0 && !youaretoohigh)
			_state = JellyAI.State.AvoidLow;
		if (targetStatus == "LowSlash" && playerturn == -1 && distx > 0 && !youaretoohigh)
			_state = JellyAI.State.AvoidLow;
		
		
		//IF LOW ON HP, CHANGE AA DISTANCE
		if (islowhealth && AADistance != longmelee)	 {	
			AADistance = longmelee;
			anim.SetBool("lowonhp",true);
		}
		else if (!islowhealth)  {    
			anim.SetBool("lowonhp",false);
			AADistance = shortmelee;}
		

					
		
		
		
		
		//WHEN TO GUARD
		if (targetStatus == "TopSlash" && playerturn == 1 && distx < 0 || targetStatus == "TopSlash" && playerturn == -1 && distx > 0 || targetStatus == "LowSlash" && playerturn == 1 && distx < 0 || targetStatus == "LowSlash" && playerturn == -1 && distx > 0)
			_state = JellyAI.State.Guard;
		//DECIDE TO ATTACK
		if (targetStatus == "Deciding" && !needtorun)
			_state = JellyAI.State.PickAttack;
		
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

			_myRigidBody.velocity = velocity;

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
		distx = (target.transform.position.x - _myTransform.position.x);
		if (distx < AwakeDistance && distx > -AwakeDistance)
			_state = JellyAI.State.Approach;
		
	}
	void AvoidLow(){
		
		Jump();
		targetStatus = target.gameObject.GetComponent<PlayerAttackColliders>().Status4Mob;
		if (targetStatus != "low")
			_state = JellyAI.State.PickAction;
	}
	void cLowattack(){
		anim.SetBool("lowkick",true);
		//put all the animation and sound here but alèssiò didnt do shit
	}
	void Lowattack (){
		myhitboxes[0].enabled = true;
	}
	void pLowattack(){
		anim.SetBool("lowkick",false);
		myhitboxes[0].enabled = false;
	}
	void cHighattack(){
		anim.SetBool("hightongue",true);
		//put all the animation and sound here but alèssiò didnt do shit
	}
	void Highattack (){
		myhitboxes[2].enabled = true;
	}
	void pHighattack(){
		anim.SetBool("hightongue",false);
		myhitboxes[2].enabled = false;
	}
	void cKnockattack (){
		anim.SetBool("knock",true);
		//animation and sound ur waifu a shit
	}
	void Knockattack (){
		myhitboxes[0].enabled = true;
		knockattack = true;
	}
	void pKnockattack (){
		myhitboxes[0].enabled = false;
		knockattack = false;
		anim.SetBool("knock",false);
	}
	void Pickattack (){
		disty =  (target.transform.position.y - _myTransform.position.y);
		distx = (target.transform.position.x - _myTransform.position.x);
		if (disty+myheight < 0 && distx > 0 || distx < -0)
			youaretoohigh = true;
		if (disty + myheight >=0)
			youaretoohigh = false;
		if (!youaretoohigh){
			toohighleft = false;
			toohighright = false;
			anim.SetFloat("Speed",0);
		}
		
		if (youaretoohigh && distx > 0 && !toohighleft)
			toohighright = true;
		if (youaretoohigh && distx < 0 && !toohighright)
			toohighleft = true;
		if (youaretoohigh && distx > -myfat && toohighright && !islowhealth){
			anim.SetFloat("Speed",1);
			_myTransform.Translate (new Vector3(movspeed*Time.deltaTime,0,0));}
		if (youaretoohigh && distx < myfat && toohighleft && !islowhealth){
			anim.SetFloat("Speed",1);
			_myTransform.Translate (new Vector3(-movspeed*Time.deltaTime,0,0));}
	
		//am i backed to a wall?
		if (Physics2D.Raycast(transform.position,Vector3.left, halfdistance + distance, mask) && FacingRight)
			obstaclebehind = true;	
		else if (Physics2D.Raycast(transform.position, Vector3.right, halfdistance + distance, mask) && !FacingRight)
			obstaclebehind = true;
		else obstaclebehind = false;


		// am i about to fall into a pit?
		if (!Physics2D.Raycast(transform.position, rightpit, halfdistance + pitfalldistance, mask) && !FacingRight || !Physics2D.Raycast(transform.position, leftpit, halfdistance + pitfalldistance, mask) && FacingRight)
			pitbehind = true;
		else pitbehind = false;
			

	

		//MEH REPOSITIONING
		distx = (target.transform.position.x - _myTransform.position.x);
		if (distx > 0 && distx < AADistance-littleamount && islowhealth && !obstaclebehind && !pitbehind){             repositioning = true;

			_myTransform.Translate (new Vector3(-3*movspeed*Time.deltaTime,0,0));
			
		
		}
		
		else if (distx < 0 && distx > -AADistance+littleamount && islowhealth && !obstaclebehind && !pitbehind){            
	
			_myTransform.Translate (new Vector3(+3*movspeed*Time.deltaTime,0,0));
			repositioning = true;
		} else repositioning = false;
		
		//      if (disty > highatkrange)
		//              Jump ();
		if (disty < highatkrange && disty > 0 && !youaretoohigh && AADistance != longmelee)
			_state = JellyAI.State.AttackHigh;
		int _pickattack = Random.Range (0,3);
		
		

		if (_pickattack == 1 && disty+myheight > 0 && !youaretoohigh && AADistance != longmelee)
			_state = JellyAI.State.AttackLow;
		if (_pickattack==2 && !youaretoohigh && AADistance != longmelee)
			_state = JellyAI.State.AttackHigh;
		if (islowhealth && pitbehind || obstaclebehind && islowhealth)
			if (distx >0 && distx < shortmelee)
			_state = JellyAI.State.AttackKnock;
		   else if (distx<0 && distx>-shortmelee)
			_state = JellyAI.State.AttackKnock;
		else _state = JellyAI.State.AttackFront;
		if (_pickattack == 0 && !youaretoohigh && !repositioning && islowhealth && !obstaclebehind && !pitbehind)
			_state = JellyAI.State.AttackFront;
		
		//      if (distx > AADistance-littleamount ||distx < -AADistance+littleamount)
		//              _state = JellyAI.State.PickAction;
		
		
	}
	/*void AdjustPosition(){
		needtorun = false;
		Debug.Log ("WAT DA PUG");
		if (distx>0 && FacingRight != true){
			Flip();
			FacingRight = true;
		}
		if (distx<0 && FacingRight == true){
			Flip ();
			FacingRight = false;
		}
		distx = (target.transform.position.x - _myTransform.position.x);
		if (distx > 0 && distx < AADistance-littleamount){             
			anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(-3*movspeed*Time.deltaTime,0,0));
			
			if (distx>0 && FacingRight != true){
				Flip();
				FacingRight = true;
			}
			
		}
		
		if (distx < 0 && distx > -AADistance+littleamount){            
			anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(+3*movspeed*Time.deltaTime,0,0));
			if (FacingRight){
				Flip ();
				FacingRight = false;
			}
			
		}
		if (-littleamount < distx - AADistance && distx - AADistance < littleamount || distx+AADistance<littleamount && distx+AADistance>-littleamount){
			anim.SetFloat("Speed",1f);
			//      _state = JellyAI.State.PickAction;
			if (!FacingRight){
				Flip ();
				FacingRight = true;
			}
		}
	}*/
	void Update () {
		
	}}

