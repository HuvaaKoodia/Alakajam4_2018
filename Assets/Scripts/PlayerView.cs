using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
	#region variables
	public new Rigidbody2D rigidbody;

	public float jumpForce = 5f, moveForce = 5f;

	public PhysicsMaterial2D jumpFriction, groundFriction;

	public bool onGround = false;
	public float groundCheckDistance = 2f;
	#endregion
	#region initialization
	void Start()
	{

	}
	#endregion
	#region logic
	void Update()
	{
		float horizontalAxis = Input.GetAxisRaw("Horizontal");
		bool jumped = Input.GetButtonDown("Jump");

		if (!onGround && rigidbody.velocity.y <= 0)
		{
			var hitR = Physics2D.Raycast(transform.position + Vector3.right * 0.9f, Vector2.down, groundCheckDistance, LayerMasks.groundCheck);
			
			var hitL = Physics2D.Raycast(transform.position + Vector3.left * 0.9f, Vector2.down, groundCheckDistance, LayerMasks.groundCheck);
			

#if UNITY_EDITOR
			Debug.DrawRay(transform.position, Vector2.down, Color.red, groundCheckDistance);
#endif

			if (hitR || hitL)
			{
				onGround = true;
				//rigidbody.sharedMaterial = groundFriction;
			}
		}

		if (onGround && jumped)
		{
			rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
			rigidbody.sharedMaterial = jumpFriction;
		}
		var v = rigidbody.velocity;

		v.x = horizontalAxis * moveForce * Time.deltaTime;

		rigidbody.velocity = v;
		rigidbody.AddForce(Vector2.right , ForceMode2D.Force);
	}
	#endregion
	#region public interface
	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}