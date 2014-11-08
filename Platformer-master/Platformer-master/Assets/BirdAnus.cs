using UnityEngine;
using System.Collections;

public class BirdAnus : MonoBehaviour {
	public Rigidbody2D birdpoop;
	public bool poop;
	
	void Update () {	
		if (poop){
		
			Rigidbody2D instantiateProjectile = Instantiate(birdpoop,transform.position,transform.rotation) as Rigidbody2D;
			bool poopshootleft = GetComponentInParent<BirdAI>().FacingRight;
			if (poopshootleft)
			instantiateProjectile.velocity = transform.TransformDirection(new Vector3(-5,0,0)); //poop2back
			else if (!poopshootleft)
				instantiateProjectile.velocity = transform.TransformDirection(new Vector3(5,0,0)); //poop2back
			poop = false;
		}
	}
}
