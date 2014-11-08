using UnityEngine;
using System.Collections;

public class Groundpoop : MonoBehaviour {
	float distx;
	bool slipright;
	bool slipleft;
	void Awake(){
		rigidbody2D.gravityScale = 0;
		Destroy (gameObject,3);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag("Player")){

			 distx = (other.gameObject.transform.position.x - transform.position.x);
			if (distx>0){

				
				other.gameObject.GetComponent<PlayerScript>().sliptime = 0.3f;
				other.gameObject.GetComponent<PlayerScript>().SlipL = true;
				}
			if (distx<0){
		
				other.gameObject.GetComponent<PlayerScript>().sliptime = 0.3f;
				other.gameObject.GetComponent<PlayerScript>().SlipR = true;
			
			}
		}}
	
		
		
		
		
	}

