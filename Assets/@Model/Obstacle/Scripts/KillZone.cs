using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Scripts.Scene;

public class KillZone : MonoBehaviour
{
    private GameSceneUI gameSceneUI;

    private void Start()
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
			//col.gameObject.GetComponent<CharacterControls>().LoadCheckPoint();
            col.gameObject.GetComponent<PlayerController>().LoadCheckPoint();
            gameSceneUI.IncreseDeathCount();
        }
	}
}
