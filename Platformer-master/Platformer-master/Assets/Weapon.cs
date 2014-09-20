using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public float fireRate = 0;
	public float Damage = 10;
	public LayerMask notToHit;
	public Transform BulletTrailPrefab;
	private float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;
	private float timeToFire = 0;
	Transform firePoint;
	public Rigidbody2D projectile;
	int turnright;
	GameObject go;
	// Use this for initialization
	void Awake () {
		go = GameObject.Find("Player");

		firePoint = transform.FindChild("FirePoint");
		if (firePoint == null)
			Debug.Log ("No FirePoint NIGGA");

	}
	
	// Update is called once per frame
	void Update () {
		turnright =go.GetComponent<PlatformerCharacter2D> ().turnright;

				if (fireRate == 0) {
			if ( Input.GetButtonDown ("Fire1")){
			    	Shoot ();
			}
		}
			else {
				if (Input.GetButton ("Fire1") && Time.time > timeToFire){
					timeToFire = Time.time + 1/fireRate;
					Shoot ();
				}
			}
	}

			void Shoot (){
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition-firePointPosition, 100, notToHit);
		if (Time.time >= timeToSpawnEffect) {
			Effect ();
			timeToSpawnEffect = Time.time + 1/effectSpawnRate;
		}

		Debug.DrawLine (firePointPosition, mousePosition);
	}
	void Effect (){
	//	Instantiate (BulletTrailPrefab, firePoint.position, firePoint.rotation);
		Shootnigger ();
	}
	void	Shootnigger (){
		
		Rigidbody2D instantiateProjectile = Instantiate(projectile,transform.position,transform.rotation) as Rigidbody2D;
		instantiateProjectile.velocity = transform.TransformDirection(new Vector3((22*turnright),0,0));
		
	}
}
