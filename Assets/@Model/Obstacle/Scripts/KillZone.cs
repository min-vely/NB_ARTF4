using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			//col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
            col.gameObject.GetComponent<PlayerController>().LoadCheckPoint();
        }
	}
}
