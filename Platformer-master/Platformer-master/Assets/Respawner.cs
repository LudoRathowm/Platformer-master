using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour {
	Vector2 _playerSpawnPointPos;
	public GameObject Player;
	GameObject _pc;
	int numberofdeaths = 0;
	// Use this for initialization
	void Start () {
		_playerSpawnPointPos = new Vector2 (-13.92f, -14.69f);
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (!player){

			_pc = Instantiate (Player, _playerSpawnPointPos, Quaternion.identity) as GameObject;
			numberofdeaths++;

		}
	if (numberofdeaths == 5)
			Application.LoadLevel(Application.loadedLevelName);}
	void OnGUI (){
		GUI.Box(new Rect(30, 10, 150, 30), "You died " + numberofdeaths + " times");
	}
}
