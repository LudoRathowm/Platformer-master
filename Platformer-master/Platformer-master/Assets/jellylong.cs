using UnityEngine;
using System.Collections;

public class jellylong : MonoBehaviour {
	bool entered;
	PolygonCollider2D thiscollider;
	void OnTriggerEnter2D(Collider2D other) {
		if (!entered && other.gameObject.CompareTag("Player") ) {
			other.gameObject.GetComponent<PlayerScript>().setstate = "Hit";
			other.gameObject.GetComponent<PlayerAttackColliders>().Hitlocked = true;
			
			Transform parent = transform.parent.transform;
			Transform enemy = other.gameObject.transform;
			float distx = (parent.position.x - enemy.position.x);
			
			if (distx < 0)
				other.gameObject.GetComponent<PlayerScript>().hitfromleft = true;
			if (distx > 0)
				other.gameObject.GetComponent<PlayerScript>().hitfromleft = false;
			entered = true;
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
