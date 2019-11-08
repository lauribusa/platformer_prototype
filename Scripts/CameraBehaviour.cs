using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
	Player player;

	public Vector2 maxStageLimit;
	public Vector2 minStageLimit;

	public float xDeadZone;
	public float yDeadZone;

	float height;
	float width;
	Vector3 velocity;
	Camera cam;
	Scene scene;

	float minXLimit;
	float maxXLimit;
	float minYLimit;
	float maxYLimit;

	CameraLimit cameraLimit;

	struct CameraLimit
	{
		public float left, right, up, down;
	}

	// Start is called before the first frame update
	void Start()
	{
		cam = GetComponent<Camera>();
		scene = FindObjectOfType<Scene>();

		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;

		DefineLimits();
		cameraLimit.left = minStageLimit.x + width / 2;
		cameraLimit.right = maxStageLimit.x - width / 2;
		cameraLimit.down = minStageLimit.y + height / 2;
		cameraLimit.up = maxStageLimit.y - height / 2;

		StartCoroutine(FindPlayerCoroutine());
		StartCoroutine(FollowPlayer());
	}

	void DefineLimits()
	{
		minXLimit = scene.bottomLeftLimit.transform.position.x;
		maxXLimit = scene.topRightLimit.transform.position.x;
		minYLimit = scene.bottomLeftLimit.transform.position.y;
		maxYLimit = scene.topRightLimit.transform.position.y;
	}
	private void LateUpdate()
	{
		
	}
	IEnumerator FindPlayerCoroutine()
	{
		while (true)
		{
			if (player == null)
				player = FindObjectOfType<Player>();

			yield return null;
		}
	}

	IEnumerator FollowPlayer()
	{
		// XXX use lateupdate
		while (true)
		{
			if (player != null)
			{
				
					MoveToPlayer();
			}

			yield return null;
		}
	}

	void DebugDrawRect(Rect rect, Color color)
	{
		// Bot
		Debug.DrawLine(new Vector3(rect.xMin, rect.yMin, 0), new Vector3(rect.xMax, rect.yMin, 0), color);
		// Top
		Debug.DrawLine(new Vector3(rect.xMin, rect.yMax, 0), new Vector3(rect.xMax, rect.yMax, 0), color);
		// Left
		Debug.DrawLine(new Vector3(rect.xMin, rect.yMin, 0), new Vector3(rect.xMin, rect.yMax, 0), color);
		// Right
		Debug.DrawLine(new Vector3(rect.xMax, rect.yMin, 0), new Vector3(rect.xMax, rect.yMax, 0), color);
	}

	void MoveToPlayer()
	{

		Vector3 target = new Vector3(
			Mathf.Clamp(player.transform.position.x, minXLimit, maxXLimit),
			Mathf.Clamp(player.transform.position.y, minYLimit, maxYLimit),
			transform.position.z);
		// Do not go out of bounds
		transform.position = target; //Vector3.SmoothDamp(transform.position, target, ref velocity, 1f);

	}
}
