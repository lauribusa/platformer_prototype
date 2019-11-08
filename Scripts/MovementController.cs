using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
	BoxCollider2D boxCollider;
	Vector2 bottomLeft, bottomRight, topLeft, topRight;

	public int horizontalRayCount;
	public int verticalRayCount;

	public LayerMask _layerMask;
	public LayerMask layerOneWayPlatform;
	public Collisions _collisions;

	public float _pixelPerUnit = 55f;
	float skinWidth;
	float pitDistance;

	float verticalRaySpacing;
	float horizontalRaySpacing;

	public struct Collisions
	{
		public bool top, bottom, left, right;

		public bool frontPit;
		public void Reset()
		{
			top = bottom = left = right = false;
			frontPit = false;
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		skinWidth = 1 / 55f;
		pitDistance = 0.5f;
		boxCollider = GetComponent<BoxCollider2D>();
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2f);
		verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
		horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
	}

	void ComputeBounds()
	{
		Bounds bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2f);
		bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		topLeft = new Vector2(bounds.min.x, bounds.max.y);
		topRight = new Vector2(bounds.max.x, bounds.max.y);
	}
	// Update is called once per frame

	public void Move(Vector2 velocity)
	{
		_collisions.Reset();
		ComputeBounds();
		if(velocity.x != 0)
		{
			HorizontalMove(ref velocity);

		}
		if(velocity.y != 0)
		{
			VerticalMove(ref velocity);
		}
		DetectFrontPit(velocity);
		transform.Translate(velocity);
	}
	public void VerticalMove(ref Vector2 velocity)
	{
		float direction = Mathf.Sign(velocity.y);
		float distance = Mathf.Abs(velocity.y)+skinWidth;

		Vector2 baseOrigin = direction == 1 ? topLeft : bottomLeft;
		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 origin = baseOrigin + new Vector2(horizontalRaySpacing * i, 0);
			Debug.DrawLine(origin, origin + new Vector2(0, direction * distance));
			
			RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(0, direction), distance, _layerMask);
			if (hit)
			{

				/*print(hit.transform.gameObject.layer);
				print(layerOneWayPlatform);*/
				print("touched: "+hit.transform.gameObject.tag);
				if(!(hit.transform.gameObject.tag == "oneWayPlatform" && direction > 0))
				{
					velocity.y = (hit.distance - skinWidth) * direction;
					distance = hit.distance - skinWidth;
					
					if (direction < 0)
					{
						_collisions.bottom = true;
					}
					if (direction > 0)
					{
						_collisions.top = true;
					}
				}
				
			}
		}
	}
	public void HorizontalMove(ref Vector2 velocity)
	{
		float direction = Mathf.Sign(velocity.x);
		float distance = Mathf.Abs(velocity.x)+skinWidth;
		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 baseOrigin = direction == 1 ? bottomRight : bottomLeft;
			Vector2 origin = baseOrigin + new Vector2(0, verticalRaySpacing * i);
			Debug.DrawLine(origin, origin + new Vector2(direction * distance, 0));
			RaycastHit2D hit = Physics2D.Raycast(origin, new Vector2(direction, 0), distance, _layerMask);
			if (hit)
			{
				velocity.x = (hit.distance - skinWidth) * direction;
				if(direction < 0)
				{
					_collisions.left = true;
				} else
				if(direction > 0)
				{
					_collisions.right = true;
				}
				
			}
		}
	}
	void DetectFrontPit(Vector2 velocity)
	{
		Vector2 origin = velocity.x > 0 ? bottomRight : bottomLeft;

		//Debug.DrawLine(origin, origin + Vector2.down * pitDistance);
		RaycastHit2D hit = Physics2D.Raycast(
			origin,
			Vector2.down,
			pitDistance,
			_layerMask
			);

		if (!hit)
		{
			_collisions.frontPit = true;
		}
	}
}
