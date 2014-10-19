using UnityEngine;
using System.Collections;

public class jellylow : MonoBehaviour {
	Collider2D other;
	bool entered;
	bool knockattack;
	float kickforce = 30;


	Transform histransform ;
	bool thrown;
	GameObject player;
	PolygonCollider2D thiscollider;
	bool grounded;
	void OnTriggerEnter2D (Collider2D other) {
		if (!entered && other.gameObject.CompareTag("Player")) {
			knockattack = GetComponentInParent<JellyAI>().knockattack;	
			if (!knockattack){
				other.gameObject.GetComponent<PlayerScript>().setstate = "Hit";
				other.gameObject.GetComponent<PlayerAttackColliders>().Hitlocked = true;
				
				Transform parent = transform.parent.transform;
				Transform enemy = other.gameObject.transform;
				float distx = (parent.position.x - enemy.position.x);
				
				if (distx < 0)
					other.gameObject.GetComponent<PlayerScript>().hitfromleft = true;
				if (distx > 0)
					other.gameObject.GetComponent<PlayerScript>().hitfromleft = false;
			}
			if (knockattack){
				other.gameObject.GetComponent<PlayerScript>().setstate = "Thrown";
				other.gameObject.GetComponent<PlayerAttackColliders>().Hitlocked = true;
		
				
				Transform parent = transform.parent.transform;
				Transform enemy = other.gameObject.transform;
				float distx = (parent.position.x - enemy.position.x);
				
				if (distx < 0)
					other.gameObject.GetComponent<PlayerScript>().hitfromleft = true;
				if (distx > 0)
					other.gameObject.GetComponent<PlayerScript>().hitfromleft = false;}
		}
	}
	
	// Use this for initialization;
	
	void Start () {
		thiscollider = gameObject.GetComponent<PolygonCollider2D> ();
	player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (thiscollider.enabled == false)
			entered = false;
	}
}

