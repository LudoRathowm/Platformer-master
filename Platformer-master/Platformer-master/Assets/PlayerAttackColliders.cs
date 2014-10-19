using UnityEngine;
using System.Collections;

public class PlayerAttackColliders : MonoBehaviour {
	PolygonCollider2D topslash;
    public bool Grounded;
	PolygonCollider2D frontal;
	PolygonCollider2D feetthrust;
	string SkillReceived;
	public bool Hitlocked;
	public float skilltimer;
	bool _alive = true;
	Animator anim;
	private Attack _attackstate;
	bool delay;
	public string Status4Mob;
	private enum Attack
	{
		WaitingForImput,
		Topslash,
		Lowslash,
		Hitlock
	}
	// Use this for initialization
	void Start () {
		_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
	    StartCoroutine ("FSM");

		GameObject _topslash = GameObject.Find ("Top Slash");
		topslash = _topslash.GetComponent<PolygonCollider2D> ();
		GameObject _frontal = GameObject.Find ("Front Slash");
		frontal = _frontal.GetComponent<PolygonCollider2D> ();
		GameObject _feetthrust = GameObject.Find ("Foot Pierce");
		feetthrust = _feetthrust.GetComponent<PolygonCollider2D> ();
		Grounded = gameObject.GetComponent<PlayerScript> ().grounded;
		anim = GetComponent<Animator>();
		SkillReceived = GetComponent<Test> ().SkillUsed;

	}

	private IEnumerator FSM(){
		while (_alive){
			switch (_attackstate) {
			case Attack.WaitingForImput:
				Status4Mob = "Deciding";
				SkillReceived = GetComponent<Test> ().SkillUsed;
				if (!Hitlocked){
				
				if (SkillReceived == "punch" && !delay)
					_attackstate = PlayerAttackColliders.Attack.Topslash;
				if (SkillReceived == "low" && !delay)
					_attackstate = PlayerAttackColliders.Attack.Lowslash;}
				else if (Hitlocked)
					_attackstate = PlayerAttackColliders.Attack.Hitlock;
				break;
			case Attack.Topslash:
				Status4Mob = "TopSlash";
				pTopslash();
				yield return new WaitForSeconds(0.2f);
				Topslash();
				yield return new WaitForSeconds(0.2f);
				cTopslash();
				_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
				break;
			case Attack.Lowslash:
				Status4Mob = "LowSlash";
				pLowslash();
				yield return new WaitForSeconds (0.1f);
				Lowslash();
				yield return new WaitForSeconds (0.2f);
				cLowslash();
				_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
				break;
			case Attack.Hitlock:
				yield return new WaitForSeconds (0.2f);
				_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
				break;
			}
			yield return null;
		}	
	}
	void pTopslash(){
		anim.SetBool ("Topslash", true);
	}
	void Topslash () {		
					topslash.enabled = true;
		}	
	void cTopslash () {
		topslash.enabled = false;
		anim.SetBool ("Topslash", false);
		}
	void pLowslash(){
		anim.SetBool ("Lowslash", true);
	}
	void Lowslash () {
		frontal.enabled = true;

	}
	void cLowslash(){
		frontal.enabled = false;
		anim.SetBool ("Lowslash", false);
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