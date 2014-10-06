using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {


	public void	OnTriggerEnter2D(Collider2D other) {
		GameObject target = other.gameObject;
		if (target.CompareTag ("Player")) {

			Destroy (target.gameObject);}
}
}