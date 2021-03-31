using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_reload : StateMachineBehaviour
{
		//OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			player player_scr = animator.gameObject.GetComponent<player>();
			player_scr.can_aim = false;
			player_scr.lr.enabled = false;
			player_scr.lr.enabled = false;
			if (player_scr.dual_wield == true) {
				player_scr.dual_barrel.GetComponent<dual_lazer>().lr.enabled = false;
			}
		}


		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		//{
		//
		//}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		// override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		// {
		//
		// }
		override public void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
			player player_scr = animator.gameObject.GetComponent<player>();
			player_scr.can_aim = true;
			player_scr.lr.enabled = true;
			player_scr.lr.enabled = true;
			if (player_scr.dual_wield == true) {
				player_scr.dual_barrel.GetComponent<dual_lazer>().lr.enabled = true;
			}
			gun_scr gun_scr = player_scr.gun_scr;
			gun_scr.reload_cooldown = -1;
			gun_scr.reload = false;
			gun_scr.reload_cooldown = gun_scr.reload_timer;
			gun_scr.clip = gun_scr.max_clip;
			gun_scr.ammo -= gun_scr.clip;
			if (gun_scr.ammo_text != null) {
					gun_scr.ammo_text.set_ammo(gun_scr.clip, gun_scr.ammo, gun_scr.weapon);
			}
			animator.SetBool("is reload", false);
		}

		// OnStateMove is called right after Animator.OnAnimatorMove()
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		//{
		//    // Implement code that processes and affects root motion
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK()
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		//{
		//    // Implement code that sets up animation IK (inverse kinematics)
		//}
}
