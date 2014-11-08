using UnityEngine;
	using System.Collections;

	public class Poopscript : MonoBehaviour {
		public Rigidbody2D poopontheground;
	float muhspeed;
	void Start (){
		muhspeed = GetComponent<Rigidbody2D>().velocity.x;
		if (muhspeed>0)
			transform.rotation = Quaternion.Euler(0,0,33);

		if (muhspeed<0)
			transform.rotation = Quaternion.Euler(0,0,-33);;
	}

		void OnTriggerEnter2D(Collider2D other){
			if (other.gameObject.CompareTag("Player")){
				//apply status poop'd on
				Debug.Log ("YOU GOT SHIT ON");
				Destroy (gameObject,0);
			}
			  
			if (other.gameObject.CompareTag("Ground")){
			Quaternion rotation = Quaternion.Euler (0,0,0);
			Vector3 poopposition = new Vector3 (transform.position.x,transform.position.y-0.2f,0);
			GameObject instantiateProjectile = Instantiate(poopontheground,poopposition,rotation) as GameObject;

			}
					
				


		}

	}