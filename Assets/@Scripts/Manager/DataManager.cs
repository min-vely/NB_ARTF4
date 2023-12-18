using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public enum DATANAME
    {
        gameOverScript,
        gameLoadingScript1,
        gameLoadingScript2
    }
    [SerializeField] private TextAsset csvFile;
    CSVLoader csv = new CSVLoader();
    JsonLoader json = new JsonLoader();


    void Start()
    {
        UpdateVersion();

        Dictionary<int, string> data = json.JsonDataLoad(DATANAME.gameLoadingScript1.ToString());
        for (int i = 0; i < data.Count; i++)
        {
            Debug.Log(data[i]);
        }
    }
    /// <summary>
    /// CSV파일 상단 버전이 바뀌어 있으면 Json파일을 업데이트
    /// </summary>
    void UpdateVersion()
    {
        float version = json.JsonVersionLoad();
        CSVData updateDate = csv.LordFile(csvFile, version);
        if (updateDate != null) json.JsonLoad(updateDate);
    }
}
