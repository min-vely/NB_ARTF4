using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePos : MonoBehaviour
{
    public bool OnCheckPoint;
	public Transform checkPoint;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerController>().checkPoint = checkPoint.position;

            OnCheckPoint = true;
		}

        else OnCheckPoint = false;
	}
}
