using UnityEngine;
using System.Collections;

public class CheckGround : MonoBehaviour {
	public bool grounded;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

public void	OnTriggerEnter2D(Collider2D other) {
	GameObject target = other.gameObject;
	if (target.CompareTag ("Ground")) 
			grounded = true;
		}
public void OnTriggerExit2D (Collider2D other) {
		GameObject target = other.gameObject;
		if (target.CompareTag ("Ground"))
			grounded = false;
	}}