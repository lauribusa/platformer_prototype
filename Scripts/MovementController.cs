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

	float verticalRaySpacing;
    // Start is called before the first frame update
    void Start()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		verticalRaySpacing = boxCollider.bounds.size.y / (verticalRayCount - 1);
    }

	void ComputeBounds()
	{
		bottomLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y);
		bottomRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y);
		topLeft = new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y);
		topRight = new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y);
	}
    // Update is called once per frame

	public void Move(Vector2 velocity)
	{
		ComputeBounds();
		HorizontalMove(ref velocity);
		transform.Translate(velocity);
	}

	public void HorizontalMove(ref Vector2 velocity)
	{
		float direction = Mathf.Sin(velocity.x);
		float distance = Mathf.Abs(velocity.x);
			for (int i = 0; i < verticalRayCount; i++)
			{
			Vector2 baseOrigin = direction == 1 ? bottomRight : bottomLeft; 
				Vector2 origin = bottomRight + new Vector2(0, verticalRaySpacing * i);
				Debug.DrawLine(origin, origin + new Vector2(velocity.x, 0));
				RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, velocity.x, _layerMask);
				if (hit)
				{
				velocity.x = hit.distance * direction;
					Debug.Log(hit.point);
				}
			}
	}

}
