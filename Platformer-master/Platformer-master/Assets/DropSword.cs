using UnityEngine;
using System.Collections;

public class DropSword : MonoBehaviour {
	Transform target;
	Transform myTransform;
	float playerY ;
	float myY;
	float playerX;
	float myX;
	float droprange = 0.2f;
	int gravitystate;
	// Use this for initialization
	void Start () {
		myTransform = transform;

;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject Player = GameObject.FindGameObjectWithTag("Player");
		if (Player){
		target = Player.transform;
			gravitystate = Player.GetComponent<GravityControl>().gravitydirection;
	
	 playerY = target.position.y;
		 myY = myTransform.position.y;
		 playerX = target.position.x;
	 myX = myTransform.position.x;
			if (gravitystate == 0 || gravitystate == 2){
			if (playerX < myX + droprange && playerX > myX - droprange) 
				Destroy (this.gameObject);

			}
			if (gravitystate == 1 || gravitystate == 3){
				if (playerY<myY +droprange && playerY> myY -droprange)
					Destroy (this.gameObject);
			}
		}
	}}
