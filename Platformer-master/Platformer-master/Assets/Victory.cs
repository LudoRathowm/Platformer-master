using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour {
//	GameObject playa;
//	int turnright;
	// Use this for initialization
	void Start () {
	//	playa = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {
	/*	turnright =playa.GetComponent<PlatformerCharacter2D> ().turnright;
		if (turnright<0){
			Vector3 theScale = transform.localScale;
		theScale.x = -0.85f;
		transform.localScale = theScale;}
	*/	GameObject go = GameObject.FindGameObjectWithTag("Enemy"); 
		if (!go) {
			GetComponent<MeshRenderer>().enabled=true;
		}
	}
}
