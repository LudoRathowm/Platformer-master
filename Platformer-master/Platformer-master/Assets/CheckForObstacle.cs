using UnityEngine;
using System.Collections;

public class CheckForObstacle : MonoBehaviour {
RaycastHit2D walao;
	float halfdistance;
	int distance = 1;
	public LayerMask mask;
	bool checkif;
	
	// Use this for initialization
	void Start () {
		halfdistance = collider2D.bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast(transform.position, Vector3.right, halfdistance + distance, mask))
			Debug.Log ("hit something m8");
	}
}

/*
float distToGround;

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
}*/