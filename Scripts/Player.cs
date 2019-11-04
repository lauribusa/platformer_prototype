using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{
	[Tooltip("Alors, toujours pédé?")]
	public int _speed = 1;
	Vector2 _velocity;
	MovementController mc;

    // Start is called before the first frame update
    void Start()
    {
		mc = GetComponent<MovementController>();

	}

    // Update is called once per frame
    void Update()
    {
		int horizontal = 0;
		int vertical = 0;
		if (Input.GetKey(KeyCode.Q))
		{
			horizontal += -1; 
		}
		if (Input.GetKey(KeyCode.D))
		{
			horizontal += 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			vertical += -1;
		}
		if (Input.GetKey(KeyCode.Z))
		{
			vertical += 1;
		}

		_velocity = new Vector2(horizontal * _speed, vertical * _speed);

		mc.Move(_velocity * Time.deltaTime);
	}
}
