using UnityEngine;
public class PlatformerCharacter2D : MonoBehaviour
{
	bool facingRight = true;	// For determining which way the player is currently facing.
	[SerializeField] float maxSpeed = 10f;	// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;	// Amount of force added when the player jumps.
	[Range(0, 1)]
	[SerializeField] float crouchSpeed = .36f;	// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] bool airControl = false;	// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;	// A mask determining what is ground to the character
	Transform groundCheck;	// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;	// Radius of the overlap circle to determine if grounded
	public bool grounded = false;	// Whether or not the player is grounded.
	Transform ceilingCheck;	// A position marking where to check for ceilings
	float ceilingRadius = .01f;	// Radius of the overlap circle to determine if the player can stand up
	Animator anim;	// Reference to the player's animator component.
	public int turnright;
	int gravity;
	int turndirection;
	Transform newrotation;
	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
		gravity = 0;
		newrotation = transform;

	}
	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);
		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		if (facingRight == true)
			turnright = 1;
		if (facingRight != true)
			turnright = -1;
	//	gravity = GetComponent<GravityControl>().gravitydirection;
		AdjustDirection();
	}
	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}
		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);
		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move * crouchSpeed : move);
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));
			// Move the character
			if (gravity == 0)
			    rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			if (gravity == 1)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, move*maxSpeed);
			if (gravity == 2)
				rigidbody2D.velocity = new Vector2 (-move*maxSpeed, rigidbody2D.velocity.y);
			if (gravity == 3)
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -move*maxSpeed);
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}
		// If the player should jump...
		if (grounded && jump) {
			// Add a vertical force to the player.
			anim.SetBool("Ground", false);
			if (gravity == 0)
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			if (gravity == 1)
				rigidbody2D.AddForce(new Vector2(-jumpForce,0f));
			if (gravity == 2)
				rigidbody2D.AddForce(new Vector2(0,-jumpForce));
			if (gravity == 3)
				rigidbody2D.AddForce(new Vector2(jumpForce, 0f));
		}
	}
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	void AdjustDirection (){
		transform.rotation = Quaternion.Slerp(transform.rotation, newrotation.rotation,5f);
		if (gravity == 1){
			if (transform.rotation != Quaternion.Euler ( 0, 0,90))
				newrotation.rotation = Quaternion.Euler ( 0, 0,90);
				}
		if (gravity == 2){
			if (transform.rotation != Quaternion.Euler ( 0, 0,180))
				newrotation.rotation = Quaternion.Euler ( 0, 0,180);
		}
		if (gravity == 3){
			if (transform.rotation != Quaternion.Euler ( 0, 0,-90))
				newrotation.rotation = Quaternion.Euler ( 0, 0,-90);
		}
		if (gravity == 0){
			if (transform.rotation != Quaternion.Euler ( 0, 0,0))
				newrotation.rotation = Quaternion.Euler ( 0, 0,0);
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, newrotation.rotation,5f);
	}
}

