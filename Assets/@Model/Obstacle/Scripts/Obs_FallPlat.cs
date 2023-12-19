using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FallPlat : MonoBehaviour
{
	public float fallTime = 0.5f;

    private void Start()
    {
        Main.Obstacle.OnInitializedObstacle += InitializedObstacle;
    }

    private void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.CompareTag("Player"))
			{
				StartCoroutine(Fall(fallTime));
			}
		}
	}

    private IEnumerator Fall(float time)
	{
		yield return new WaitForSeconds(time);
		gameObject.SetActive(false);
	}

    private void InitializedObstacle()
    {
        gameObject.SetActive(true);
    }
}
