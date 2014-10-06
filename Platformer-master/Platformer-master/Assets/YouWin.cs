using UnityEngine;
using System.Collections;

public class YouWin : MonoBehaviour {
	bool winwin = false;
	public void	OnTriggerEnter2D(Collider2D other) {
		GameObject target = other.gameObject;
		if (target.CompareTag ("Player")) {
			winwin = true;
	}
	}
	void OnGUI (){
	if (winwin)
		GUI.Box(new Rect(300, 100, 150, 30), "You WIN");
	}}

