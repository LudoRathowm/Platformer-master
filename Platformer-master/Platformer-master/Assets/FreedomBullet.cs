using UnityEngine;
using System.Collections;

public class FreedomBullet : MonoBehaviour {
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
		if (target.CompareTag ("Enemy")) {
			other.gameObject.GetComponent<EnemyHealth> ().curHealth -= 15;
			Destroy (gameObject);
		}
		
	}
}



