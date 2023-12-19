using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static DataManager;


public class JsonLoader
{
    // json 로드를 위한 파일패스
    static string uiFilePath = Path.Combine(Application.dataPath, "@Resources/@Json/GameUIScript.json");
    static string itemFilePath = Path.Combine(Application.dataPath, "@Resources/@Json/GameItemData.json");
    static string savePointFilePath = Path.Combine(Application.dataPath, "@Resources/@Json/SavePoint.json");
    /// <summary>
    /// 파일 패스를 가지고 제이슨 파일을 읽어오는 메서드
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private string ReadJsonFile(string filePath)
    {
        try
        {
            //파일 패스를 가지고 값을 가져오는데 성공하면 json의 모든 값을 string으로 파싱
            return File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            //실패하면 버그 메시지 출력 후 null리턴
            Debug.LogError("Error reading JSON file: " + e.Message);
            return null;
        }
    }
  
    
    /// <summary>
    /// 데이터가 빈값이 아니라면 가지고 온 데이터를 가지고 현재 json 파일을 새로 쓰는 메서드
    /// </summary>
    /// <param name="updateDate"></param>
    internal void JsonLoad(CSVData updateUIDate)
    {
        // 받은 CSVData 파일을 json파일로 직렬화
        string json = JsonConvert.SerializeObject(updateUIDate, Formatting.Indented);

        using (StreamWriter file = File.CreateText(uiFilePath))
        {
            //StreamWriter을 사용하여 json 작성
            file.Write(json);
        }
    }
    internal void JsonLoad(ItemDataContainer updateItemDate)
    {
        string json = JsonConvert.SerializeObject(updateItemDate, Formatting.Indented);

        using (StreamWriter file = File.CreateText(itemFilePath))
        {
            //StreamWriter을 사용하여 json 작성
            file.Write(json);
        }
    }


    /// <summary>
    /// json파일에 키값을 가지고 해당 안에 있는 값을 Dictionary<int, string> 타입으로 가져오는 메서드 없으면 null을 반환
    /// </summary>
    /// <param name="dataName"></param>
    /// <returns></returns>
    internal Dictionary<int, string> JsonDataLoad(DataManager.DATANAME dataName)
    {
        // 값을 받을 Dictionary 생성
        Dictionary<int, string> result = null;

        // json파일 로드
        string jsonContent = ReadJsonFile(uiFilePath);

        // 빈값이 아니라면 실행
        if (!string.IsNullOrEmpty(jsonContent))
        {
            // JObject로 모든 데이터를 변환
            JObject jsonObject = JObject.Parse(jsonContent);

            // 받아온 값을 이용하여 해당 값에 일치하는 데이터를 JObject로 가져옴
            JObject gameLoadingScript1Object = (JObject)jsonObject[dataName.ToString()];

            // gameLoadingScript1Object를 Dictionary<int, string>으로 파싱후 result에 할당
            result = gameLoadingScript1Object.ToObject<Dictionary<int, string>>();

        } 
        // 없으면 null 있으면 Dictionary를 반환
        return result;
    }

    internal ItemDataContainer JsonItemLoad()
    {
        ItemDataContainer result = null;
        string jsonContent = ReadJsonFile(itemFilePath);
        if (!string.IsNullOrEmpty(jsonContent))
        {
            // JObject로 모든 데이터를 변환
            JObject jsonObject = JObject.Parse(jsonContent);

            // gameLoadingScript1Object를 Dictionary<int, string>으로 파싱후 result에 할당
            result = jsonObject.ToObject<ItemDataContainer>();

        } 
        return result;
    }

    internal ItemData JsonItemLoad(int itemNo)
    {
        // 값을 받을 Dictionary 생성
        ItemData result = new ItemData();

        // json파일 로드
        string jsonContent = ReadJsonFile(itemFilePath);

        // 빈값이 아니라면 실행
        if (!string.IsNullOrEmpty(jsonContent))
        {
            // JObject로 모든 데이터를 변환
            JObject jsonObject = JObject.Parse(jsonContent);

            // 받아온 값을 이용하여 해당 값에 일치하는 데이터를 JObject로 가져옴
            JObject jsonObjectItem = (JObject)jsonObject["Items"][itemNo.ToString()];
            result.id = jsonObjectItem["id"].Value<int>();
            result.name = jsonObjectItem["name"].Value<string>();
            result.category = jsonObjectItem["category"].Value<string>();
            result.description = jsonObjectItem["description"].Value<string>();
            result.duration = jsonObjectItem["duration"].Value<int>();
            result.power = jsonObjectItem["power"].Value<float>();
        }
        // 없으면 null 있으면 Dictionary를 반환
        return result;
    }

    internal void SetCheckPoint(Vector3Data point)
    {
        string json = JsonConvert.SerializeObject(point);

        using (StreamWriter file = File.CreateText(savePointFilePath))
        {
            //StreamWriter을 사용하여 json 작성
            file.Write(json);
        }
    }
    internal Vector3 GetCheckPoint()
    {
        string jsonContent = ReadJsonFile(savePointFilePath);
        return JsonConvert.DeserializeObject<Vector3>(jsonContent);
    }
}
