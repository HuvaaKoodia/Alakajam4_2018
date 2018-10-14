using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
	#region variables
	public new Rigidbody2D rigidbody;

	public float jumpForce = 5f, moveForce = 5f, crouchingForce = 5f;

	public PhysicsMaterial2D jumpFriction, groundFriction;
	public Animator animator;
	public Transform rotationParent;
	public float rotationSpeed = 1f;
	public bool onGround = false;
	public float groundCheckDistance = 2f;
	public BoxCollider2D box;
	public CapsuleCollider2D capsule;
	public bool dead = false;
	public GameObject graphicsParent;
	public Squish squish;
	Vector2 capsuleColliderStartSize, boxColliderStartSize;

	Vector3 currentLookDirection = Vector3.forward;
	bool crouching = false;

	#endregion
	#region initialization
	void Start()
	{
		boxColliderStartSize = box.size;
		capsuleColliderStartSize = capsule.size;
	}
	#endregion
	#region logic
	void Update()
	{
		if (dead)return;

		float horizontalAxis = Input.GetAxisRaw("Horizontal");
		float verticalAxis = Input.GetAxisRaw("Vertical");
		bool jumped = Input.GetButtonDown("Jump");

		var groundHit = Physics2D.CircleCast(transform.position, capsule.size.x * 0.5f * 0.95f, Vector2.down, groundCheckDistance, LayerMasks.groundCheck);

#if UNITY_EDITOR
		Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
		Debug.DrawRay(transform.position + Vector3.down * groundCheckDistance, Vector2.down * capsule.bounds.extents.x, Color.magenta);
#endif

		if (groundHit)
		{
			if (!onGround && rigidbody.velocity.y <= 0)
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

		v.x = horizontalAxis * (crouching ? crouchingForce : moveForce);

		if (animator)
			animator.SetBool("walkbool", horizontalAxis != 0);

		rigidbody.velocity = v;

		if (horizontalAxis != 0)
		{
			currentLookDirection = Vector3.forward * horizontalAxis;
		}

		Debug.Log("VAxis: " + verticalAxis);

		if (!crouching && onGround && verticalAxis < 0)
		{
			crouching = true;
			graphicsParent.transform.localScale = new Vector3(1, 0.5f);

			capsule.offset = new Vector2(0, -capsuleColliderStartSize.y * 0.25f);
			capsule.size = new Vector2(capsuleColliderStartSize.x, capsuleColliderStartSize.y * 0.5f);

			box.offset = new Vector2(0, -boxColliderStartSize.y * 0.25f);
			box.size = new Vector2(boxColliderStartSize.x, boxColliderStartSize.y * 0.5f);
		}

		if (crouching && (verticalAxis >= 0 || !onGround))
		{
			var groundHitUp = Physics2D.CircleCast(transform.position + (Vector3)capsule.offset, capsule.size.x * 0.9f, Vector2.up, capsule.size.y, LayerMasks.groundCheck);

			if (!groundHitUp)
			{
				crouching = false;
				graphicsParent.transform.localScale = Vector3.one;

				capsule.offset = Vector2.zero;
				capsule.size = capsuleColliderStartSize;

				box.offset = Vector2.zero;
				box.size = boxColliderStartSize;
			}
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