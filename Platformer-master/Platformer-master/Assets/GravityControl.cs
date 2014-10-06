using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour {

	Vector3 gravity;
	public int gravityamount = 10;
	public int gravitydirection;
	Transform camera;
	void Start()
	{
		gravity = Physics2D.gravity;
		GameObject _camera = GameObject.Find ("Main Camera");
		camera = _camera.transform;
	}
	void Update (){
		if (Input.GetButtonUp("RotateGravity"))
		gravitydirection = gravitydirection+1;
		if (Input.GetButtonUp("ARotateGravity"))
			gravitydirection = gravitydirection-1;
	}
	void FixedUpdate()
	{
		Physics2D.gravity = gravity;
		if(gravitydirection == 1)
		{
			gravity.x = gravityamount;
			gravity.y = 0;
			gravity.z = 0;
			camera.rotation = Quaternion.Euler ( 0, 0,90);
		}
		if(gravitydirection == 2)
		{
			gravity.x = 0;
			gravity.y = gravityamount;
			gravity.z = 0;
			camera.rotation = Quaternion.Euler ( 0, 0,180);
		}
		if (gravitydirection == 3){
			gravity.x = -gravityamount;
			gravity.y = 0;
			gravity.z = 0;
			camera.rotation = Quaternion.Euler ( 0, 0,-90);
		}
		if (gravitydirection == 0){
			gravity.x = 0;
			gravity.y = -gravityamount;
			gravity.z = 0;
			camera.rotation = Quaternion.Euler ( 0, 0,0);
		}
		if (gravitydirection > 3)
			gravitydirection = 0;
		if (gravitydirection < 0)
					gravitydirection = 3;
	}}
