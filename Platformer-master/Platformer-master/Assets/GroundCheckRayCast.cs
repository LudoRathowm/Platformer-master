using UnityEngine;
using System.Collections;

public class GroundCheckRayCast : MonoBehaviour {
	float distToGround;
	RaycastHit2D walao;

	// Use this for initialization
	void Start () {
		distToGround = collider2D.bounds.extents.y;
	}



	

	// Update is called once per frame
	void Update () {
		walao = Physics2D.Raycast(transform.position, -Vector3.up, distToGround +0.1f);
	if (walao.collider != null)
			Debug.Log (walao);
	}
}
