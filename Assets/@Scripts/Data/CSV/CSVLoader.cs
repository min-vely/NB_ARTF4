using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class CSVLoader
{
    //static string uiFilePath = Application.dataPath + "/@Resources/@CSV/GameUIScript.csv";
    //static string itemFilePath = Application.dataPath + "/@Resources/@CSV/GameItemData.csv";
    //static string itemVectorFilePath = Application.dataPath + "/@Resources/@CSV/GameItemPosition.csv";
    /// <summary>
    /// 현재 버전 넘버를 가지고 다르면 업데이트 하는 메서드
    /// </summary>
    /// <param name="csvFile"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    internal CSVData LordUIFile()
    {
        //새로운  CSVData 객체 생성
        CSVData data = new();

        //각 값이 바뀔 때마다 해당 Dictionary를 바꿔주는 Dictionary
        Dictionary<int, string> addDate = null;

        //CSV파일의 마지막 ,을 지워줌
        TextAsset contents = Main.Resource.Load<TextAsset>("GameUIScript.csv");
        string csvText = contents.text[..^1];

        //CSV파일을 줄 별로 잘라 배열로 만들어줌
        string[] rows = csvText.Split(new char[] { '\n' });

        // rows을 for문을 통해 반복 처리
        for (int i = 0; i < rows.Length; i++)
        {
            //rows을 다시 한번 , 을 기준으로 잘라줌
            string[] rowsValues = rows[i].Split(new char[] { ',' });

            // rowsValues의 첫값 즉 CSV의 첫번째 값이 존재하면 처리할 if문
            if (rowsValues[0] != "")
            {
                //아니라면 0번째 인덱스에 값에 따라 배치를 바꿔주는 인덱스 실행
                RowCheck(data, rowsValues, ref addDate);
                // 아래 코드를 실행하지 않고 다음 반복문 실행
                continue;
            }
            //존재하지 않는다면 해당 1번째 인덱스와 2번째 인덱스의 값을 Dictionary에 담아줌
            addDate.Add(int.Parse(rowsValues[1]), rowsValues[2][..^1]);
        }
        // 해당 for문의 반복이 끝나면 해당 데이터를 리턴
        return data;
    }

    /// <summary>
    /// 각 파일의 배치가 넘어갈 때 배치를 바꿔주는 메서드
    /// </summary>
    /// <param name="data"></param>
    /// <param name="rowValues"></param>
    /// <param name="addDate"></param>
    private void RowCheck(CSVData data, string[] rowValues, ref Dictionary<int, string> addDate)
    {
        // 스위치문 지양해야하는데 도대체 어캐해야하누 힘들다.
        switch (rowValues[0])
        {
            case "GameOverScript":
                addDate = data.gameOverScript;
                break;
            case "GameLoadingScript1":
                addDate = data.gameLoadingScript1;
                break;
            case "GameLoadingScript2":
                addDate = data.gameLoadingScript2;
                break;
        }
    }

    internal ItemDataContainer LordItemFile()
    {
        ItemDataContainer data = new ItemDataContainer();



        TextAsset contents = Main.Resource.Load<TextAsset>("GameItemData.csv");
        string csvText = contents.text[..^1];

        string[] itemdata = csvText.Split(new char[] { '\n' });

        for (int i = 0; i < itemdata.Length; i++)
        {
            ItemData item = new ItemData();
            string[] rowsValues = itemdata[i].Split(new char[] { ',' });
            if (rowsValues[0] == "") continue;
            item.id = rowsValues[1];
            item.name = rowsValues[2];
            item.category = rowsValues[3];
            item.description = rowsValues[4];
            item.duration = float.Parse(rowsValues[5]);
            item.power = float.Parse(rowsValues[6]);

            data.Items.Add(int.Parse(rowsValues[0]), item);
        }
        return data;

    }

    internal Vector3DataContainer LordItemVectorFile()
    {
        Vector3DataContainer data = new Vector3DataContainer();
        TextAsset contents = Main.Resource.Load<TextAsset>("GameItemPosition.csv");
        string csvText = contents.text[..^1];

        string[] itemVectordata = csvText.Split(new char[] { '\n' });

        for (int i = 0; i < itemVectordata.Length; i++)
        {
            string[] rowsValues = itemVectordata[i].Split(new char[] { ',' });
            if (rowsValues[0] != "") continue;

            Vector3Data vector3Data = new Vector3Data(new Vector3(float.Parse(rowsValues[2]), float.Parse(rowsValues[3]), float.Parse(rowsValues[4])));

            data.ItemVectorDate.Add(int.Parse(rowsValues[1]), vector3Data);
        }
        return data;

    }
}
