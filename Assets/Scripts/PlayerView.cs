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
	public AudioPlayer splat;

	Vector3 currentLookDirection = Vector3.forward;
	bool crouching = false;
	float crouchTarget = 0.0f;

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
		bool jumpInput = Input.GetButtonDown("Jump") || verticalAxis > 0.5f;
		bool crouchInput = verticalAxis < -0.2f;

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

		if (onGround && jumpInput && rigidbody.velocity.y == 0)
		{
			rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			
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

		//Crouching
		if (!crouching && onGround && crouchInput)
		{
			crouching = true;
			crouchTarget = 1.0f;

			capsule.offset = new Vector2(0, -capsuleColliderStartSize.y * 0.25f);
			capsule.size = new Vector2(capsuleColliderStartSize.x, capsuleColliderStartSize.y * 0.5f);

			box.offset = new Vector2(0, -boxColliderStartSize.y * 0.25f);
			box.size = new Vector2(boxColliderStartSize.x, boxColliderStartSize.y * 0.5f);
		}

			var groundHitUp = Physics2D.CircleCast(transform.position + (Vector3)capsule.offset * 2, capsule.size.x * 0.49f, Vector3.up, capsule.size.y * 1.5f, LayerMasks.groundCheck);
			
			#if UNITY_EDITOR
		Debug.DrawRay(transform.position + (Vector3)capsule.offset* 2, Vector3.up * capsule.size.y* 1.5f, Color.green, 2f);
		Debug.DrawRay(transform.position + (Vector3)capsule.offset* 2 + Vector3.up * capsule.size.y* 1.5f, Vector2.up * capsule.size.x * 0.49f, (groundHitUp ? Color.red : Color.blue), 2f);
#endif
			
		if (crouching && (verticalAxis >= 0 || !onGround))
		{
			if (!groundHitUp)
			{
				crouchTarget = 0.0f;
				crouching = false;

				capsule.offset = Vector2.zero;
				capsule.size = capsuleColliderStartSize;

				box.offset = Vector2.zero;
				box.size = boxColliderStartSize;
			}
		}

		animator.SetFloat("Crouch", crouchTarget, 0.2f, Time.deltaTime);

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
			splat.Play2DSound();
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