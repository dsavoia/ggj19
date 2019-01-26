using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public enum MovementAnimationEnum
	{
		stand = 0,
		walk = 1,
		jump = 2,
		wallSlide = 3,
	}

	MovementAnimationEnum animationState = MovementAnimationEnum.stand;

	Animator anim;
	Player player;
	Vector3 playerScale;
	bool facingRight = true;

	void Start () 
	{
		anim = GetComponent<Animator> ();
		player = GetComponentInParent<Player> ();
	}

	void Update()
	{
		UpdateMovementAnimation();
	}

	void UpdateMovementAnimation () 
	{
		animationState = MovementAnimationEnum.stand;
		playerScale = transform.localScale;

		if(player.controller.collisions.faceDir > 0)
		{
			if(!facingRight)
			{
				playerScale.x = -transform.localScale.x;
				facingRight = true;
			}
		}
		else
		{
			if(facingRight)
			{
				playerScale.x = -transform.localScale.x;
				facingRight = false;
			}
		}

		transform.localScale = playerScale;
		//print (player.velocity.x);

		if(player.controller.playerInput.x != 0)
		{
			animationState = MovementAnimationEnum.walk;
		}

		if(player.velocity.y != 0)
		{
			animationState = MovementAnimationEnum.jump;
		}

		if(player.wallSliding)
		{
			animationState = MovementAnimationEnum.wallSlide;
		}

		anim.SetInteger ("playerState", (int)animationState);
	}
}
