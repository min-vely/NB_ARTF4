using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameSceneUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI deathCountText;
    [SerializeField] private TextMeshProUGUI timeText;

    private int deathCount = 1;
    private float time = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateDeathCountUI();
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString("N2");
    }

    public void IncreseDeathCount()
    {
        deathCount++;
        UpdateDeathCountUI();
    }

    private void UpdateDeathCountUI()
    {
        deathCountText.text = "Try : " + deathCount + "회";
    }
}
