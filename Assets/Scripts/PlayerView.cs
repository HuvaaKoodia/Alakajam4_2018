using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
	#region variables
	public new Rigidbody2D rigidbody;

	public float jumpForce = 5f, moveForce = 5f;

	public PhysicsMaterial2D jumpFriction, groundFriction;
	public Animator animator;
	public Transform rotationParent;
	public float rotationSpeed = 1f;
	public bool onGround = false;
	public float groundCheckDistance = 2f;
	public Collider2D box, capsule;
	public bool dead = false;
	public GameObject graphicsParent;
	public Squish squish;

	Vector3 currentLookDirection = Vector3.forward;

	#endregion
	#region initialization
	#endregion
	#region logic
	void Update()
	{
		if (dead) return;

		float horizontalAxis = Input.GetAxisRaw("Horizontal");
		bool jumped = Input.GetButtonDown("Jump");

		var groundHit = Physics2D.CircleCast(transform.position, capsule.bounds.extents.x, Vector2.down, groundCheckDistance, LayerMasks.groundCheck);

#if UNITY_EDITOR
		Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
		Debug.DrawRay(transform.position + Vector3.down * groundCheckDistance, Vector2.down * capsule.bounds.extents.x, Color.magenta);
#endif

		if (groundHit)
		{
			if (!onGround && rigidbody.velocity.y < 0)
			{
				onGround = true;
				animator.SetBool("ilmabool", false);
				box.enabled = true;
				squish.SquishIt();
			}
			//rigidbody.sharedMaterial = groundFriction;
		}
		else if (onGround)
		{
			onGround = false;
			animator.SetBool("ilmabool", true);
			box.enabled = false;
		}

		if (onGround && jumped)
		{
			rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			//rigidbody.sharedMaterial = jumpFriction;
		}
		var v = rigidbody.velocity;

		v.x = horizontalAxis * moveForce;

		if (animator)
			animator.SetBool("walkbool", horizontalAxis != 0);

		rigidbody.velocity = v;

		if (horizontalAxis != 0)
		{
			currentLookDirection = Vector3.forward * horizontalAxis;
		}

		rotationParent.localRotation = Quaternion.Lerp(rotationParent.localRotation, Quaternion.LookRotation(currentLookDirection, Vector3.up), rotationSpeed * Time.deltaTime);
	}

	public ParticleSystem blood;
	public Scale crushScale;

	public void CheckCrushingDeath()
	{
		dead = true;
		box.enabled = false;
		capsule.enabled = false;

		//if (onGround)
		{
			crushScale.Shrink();

			animator.SetTrigger("kuolematrig");
		}
		// else
		// {
		// 	//graphicsParent.gameObject.SetActive(false);	

		// }
		
		rigidbody.simulated = false;
	}
	#endregion
	#region public interface
	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}