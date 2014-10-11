using UnityEngine;
using System.Collections;

public class jellylow : MonoBehaviour {
	Collider2D other;
	bool entered;
	bool knockattack;
	float kickforce = 30;
	float disty;
	float distx;
	Transform histransform ;
	bool thrown;
	GameObject player;
	PolygonCollider2D thiscollider;
	bool grounded;
	void OnTriggerEnter2D (Collider2D other) {
		if (!entered && other.gameObject.CompareTag("Player")) {
			knockattack = GetComponentInParent<JellyAI>().knockattack;		
			if (knockattack){
				other.gameObject.GetComponent<PlatformerCharacter2D>().enabled = false;
				other.gameObject.GetComponent<Platformer2DUserControl>().enabled = false;
				other.gameObject.GetComponent<Animator>().SetBool("Ground",false);
				Debug.Log ("KNOCKATTACKU");
				thrown = true;
			histransform = transform.parent.GetComponent<Transform>();
			distx = (other.gameObject.transform.position.x - histransform.position.x);
				Rigidbody2D targetrigid = other.gameObject.rigidbody2D;
			if (distx > 0)
						targetrigid.velocity = new Vector2 (15,15);
			if (distx <0)
					targetrigid.velocity = new Vector2 (-15,15);
				entered = true;}
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
		histransform = transform.parent.GetComponent<Transform>();
		distx = (player.transform.position.x - histransform.position.x);
		disty = (histransform.position.y -player.transform.position.y );
		if (disty > 0.4f && thrown == true){ //15 is the distance for the player to fall down before being able to move again
		player.GetComponent<Platformer2DUserControl>().enabled = true;
			player.GetComponent<PlatformerCharacter2D>().enabled = true;

			thrown = false;}
	}
}

