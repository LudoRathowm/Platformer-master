using UnityEngine;
using System.Collections;

public class PlayerAttackColliders : MonoBehaviour {
	PolygonCollider2D topslash;
	bool Grounded;
	PolygonCollider2D frontal;
	PolygonCollider2D feetthrust;
	string SkillReceived;
	public float skilltimer;
	Animator anim;

	// Use this for initialization
	void Start () {
		GameObject _topslash = GameObject.Find ("Top Slash");
		topslash = _topslash.GetComponent<PolygonCollider2D> ();
		GameObject _frontal = GameObject.Find ("Front Slash");
		frontal = _frontal.GetComponent<PolygonCollider2D> ();
		GameObject _feetthrust = GameObject.Find ("Foot Pierce");
		feetthrust = _feetthrust.GetComponent<PolygonCollider2D> ();
		Grounded = gameObject.GetComponent<PlatformerCharacter2D> ().grounded;
		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {	
		if (skilltimer > 0)
						skilltimer -= Time.deltaTime;
		if (skilltimer < 0)
						skilltimer = 0;
		if (skilltimer == 0) {
			anim.SetBool("Topslash", false);
			topslash.enabled=false;	
			feetthrust.enabled=false;

				}
		SkillReceived = GetComponent<Test> ().SkillUsed;

		if (SkillReceived == "punch" && skilltimer == 0) {
			topslash.enabled = true;
			skilltimer = 0.2f;
			anim.SetBool ("Topslash", true);
				}
		if (SkillReceived == "feetthrust" && skilltimer == 0 && !Grounded) {
			feetthrust.enabled=true;
			skilltimer = 1f;
	
		}
}
}