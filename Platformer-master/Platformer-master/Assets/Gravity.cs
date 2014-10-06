using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {
	bool grounded;
	float halfheight = 0.3f;
	[SerializeField] LayerMask whatIsGround;
	// Use this for initialization
	void Start () {
		grounded = Physics2D.OverlapCircle(transform.position, halfheight, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
if (!grounded)
			transform.Translate(0,-1,0);
	}
}
