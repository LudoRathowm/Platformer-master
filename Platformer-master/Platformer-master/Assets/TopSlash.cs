using UnityEngine;
using System.Collections;

public class TopSlash : MonoBehaviour {
	bool entered;
	PolygonCollider2D thiscollider;
	void OnTriggerEnter2D(Collider2D other) {
				if (!entered) {
					//	Debug.Log ("HIT THE TOP");
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
