using UnityEngine;
using System.Collections;

public class BirdAI : MonoBehaviour {
	//i didn't even start yet and i already want to kill myself
	bool _alive = true;
	float flyspeedy;
	bool toohigh;
	bool toolow;
	Transform _myTransform;
	Transform target;
	bool rightheight;
	float flyheightmin = 8;
	float flyheightmax = 8.1f;
	float flyheightworld; //za warudo
	[SerializeField] LayerMask whatIsGround;	
	float distx;
	float bombingdistance = 10; //start bombing 
	float movspeed = 15;
	float bombmovspeed = 30;
	float bombextradistance = 20; //stop bombing distance
	float poopdelay = 0.3f;
	Animator anim;
	public bool FacingRight;
	private enum State
	{
		Init,
		Approach,
		Bombing,
		BombR,
		BombL

	}
	State _state;

	// Use this for initialization
	void Start () {
		_state = State.Init;
		StartCoroutine ("FSM");
		flyheightworld = _myTransform.position.y;
		FacingRight = true;
	}

	IEnumerator FSM(){
		while (_alive){
			switch (_state){
			case State.Init:
				_myTransform = transform;
		target = GameObject.FindGameObjectWithTag("Player").transform;
				_state = State.Approach;
				anim = GetComponent<Animator>();
				break;
			case State.Approach:
				Approach();
				break;
			case State.Bombing:
				Osama();
				break;			
			case State.BombL:
				BombL();
				break;
			case State.BombR:
				BombR();
				break;
			}
			yield return null;
		}
	}


	void Approach (){
		anim.SetBool("slow",true);
		distx = (target.position.x-_myTransform.position.x);
		if (distx > bombingdistance){             
			//	anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(movspeed*Time.deltaTime,0,0));}
		if (distx < -bombingdistance){

			//	anim.SetFloat("Speed",1f);
			_myTransform.Translate (new Vector3(-movspeed*Time.deltaTime,0,0));}
		else if (distx < bombingdistance && distx > -bombingdistance)
			_state = State.Bombing;
		if (distx>0 && FacingRight != true){
			Flip();
			FacingRight = true;
		}
		if (distx<0 && FacingRight == true){
			Flip ();
			FacingRight = false;
		}
		
		KeepFlying();

	}

	void Osama (){
		// fly up to da playa and do shiet
		distx = (target.position.x-_myTransform.position.x);
		if (distx > 0)
			_state = State.BombR;
		if (distx < 0)
			_state = State.BombL;
	}
	void BombL (){
		anim.SetBool("slow",false);
		KeepFlying();
		distx = (target.position.x-_myTransform.position.x);
		if (distx-bombextradistance < -bombingdistance)
			_myTransform.Translate (new Vector3(-bombmovspeed*Time.deltaTime,0,0));
		else _state = State.Approach;
		if (poopdelay >0)
			poopdelay -=Time.deltaTime;
		if (poopdelay<0)
			poopdelay = 0;
		if (poopdelay == 0){
			GetComponentInChildren<BirdAnus>().poop = true;
			poopdelay = 0.3f;
		}
	}
	void BombR(){
		anim.SetBool("slow",false);
		KeepFlying();
		distx = (target.position.x-_myTransform.position.x);
		if (distx+bombextradistance>bombingdistance)
			_myTransform.Translate (new Vector3(bombmovspeed*Time.deltaTime,0,0));
		else _state = State.Approach;
		if (poopdelay >0)
			poopdelay -=Time.deltaTime;
		if (poopdelay<0)
			poopdelay = 0;
		if (poopdelay == 0){
			GetComponentInChildren<BirdAnus>().poop = true;
			poopdelay = 0.3f;
		}
	}
	void KeepFlying(){
		//bunch of stuff to keep flying at same height;
		
		if (Physics2D.Raycast(transform.position,Vector3.down, flyheightmax, whatIsGround)) //xaxaxa
			if (!Physics2D.Raycast(transform.position,Vector3.down, flyheightmin, whatIsGround)) //xaxaxa
		{rightheight = true;
			toolow = false;
			toohigh = false;}
		
		if (!Physics2D.Raycast(transform.position,Vector3.down, flyheightmax, whatIsGround)){

			rightheight = false;
			toohigh = true;
			toolow = false;}
		if(Physics2D.Raycast(transform.position,Vector3.down, flyheightmin, whatIsGround)){ //xaxaxa
			rightheight = false;
			toohigh = false;
			toolow = true;}
		
		if (rightheight)
			flyheightworld = _myTransform.position.y;
		else if (toohigh)
			flyheightworld = -100;
		else if (toolow)
			flyheightworld = 100;
		if (_myTransform.position.y < flyheightworld){

			rigidbody2D.gravityScale = -1;}
		else {
			rigidbody2D.gravityScale = +1;}
		flyspeedy = rigidbody2D.velocity.y;
		if (flyspeedy > 3 || flyspeedy<-3)
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y/2);
	}
	void Flip (){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
