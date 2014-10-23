using UnityEngine;
using System.Collections;

public class AttackInputs : MonoBehaviour {
	public string SkillUsed = "nope";
	int facingright;

	public string LastAction; //for combos;
	float skilltimer;
	bool _alive = true;
	bool halfatktime = false;
	private KeyCombo Basic= new KeyCombo(new string[] {"Attack"});         //SHIT SUCKS, USE JUST FOR ROLL AND SUCH
	private KeyCombo ThrustR= new KeyCombo(new string[] {"Attack", "right"});
	private KeyCombo ThrustL= new KeyCombo(new string[] {"Attack", "left"});
	private KeyCombo feetthrust = new KeyCombo (new string[] {"Fire1", "down"});
	private KeyCombo topblock = new KeyCombo (new string[] {"BackHigh"});
	private enum InputState
	{
	    Waiting,
        Basic,
	    ThrustR,
		ThrustL,
		TopSlash,
		FootPierce,
		Combo,
		DoubleCombo

	}
	private InputState _input;



	// Use this for initialization
	void Start () {
	
		StartCoroutine ("FSM");
		_input = AttackInputs.InputState.Waiting;
	}

	private IEnumerator FSM(){
		while (_alive){
			switch (_input) {
			case InputState.Waiting:
				SkillUsed = "nope"; 
				if (Input.GetButtonDown("Attack")){
					skilltimer = 0.2f;
					_input = InputState.Basic;
				}

				if (Input.GetButtonDown("Attack") && Input.GetAxis("AtkDire") > 0)
					_input = InputState.ThrustR;
				if (Input.GetButtonDown("Attack") && Input.GetAxis("AtkDire") < 0)
					_input = InputState.ThrustL;
				if (Input.GetButtonDown("Attack") && Input.GetButtonDown("Uppi") && Input.GetAxis("AtkDire") == 0)
					_input = InputState.TopSlash;
				if (Input.GetButtonDown("Attack") && Input.GetAxis("Vertical") < 0)
					_input = InputState.FootPierce;
				break;
			case InputState.Basic:

				if (skilltimer > 0)                    //THIS GOES IN THE ATTACK COLLIDER, ILL JUST LEAVE IT HERE FOR NOW JUST AS A REMINDER;
					skilltimer -= Time.deltaTime;          // THIS GOES HERE 420 BLAZE IT
				if (skilltimer < 0)
					skilltimer = 0;					
				if (skilltimer == 0) {
					if (!halfatktime){
						Debug.Log ("NORMALTK");
						halfatktime = true;
						skilltimer = 0.2f;
					}
					Debug.Log ("END");
					_input = AttackInputs.InputState.Waiting;
				}
				if (Input.GetAxis("AtkDire") > 0)
					Debug.Log ("THRUSTATK");
					
				break;
			case InputState.ThrustR:
				SkillUsed = "ThrustR";
				yield return new WaitForSeconds(0.25f);
				_input = InputState.Waiting;
				break;
			case InputState.ThrustL:
				Debug.Log ("THRUST");
				SkillUsed = "ThrustL";
				yield return new WaitForSeconds(0.25f);
				_input = InputState.Waiting;
				break;
			case InputState.FootPierce:
				SkillUsed = "FootPierce";
				yield return new WaitForSeconds(0.3f);
				_input = InputState.Waiting;
				break;
			case InputState.TopSlash:
				SkillUsed = "TopSlash";
				yield return new WaitForSeconds(0.3f);
				_input = InputState.Waiting;
				break;

			}
			yield return null;
		}
	}
	void FixedUpdate (){
		if (LastAction != "Moving" && LastAction != "Jumping")
		LastAction = SkillUsed;
	}
	/*
	void Update () {		
		if (skilltimer > 0)
			skilltimer -= Time.deltaTime;
		if (skilltimer < 0)
			skilltimer = 0;
		if (skilltimer == 0) {
			SkillUsed = "nope";
		}
	    if (Input.GetButtonDown("Attack") && Input.GetAxis("Horizontal") > 0 && SkillUsed == "nope"){
			SkillUsed = "Thrust";
			skilltimer = 0.25f;
		}
		if (Input.GetButtonDown("Attack") && Input.GetAxis("Vertical") > 0 && SkillUsed == "nope"){
			skilltimer =0.3f;
			SkillUsed = "Top Slash";}
		if (Input.GetButtonDown("Attack") && Input.GetAxis("Vertical") < 0 && SkillUsed == "nope"){
			skilltimer = 0.3f;
			SkillUsed = "Foot Pierce";
		}
		if (Input.GetButtonDown("Attack") && LastAction == "Basic" && (Input.GetAxis("Horizontal") == 0) && (Input.GetAxis("Vertical") == 0) ){
			skilltimer = 0.2f;
			Debug.Log ("COMBO 1");
			SkillUsed = "Combo Attack";
		}
		if (Input.GetButtonDown("Attack") && LastAction == "Combo Attack" && (Input.GetAxis("Horizontal") == 0) && (Input.GetAxis("Vertical") == 0) ){
			Debug.Log ("COMBO 2");
		}

	}*/
}
