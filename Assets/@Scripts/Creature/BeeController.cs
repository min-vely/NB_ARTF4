using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    private Dictionary<int, string> beeText;
    [SerializeField] GameObject textBox;
    private TMP_Text text;
    private void Awake()
    {
        text = textBox.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        beeText = Main.Data.JsonDataLoad(DataManager.DATANAME.gameOverScript);
        KillZone.OnDeath += BeeMove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeeMove()
    {
        RotateBee(new Vector3(0, 180, 0), 0.5f);

        textBox.SetActive(true);
        int randomText = new System.Random().Next(0, 5);
        text.text = beeText[randomText];

        Invoke("ReRotateBee",1f);
    }
   
    private void ReRotateBee()
    {
        RotateBee(new Vector3(0, 180, 0), 0.5f);
        textBox.SetActive(false);
    }
    private void RotateBee(Vector3 byAngles, float inTime)
    {
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        transform.DORotateQuaternion(toAngle, inTime);
    }
}
