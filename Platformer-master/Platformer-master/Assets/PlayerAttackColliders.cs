using UnityEngine;
using System.Collections;

public class PlayerAttackColliders : MonoBehaviour {
	public bool Grounded;
    string SkillReceived;
	public bool Hitlocked;
	public float skilltimer;
	public bool lastactionisbasicattack;
	bool parrylelr; //block other actions
	bool parrylell; //block other actions
	public bool lastactioniscomboattack;
	int turnright;
	bool _alive = true;
	public int parry;
	Animator anim;
	private Attack _attackstate;

	public string Status4Mob;
	Collider2D[] myhitboxes;
	private enum Attack	
	{
		WaitingForInput,
		Basic,
		BasicAttack,
		ThrustR,
		ThrustL,
		Topslash,
		Hitlock,
		RollR,
		RollL,
		RollPick,
		FootPierce,
		Combo,
		Combotwo,
		Parry,
		ParryR,
		ParryL,
	//	ParryFront,
	//	ParryBack
	
	}
	// Use this for initialization
// DO NOT USE THE COMBO SYSTEM ITS SHIT. SHIIIIIIT
	void Start () {
		_attackstate = PlayerAttackColliders.Attack.WaitingForInput;
	    StartCoroutine ("FSM");
		anim = GetComponent<Animator>();
		myhitboxes = gameObject.GetComponentsInChildren<Collider2D>();
		Debug.Log (myhitboxes[0]);
		Debug.Log (myhitboxes[1]);
		Debug.Log (myhitboxes[2]);
		Debug.Log (myhitboxes[3]);
		Debug.Log (myhitboxes[4]);
		Debug.Log (myhitboxes[5]);
		Debug.Log (myhitboxes[6]);
		Debug.Log (myhitboxes[7]);
		Debug.Log (myhitboxes[8]);
		Debug.Log (myhitboxes[9]);

	}

