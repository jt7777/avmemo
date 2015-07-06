using UnityEngine;
using System.Collections;

public class RandomState : StateMachineBehaviour {

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		animator.SetInteger("RandomSelector", Random.Range(0,2));
		Debug.Log ("anim: " + animator.GetInteger ("RandomSelector"));
	}
}
