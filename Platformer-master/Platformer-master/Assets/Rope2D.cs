using UnityEngine;
using System.Collections;

public class Rope2D : MonoBehaviour {    //prevents the object from moving and rotating in a way unfit for 2D
	float zposition;
	float xangle;
	float yangle;
	void Start (){
		zposition = transform.localPosition.z;
		xangle = 	transform.eulerAngles.x;
		yangle = transform.eulerAngles.y;
	}
	void FixedUpdate () {

		zposition = 0;
		xangle = 0;
		yangle = 0;
	//	transform.localPosition.z = 0;
	//	transform.eulerAngles.x = 0;
	//	transform.eulerAngles.y = 0;
	}

}
