using UnityEngine;
using System.Collections;

public class Spit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject,3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name=="Top Slash" || other.gameObject.name=="Front Slash")
			Destroy(gameObject,0f);
		if (other.gameObject.name=="Player"){
			
			other.gameObject.GetComponent<PlayerScript>().setstate = "Hit";
		other.gameObject.GetComponent<PlayerAttackColliders>().Hitlocked = true;
			GameObject spitter = GameObject.Find("jellyspitter");
			Transform parent = spitter.transform;
		Transform enemy = other.gameObject.transform;
		float distx = (parent.position.x - enemy.position.x);
		
		if (distx < 0)
			other.gameObject.GetComponent<PlayerScript>().hitfromleft = true;
		if (distx > 0)
			other.gameObject.GetComponent<PlayerScript>().hitfromleft = false;
			Destroy(gameObject,0f);
	}
		}
	}

