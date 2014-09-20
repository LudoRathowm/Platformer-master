using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {
	public int moveSpeed = 230;

		// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 1);
	}

	
	public void	OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("HIT");
		GameObject target = other.gameObject;
		if (target.CompareTag ("Enemy")) {
			other.gameObject.GetComponent<EnemyHealth> ().curHealth -= 15;
			Destroy (gameObject);
		}
		
	}
}
