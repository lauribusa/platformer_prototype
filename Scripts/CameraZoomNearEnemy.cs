using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomNearEnemy : MonoBehaviour
{
	public float circleRadius;
	Camera cam;
	LayerMask enemyLayer;
   
    // Update is called once per frame
    void Update()
    {
		Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll(transform.position, circleRadius, enemyLayer);
    }
}
