using UnityEngine;
using System.Collections;

public class JumpyEnemy : MonoBehaviour {
	Transform myTransform;
	public Transform target;
	bool grounded = false;						
	[SerializeField] LayerMask whatIsGround;	
	Transform groundCheck;	
	float groundedRadius = .2f;	
	[SerializeField] float jumpForce = 400f;	
	float jumptimer = 0;
	float shootimer = 0;
	float shootmaxtimer = 0.3f;
	public Rigidbody2D projectile;
		float maxjumptimer = 1.7f;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		Transform target = GameObject.FindGameObjectWithTag ("Player").transform;
	//	grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
	
				Transform target = GameObject.Find ("Player").transform;
				float playerY = target.position.y;
				float myY = myTransform.position.y;
				float playerX = target.position.x;
				float myX = myTransform.position.x;
				if (playerX > myX && myTransform.rotation.y != 0)
						myTransform.Rotate (0, 180, 0);
				//	if (playerX < myX)
				//					myTransform.Rotate (-180, 0, 0);


				if (jumptimer > 0)
						jumptimer -= Time.deltaTime;
				if (jumptimer < 0)
						jumptimer = 0;
				if (jumptimer == 0) {
						rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
						jumptimer = maxjumptimer;
				}
				
				
				if (shootimer > 0)
						shootimer -= Time.deltaTime;
				if (shootimer < 0)
						shootimer = 0;
				if (shootimer == 0) {
						if (playerY < myY + 0.2f && playerY > myY - 0.2f) {
				Shoot ();
								shootimer = shootmaxtimer;
						}
				}
		}					
				
void	Shoot (){

		Rigidbody2D instantiateProjectile = Instantiate(projectile,transform.position,transform.rotation) as Rigidbody2D;
		instantiateProjectile.velocity = transform.TransformDirection(new Vector3(-22,0,0));

	}


}