using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{
	[Tooltip("Alors, toujours pédé?")]
	public int _speed = 1;
	[Tooltip("Max jump height")]
	public float jumpHeight;
	[Tooltip("Time to achieve peak")]
	public float timeToMaxJump;
	[Tooltip("Activate air control")]
	public bool _airControlActivated;
	[Tooltip("Degree of horizontal control of airborne character")]
	public float _airControl;
	public bool accelerationMode;
	public float acceleration;
	[Range(0,50)]
	public float brakeForce;
	
	public float maxSpeed;

	float gravity;
	float minSpeedThreshold;
	float _jumpForce;

	Vector2 _velocity;
	MovementController mc;

	public int jumpCount = 1;
	int availableAirJumps;

    // Start is called before the first frame update
    void Start()
    {
		availableAirJumps = jumpCount;
		if(brakeForce >= 0)
		{
			brakeForce = acceleration;
		}
		minSpeedThreshold = acceleration * Application.targetFrameRate * 2f;
		
		mc = GetComponent<MovementController>();
		// Math calculation for gravity and jumpForce
		gravity = -(2f * jumpHeight) / Mathf.Pow(timeToMaxJump, 2);
		_jumpForce = Mathf.Abs(gravity) * timeToMaxJump;
	}

    // Update is called once per frame
    void Update()
    {
		if (mc._collisions.bottom)
		{
			_velocity.y = 0;
			availableAirJumps = jumpCount;
		}
		if (mc._collisions.top)
		{
			_velocity.y = 0;
		}
		AirJumpUpdate();
		int horizontal = 0;
		if (mc._collisions.bottom || _airControlActivated)
		{
			horizontal = 0;

			if (Input.GetKey(KeyCode.Q))
			{
				horizontal += -1;
			}

			if (Input.GetKey(KeyCode.D))
			{
				horizontal += 1;
			}
		}
		if (accelerationMode)
		{
			_velocity.x += horizontal * acceleration * Time.deltaTime;

			if (Mathf.Abs(_velocity.x) > maxSpeed)
			{
				_velocity.x = maxSpeed * horizontal;
			}

			/*if (_velocity.x > maxSpeed)
			{
				_velocity.x = maxSpeed;

			}
			if (_velocity.x < -maxSpeed)
			{
				_velocity.x = -maxSpeed;

			}*/

			if (horizontal == 0)
			{
				
				if (_velocity.x > minSpeedThreshold)
				{
					_velocity.x -= brakeForce * Time.deltaTime;

				}
				else if (_velocity.x < -minSpeedThreshold)
				{
					_velocity.x += acceleration * Time.deltaTime;

				}
				else
				{
					_velocity.x = 0;

				}
			}
			
		} else
		{
			_velocity.x = horizontal * _speed;

		}
		
		
		
		/*if (Input.GetKey(KeyCode.Z))
		{
			vertical = 1;
		}*/
	
		
		_velocity.y += gravity * Time.deltaTime;

		mc.Move(_velocity * Time.deltaTime);
	}
	void Jump()
	{ 
		_velocity.y = _jumpForce;
	}

	void AirJumpUpdate()
	{
		if (mc._collisions.bottom)
		{
			availableAirJumps = jumpCount;
		}
		if (Input.GetKeyDown(KeyCode.Space) && availableAirJumps > 0)
		{
			Jump();
			availableAirJumps--;
		}
	}
}
