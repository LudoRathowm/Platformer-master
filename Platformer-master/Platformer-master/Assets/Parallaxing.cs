using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {
	public Transform[] backgrounds; //array of all back and foregrounds to be parallaxed
	private float [] parallaxScales; // the proportion of the cameras movement to move the backgrounds by
	public float smoothing = 1f; //how smooth is this going to be. above zero obv.

	private Transform cam; //reference to the main camera transform
	private Vector3 previousCamPosition; //the position of the camera in the previous frame

	//awk is called before start and after the gameobjects are setup. use it for references
	void Awake () {
		cam = Camera.main.transform;
			}

	// Use this for initialization
	void Start () {
	//the previous frame had the current frame's camera position
		previousCamPosition = cam.position;
		//assigning parallax scales
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z*-1;		
		}

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			float parallax = (previousCamPosition.x-cam.position.x)* parallaxScales[i];		
		//set a target x position which is the current position plus the parallax
			float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

		//create a target position which is the background's current position with it's target x position
		Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPositionX, backgrounds[i].position.y, backgrounds [i].position.z);

	    //fade between positions
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position,backgroundTargetPos, smoothing*Time.deltaTime);
			}
		//set the previouscampos to the camera's position at the end of the frame
		previousCamPosition = cam.position;

	}
}
