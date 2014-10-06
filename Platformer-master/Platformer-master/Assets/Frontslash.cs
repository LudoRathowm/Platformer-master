﻿using UnityEngine;
using System.Collections;

public class Frontslash : MonoBehaviour {
	bool entered;
	PolygonCollider2D thiscollider;
	void OnTriggerEnter2D(Collider2D other) {
		if (!entered && other.gameObject.CompareTag("Enemy")) {
				Debug.Log ("hit you");
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