	private IEnumerator FSM(){
		while (_alive){
			switch (_attackstate) {
			case Attack.WaitingForInput:
				Status4Mob = "Deciding";
				if (Input.GetButtonDown("Attack")){
					skilltimer = 0.25f;
					_attackstate = Attack.Basic;
				}
				else if (Hitlocked)
					_attackstate = PlayerAttackColliders.Attack.Hitlock;
				if (Input.GetButtonDown("Parry")){
					skilltimer = 0.2f;
					_attackstate = Attack.Parry;}
				if (Input.GetAxis("AtkDire")!=0){
					skilltimer = 0.25f;
					_attackstate = Attack.RollPick;
				}

						
				
				break;				
			case Attack.Basic:
				Grounded = GetComponent<PlayerScript>().grounded;
				Status4Mob = "BasicAttack"; //kek
				if (skilltimer > 0)               {     //THIS GOES IN THE ATTACK COLLIDER, ILL JUST LEAVE IT HERE FOR NOW JUST AS A REMINDER;
					skilltimer -= Time.deltaTime;  
				if (lastactionisbasicattack)
						skilltimer -= Time.deltaTime;  //if im comboing im faster
				}        // THIS STAYS HERE 420 BLAZE IT
				if (skilltimer < 0)
					skilltimer = 0;					
				if (skilltimer == 0) {
					if (!lastactionisbasicattack && !lastactioniscomboattack)
					_attackstate = Attack.BasicAttack;
					if (lastactionisbasicattack && !lastactioniscomboattack)
						_attackstate = Attack.Combo;
					if (lastactioniscomboattack && !lastactionisbasicattack)
						_attackstate = Attack.Combotwo;
					}
					
				if (Input.GetAxis("AtkDire") < 0) // AUTISM
					_attackstate =Attack.ThrustL;


				if (Input.GetAxis("AtkDire") > 0)
					_attackstate =Attack.ThrustR;

				if (Input.GetAxis("Vertical")>0)
					_attackstate = Attack.Topslash;
				if (Input.GetAxis("Vertical")<0 && !Grounded)
					_attackstate = Attack.FootPierce;
				break;
			case Attack.RollPick:
				if (Input.GetAxis("AtkDire")>0)
				    {
					if (skilltimer > 0)                   
						skilltimer -= Time.deltaTime;     
					if (skilltimer < 0)
						skilltimer = 0;					
					if (skilltimer == 0) {
						_attackstate = Attack.WaitingForInput;
					}
				if (Input.GetButtonDown("Attack"))
						_attackstate = Attack.RollR;
				}
				if (Input.GetAxis("AtkDire")<0)
				{
					if (skilltimer > 0)                   
						skilltimer -= Time.deltaTime;     
					if (skilltimer < 0)
						skilltimer = 0;					
					if (skilltimer == 0) {
						_attackstate = Attack.WaitingForInput;
					}
					if (Input.GetButtonDown("Attack"))
						_attackstate = Attack.RollL;
				}
				if (skilltimer > 0)                   
					skilltimer -= Time.deltaTime;     
				if (skilltimer < 0)
					skilltimer = 0;					
				if (skilltimer == 0) {
					_attackstate = Attack.WaitingForInput;}
				break;

				
			case Attack.BasicAttack:
				Status4Mob = "BasicAttack";
				pBasicAttack(); //basic atk already waited enought
				yield return new WaitForSeconds(0.1f);
				BasicAttack();
				yield return new WaitForSeconds(0.2f);
				cBasicAttack();
				lastactionisbasicattack = true;
				lastactioniscomboattack = false;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.ThrustR:
				Status4Mob = "Thrust";
				turnright = GetComponent<PlayerScript>().turnright;
				if (turnright == 1){
					pThrust();
					yield return new WaitForSeconds(0.01f);
					Thrust();
					yield return new WaitForSeconds(0.1f);
					cThrust();
					lastactionisbasicattack = false;
					lastactioniscomboattack = false;
					_attackstate = Attack.WaitingForInput;
				}
				if (turnright == -1){
		
					pKick();
					yield return new WaitForSeconds(0.1f);
					Kick();
					yield return new WaitForSeconds(0.2f);
					cKick();
					GetComponent<PlayerScript>().muhflip = true;
					lastactionisbasicattack = false;
					lastactioniscomboattack = false;
					_attackstate = Attack.WaitingForInput;
				}
				break;
			case Attack.ThrustL:
				Status4Mob = "Thrust";
				turnright = GetComponent<PlayerScript>().turnright;
				if (turnright == -1){
					pThrust();
					yield return new WaitForSeconds(0.01f);
					Thrust();
					yield return new WaitForSeconds(0.1f);
					cThrust();
					lastactionisbasicattack = false;
					lastactioniscomboattack = false;
					_attackstate = Attack.WaitingForInput;
				}
				if (turnright == 1){
		
					pKick();
					yield return new WaitForSeconds(0.1f);
					Kick();
					yield return new WaitForSeconds(0.2f);
					cKick();
					GetComponent<PlayerScript>().muhflip = true;
					lastactionisbasicattack = false;
					lastactioniscomboattack = false;
					_attackstate = Attack.WaitingForInput;
				}
				break;
			case Attack.Combo:
				Status4Mob = "ComboAttack";
				pBasicAttack(); //1st combo is basically a normal a bit stronger and faster attack
				BasicAttack();
				yield return new WaitForSeconds(0.1f);
				cBasicAttack();
	
				lastactionisbasicattack = false;
				lastactioniscomboattack = true;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.Combotwo:
				Status4Mob = "DoubleComboAttack";
				pComboAttack();
				yield return new WaitForSeconds(0.3f);
				ComboAttack();
				yield return new WaitForSeconds(0.2f);
				cComboAttack();
			
				lastactionisbasicattack = false;
				lastactioniscomboattack = false;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.RollL:
				Status4Mob = "RollL";
				/*if (turnright == -1)                     //implying i have animations
					anim.SetBool (Roll,true);
				if (turnright == 1)
					anim.SetBool (Cart,true);*/
				myhitboxes[0].enabled = false;
				myhitboxes[1].enabled = false;
				myhitboxes[2].enabled=true;
				yield return new WaitForSeconds (0.1f);
				myhitboxes[0].enabled = true;
				myhitboxes[1].enabled = true;
				//anim.SetBool (Cart,false);
				//anim.SetBool (Roll,false);
				myhitboxes[2].enabled = false;
				lastactionisbasicattack = false;
				lastactioniscomboattack = false;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.RollR:
				Status4Mob = "RollR";
				/*if (turnright == 1)                     //implying i have animations
					anim.SetBool (Roll,true);
				if (turnright == -1)
					anim.SetBool (Cart,true);*/
				myhitboxes[0].enabled = false;
				myhitboxes[1].enabled = false;
				myhitboxes[2].enabled=true;
				yield return new WaitForSeconds (0.1f);
				myhitboxes[0].enabled = true;
				myhitboxes[1].enabled = true;
				//anim.SetBool (Cart,false);
				//anim.SetBool (Roll,false);
				myhitboxes[2].enabled = false;
				lastactionisbasicattack = false;
				lastactioniscomboattack = false;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.FootPierce:
				Status4Mob = "FootPierce";
				pFootPierce();
				yield return new WaitForSeconds (0.2f);
				FootPierce();
				Grounded = GetComponent<PlayerScript>().grounded;
				if (Grounded){
					cFootPierce();					
					lastactionisbasicattack = false;
					lastactioniscomboattack = false;
					_attackstate = Attack.WaitingForInput;
				}
				yield return new WaitForSeconds (0.5f);
				cFootPierce();
				lastactionisbasicattack = false;
				lastactioniscomboattack = false;
				_attackstate = Attack.WaitingForInput;
				break;
			case Attack.Topslash:
				Status4Mob = "TopSlash";
				pTopslash();
				yield return new WaitForSeconds(0.2f);
				Topslash();
				yield return new WaitForSeconds(0.2f);
				cTopslash();
				lastactionisbasicattack = false;
				_attackstate = PlayerAttackColliders.Attack.WaitingForInput;
				break;
			case Attack.Hitlock:
				yield return new WaitForSeconds (0.2f);
				Hitlocked = false;
				lastactionisbasicattack = false;
				lastactioniscomboattack = false;
				_attackstate = PlayerAttackColliders.Attack.WaitingForInput;
				break;
			case Attack.Parry:
				Status4Mob = "Parry";
				turnright = GetComponent<PlayerScript>().turnright;
				if (skilltimer > 0)                   
					skilltimer -= Time.deltaTime;     
				if (skilltimer < 0)
					skilltimer = 0;					
				if (skilltimer == 0) {
					if (turnright == 1)
						_attackstate = Attack.ParryR;
					if (turnright == -1)
						_attackstate = Attack.ParryL;
				}
			
				if (Input.GetAxis("AtkDire")>0 )
					_attackstate = Attack.ParryR;

				if (Input.GetAxis("AtkDire")<0 )
					_attackstate = Attack.ParryL;
				break;/*
			case Attack.ParryBack:
				turnright = GetComponent<PlayerScript>().turnright;
				parry = -1;
				if (turnright == -1){

					//anim.blabla.activateit.parryfront.
					yield return new WaitForSeconds (0.5f);
						//anim.blablav2
						parry = 0;
					_attackstate = Attack.WaitingForInput;
				}
				if (turnright == 1){
					//anim dicks
					yield return new WaitForSeconds (0.5f);
					GetComponent<PlayerScript>().muhflip = true;
					parry = 0;
					_attackstate = Attack.WaitingForInput;

				}
				break;
			case Attack.ParryFront:
				turnright = GetComponent<PlayerScript>().turnright;
				parry = 1;
				if (turnright == 1){
					
					//anim.blabla.activateit.parryfront.
					yield return new WaitForSeconds (0.5f);
					//anim.blablav2
					parry = 0;
					_attackstate = Attack.WaitingForInput;
				}
				if (turnright == -1){
					//anim dicks
					yield return new WaitForSeconds (0.5f);
					GetComponent<PlayerScript>().muhflip = true;
					parry = 0;
					_attackstate = Attack.WaitingForInput;
					
				}
				break;*/
			case Attack.ParryR:
				turnright = GetComponent<PlayerScript>().turnright;
				parry = 1;
				Status4Mob = "ParryR";
				//if (turnright == 1)
					//anim.normale, solo anim in cui tiene la spada, niente movimento per avvicinarla
				//if (turnright == -1)
						//anim.anormale
				
				if (Input.GetButtonUp("Parry")){
					Debug.Log ("FUCK ME");
					parry = 0;
					//	anim.blablacloseall
					_attackstate = Attack.WaitingForInput;
				}
				break;
			case Attack.ParryL:
				turnright = GetComponent<PlayerScript>().turnright;
				parry = -1;
				Status4Mob = "ParryL";
				//if (turnright == -1)
					//anim.normale, solo anim in cui tiene la spada, niente movimento per avvicinarla
				//	if (turnright == +1)
						//anim.anormale
					if (Input.GetButtonUp("Parry")){
						Debug.Log ("FUCK ME");
						parry = 0;
						//	anim.blablacloseall
						_attackstate = Attack.WaitingForInput;
					}
				break;
			}

			yield return null;
		}	
	}
	void pFootPierce(){}
	void FootPierce(){
		myhitboxes[6].enabled=true;
	}
	void cFootPierce(){
		myhitboxes[6].enabled=false;
	}

