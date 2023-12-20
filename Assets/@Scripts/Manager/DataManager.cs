using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// SCV파일에 들어있는 값을 담을 Dictionary을 모아둔 클래스
/// </summary>
public class CSVData
{
    public Dictionary<int, string> gameOverScript = new Dictionary<int, string> { };
    public Dictionary<int, string> gameLoadingScript1 = new Dictionary<int, string> { };
    public Dictionary<int, string> gameLoadingScript2 = new Dictionary<int, string> { };
    //CSV파일에 덩어리 하나 추가시 여기에 Dictionary 추가 및 RowCheck()에 해당 덩어리 제목으로 스위치 문 하나 추가하면 됩니다.
}
public class ItemData
{
    public string id = null;
    public string name = null;
    public string category = null;
    public string description = null;
    public float duration = 0;
    public float power = 0;
}
public class ItemDataContainer
{
    public Dictionary<int, ItemData> Items = new Dictionary<int, ItemData> { };
}
public class Vector3Data
{
    public float x;
    public float y;
    public float z;

    public Vector3Data(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
}
public class Vector3DataContainer
{
    public Dictionary<int, Vector3Data> ItemVectorDate = new Dictionary<int, Vector3Data> { };
}
public class DataManager : MonoBehaviour
{
    public enum DATANAME
    {
        gameOverScript,
        gameLoadingScript1,
        gameLoadingScript2
    }
    CSVLoader csv = new CSVLoader();
    JsonLoader json = new JsonLoader();


    void Start()
    {
        
    }
    /// <summary>
    /// CSV파일 상단 버전이 바뀌어 있으면 Json파일을 업데이트
    /// </summary>
    public void UpdateVersion()
    {
        var updateUIDate = csv.LordUIFile();
        var updateItemDate = csv.LordItemFile();
        var itemVertorDate = csv.LordItemVectorFile();
        if (updateUIDate != null) json.JsonLoad(updateUIDate);
        if (updateItemDate != null) json.JsonLoad(updateItemDate);
        if (itemVertorDate != null) json.JsonLoad(itemVertorDate);
    }

    public ItemDataContainer itemLoader()
    {
        return json.JsonItemLoad();
    }
    public ItemData itemLoader(int ItemNo)
    {
        return json.JsonItemLoad(ItemNo);
    }
    public void SetCheckPoint(Vector3 point)
    {
        json.SetCheckPoint(new Vector3Data(point));
    }
    public Vector3 GetCheckPoint()
    {
        return json.GetCheckPoint();
    }
    public Dictionary<int, string> JsonDataLoad(DATANAME dataName)
    {
        return json.JsonDataLoad(dataName);
    }
    public Vector3DataContainer ItemVectorDate()
    {
        return json.JsonItemVectorLoad();
    }
}
