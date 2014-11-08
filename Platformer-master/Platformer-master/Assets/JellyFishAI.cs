using UnityEngine;
using System.Collections;

public class JellyFishAI : MonoBehaviour {
	Vector2 muhvelocity;
	float rotationtimer = 3; //reset rotation to avoid bugs
	public LayerMask mask; // avoid bumping on walls
    float direction;
	float rotatespeed = 70;
	float sindirection;
	float cosdirection;
	float raddirection;
	float force4movement = 100f;
	float littlefixrotation = 1f;
	Quaternion Direction;
	float muhspeedx; // stuff to decrease the speed afte you move
	float muhspeedy; 
	Transform _myTransform;

	bool _alive = true;
	private enum State
	{
		Initialization,
		PickDirection,
		Rotate,
		Move,
		Slowdown
	
	}
	State _state;

	// Use this for initialization
	void Start () {
		_myTransform = transform;
		_state = JellyFishAI.State.PickDirection;
		StartCoroutine ("FSM");
	}
	private IEnumerator FSM(){
		while (_alive){
			switch (_state){
			case State.PickDirection:
			direction = Random.Range (-360,360);
			
				Direction = Quaternion.Euler (0,0,direction);
				_state = State.Rotate;
				break;
			case State.Rotate:
				if (rotationtimer>0)
					rotationtimer-=Time.deltaTime;
				if (rotationtimer < 0)
					rotationtimer = 0;
				if (rotationtimer == 0){
					rotationtimer = 3;
					_state = State.Move;
				}
				_myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation,Direction,Time.deltaTime*rotatespeed);
				Debug.Log(_myTransform.rotation.eulerAngles.z);
				if (_myTransform.rotation.eulerAngles.z > direction-littlefixrotation
				    && _myTransform.rotation.eulerAngles.z < direction+littlefixrotation){
					Debug.Log ("WHYYY");
					if (Physics2D.Raycast(transform.position,Vector3.forward, 5, mask))
						Debug.Log ("WALL");
					else 
					_state = State.Move;}
				/*if (_myTransform.rotation.z == Direction.z || _myTransform.rotation.z == -Direction.z){
					if (Physics2D.Raycast(transform.position,Vector3.forward, 5, mask))		
						Debug.Log ("WALL");
					else 
					_state = State.Move;}*/
				break;
			case State.Move:
				raddirection = Mathf.Deg2Rad*direction;
				sindirection = Mathf.Sin(raddirection);
				cosdirection = Mathf.Cos(raddirection);
				Debug.Log ("the sin of " + direction+ " is "+sindirection);
				Debug.Log ("the cos of " + direction+ " is "+cosdirection);
				Vector2 movedirection = new Vector2 (force4movement*cosdirection,force4movement*sindirection);
				rigidbody2D.AddForce (movedirection, ForceMode2D.Impulse);
				_state = State.Slowdown;
				break;
			case State.Slowdown:
				if (rigidbody2D.velocity.y < 1 && rigidbody2D.velocity.y >-1 && rigidbody2D.velocity.x < 1 && rigidbody2D.velocity.x > -1)
					_state = State.PickDirection;
				break;


			
			

			}
		yield return null;
		}}
	// Update is called once per frame
	void Update () {



	}
}
