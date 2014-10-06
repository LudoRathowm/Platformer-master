using UnityEngine;
using System.Collections;

public class PlayerAttackColliders : MonoBehaviour {
	PolygonCollider2D topslash;
    public bool Grounded;
	PolygonCollider2D frontal;
	PolygonCollider2D feetthrust;
	string SkillReceived;
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
		Lowslash
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
		Grounded = gameObject.GetComponent<PlatformerCharacter2D> ().grounded;
		anim = GetComponent<Animator>();
		SkillReceived = GetComponent<Test> ().SkillUsed;

	}

	private IEnumerator FSM(){
		while (_alive){
			switch (_attackstate) {
			case Attack.WaitingForImput:
				SkillReceived = GetComponent<Test> ().SkillUsed;
				Status4Mob = "Deciding";
				if (SkillReceived == "punch" && !delay)
					_attackstate = PlayerAttackColliders.Attack.Topslash;
				if (SkillReceived == "low" && !delay)
					_attackstate = PlayerAttackColliders.Attack.Lowslash;


				break;
			case Attack.Topslash:
				Status4Mob = "TopSlash";
				pTopslash();
				yield return new WaitForSeconds(0.2f);
				Topslash();
				yield return new WaitForSeconds(0.2f);
				cTopslash();
				break;
			case Attack.Lowslash:
				Status4Mob = "LowSlash";
				pLowslash();
				yield return new WaitForSeconds (0.1f);
				Lowslash();
				yield return new WaitForSeconds (0.2f);
				cLowslash();
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
		_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
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
		_attackstate = PlayerAttackColliders.Attack.WaitingForImput;
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