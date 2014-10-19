using UnityEngine;
using System.Collections;

public class JellySpitter : MonoBehaviour {
	public Rigidbody2D projectile;
	public bool shoot;
	float speed = 44f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	
		Transform target = GameObject.FindGameObjectWithTag("Player").transform;
		Vector3 vectorToTarget = target.position - transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
		if (shoot){
			Rigidbody2D instantiateProjectile = Instantiate(projectile,transform.position,transform.rotation) as Rigidbody2D;
			instantiateProjectile.velocity = transform.TransformDirection(new Vector3(22,0,0));
			shoot = false;
		}
	}
}
