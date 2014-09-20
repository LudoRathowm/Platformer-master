using UnityEngine;
using System.Collections;

public class Bulleteffect : MonoBehaviour {
	Collider2D other;
	float timer = 2;

	// Use this for initialization
	void Start () {

	}
	void Update (){
		if (timer> 0)
			timer -= Time.deltaTime;
		if (timer < 0)
			timer = 0;
		if (timer == 0) {
			Destroy(gameObject);

		}
	}
	
	public void	OnTriggerEnter2D(Collider2D other) {
		GameObject target = other.gameObject;
		if (target.CompareTag ("Player")) {
			bool cantHit = other.gameObject.GetComponent<Test>().isTopBlocking;
			Debug.Log (cantHit);
			if (!cantHit)
			other.gameObject.GetComponent<PlayerHealth> ().curHealth -= 15;
			Destroy (gameObject);
				}
	
		}
	}



