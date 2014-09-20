using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	public string SkillUsed;
	public bool punch;
	public bool frontal;
	private Animator animator;
	private float blocktimer = 0.5f;
	float skilltimer;
	private float blockduration = 0.5f;
	public bool isTopBlocking = false;
	private KeyCombo falconPunch= new KeyCombo(new string[] {"up", "down"});
	private KeyCombo falconKick= new KeyCombo(new string[] {"down", "right"});
	private KeyCombo feetthrust = new KeyCombo (new string[] {"Fire1", "down"});
	private KeyCombo topblock = new KeyCombo (new string[] {"BackHigh"});
	void Awake (){
		animator = GetComponent<Animator>();

	}
	void Update () {
		if (skilltimer > 0)
						skilltimer -= Time.deltaTime;
		if (skilltimer < 0)
						skilltimer = 0;
		if (skilltimer == 0) {
		SkillUsed = "none";
		}


		if (blocktimer> 0)
			blocktimer -= Time.deltaTime;
		if (blocktimer < 0)
			blocktimer = 0;
		if (blocktimer == 0) {

			isTopBlocking = false;
			blocktimer = blockduration;
				}

		if (feetthrust.Check ()) {

			SkillUsed = "feetthrust";
			skilltimer = 0.15f;
		}

		if (falconPunch.Check())
		{SkillUsed = "punch";
			skilltimer = 0.15f;
		}		
		if (falconKick.Check())
		{SkillUsed = "frontal";
			skilltimer = 0.15f;
		}
		if (topblock.Check())
		{
			topBlock();
			}
	}

void	topBlock(){
		//Debug.Log ("Blocked high attack");
		animator.SetTrigger("topblock");
		isTopBlocking = true;

		//i'll make it so that you block attacks from enemies with y > your y or with top attack attribute,idk 

	}
}