	void pBasicAttack(){
		/*if (!lastactionisbasicattack)
			normalanimation
				else if (lastactionisbasicattack)
					gayasscomboanimation*/
	}
	void BasicAttack (){
		myhitboxes[3].enabled = true;
	}
	void cBasicAttack (){
		myhitboxes [3].enabled = false;
		//close animations duh
	}
	void pThrust (){}
	void Thrust (){
		myhitboxes[4].enabled = true;
	}
	void cThrust(){
		myhitboxes[4].enabled=false;
	}
	void pKick(){}
	void Kick (){
		myhitboxes[10].enabled=true;
	}
	void cKick(){
		myhitboxes[10].enabled = false;
	}
	void pComboAttack(){
		//>having animations
	}
	void ComboAttack(){
		myhitboxes[9].enabled = true;
	}
	void cComboAttack(){
		myhitboxes[9].enabled= false;
	}
	void pTopslash(){
		anim.SetBool ("Topslash", true);
	}
	void Topslash () {		
					myhitboxes[5].enabled = true;
		}	
	void cTopslash () {
		myhitboxes[5].enabled = false;
		anim.SetBool ("Topslash", false);
		}
	void pLowslash(){
		anim.SetBool ("Lowslash", true);
	}













	
	// Update is called once per frame
/*	void Update () {	
		if (skilltimer > 0)
						skilltimer -= Time.deltaTime;
		if (skilltimer < 0)
						skilltimer = 0;
		if (skilltimer == 0) {
			anim.SetBool("Topslash", false);
			anim.SetBool("Frontslash", false);
			topslash.enabled=false;	
			frontal.enabled = false;
			feetthrust.enabled=false;
							}
		SkillReceived = GetComponent<Test> ().SkillUsed;


		if (SkillReceived == "low" && skilltimer == 0){
			frontal.enabled = true;
			skilltimer = 0.1f;
			anim.SetBool ("Frontslash", true);
		}
		if (SkillReceived == "feetthrust" && skilltimer == 0 && !Grounded) {
			feetthrust.enabled=true;
			skilltimer = 1f;
	
		}
}*/
}