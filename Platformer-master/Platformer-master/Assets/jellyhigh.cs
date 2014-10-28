using UnityEngine;
using System.Collections;

public class jellyhigh : MonoBehaviour {
	bool entered;
	public	bool parried ;
	PolygonCollider2D thiscollider;
	void OnTriggerEnter2D(Collider2D other) {
		if (!entered && other.gameObject.CompareTag("Player") ) {
			Transform parent = transform.parent.transform;
			Transform enemy = other.gameObject.transform;
			float distx = (parent.position.x - enemy.position.x);
			int parry = enemy.gameObject.GetComponent<PlayerAttackColliders>().parry; //-1 parry back, 0 no parry, 1 parry front

			if (distx<0 && parry !=-1 || distx>0 && parry != 1){

			other.gameObject.GetComponent<PlayerScript>().setstate = "Hit";
			other.gameObject.GetComponent<PlayerAttackColliders>().Hitlocked = true;



			if (distx < 0)
			other.gameObject.GetComponent<PlayerScript>().hitfromleft = true;
			if (distx > 0)
			other.gameObject.GetComponent<PlayerScript>().hitfromleft = false;
			}
		}
	}
	
	// Use this for initialization;
	
	void Start () {
		thiscollider = gameObject.GetComponent<PolygonCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (thiscollider.enabled == false)
			entered = false;

		

	}
}
