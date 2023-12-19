using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlat : MonoBehaviour
{
	public float fallTime = 0.5f;
    

    void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			//Debug.DrawRay(contact.point, contact.normal, Color.white);
			if (collision.gameObject.tag == "Player")
			{
				StartCoroutine(Fall(fallTime));
			}
		}
	}

    private void Update()
    {
        if (Main.SavePos.OnCheckPoint)
        {
            Debug.Log(Main.SavePos.OnCheckPoint);
            transform.gameObject.SetActive(true);
        }
    }

    IEnumerator Fall(float time)
	{
		yield return new WaitForSeconds(time);
		transform.gameObject.SetActive(false);
	}
}
