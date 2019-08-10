using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public sealed class JsonTools {

    /// <summary>
    /// 加载并解析json数据
    /// </summary>
    /// <param name="fileName">json文件名</param>
    /// <returns>List对象</returns>
    public static List<T> LoadJsonFile<T>(string fileName)
    {
        List<T> tempList = new List<T>();
        string tempJsonStr = Resources.Load<TextAsset>("JsonData/" + fileName).text;

        //解析json
        JsonData jsonData = JsonMapper.ToObject(tempJsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            tempList.Add(JsonMapper.ToObject<T>(jsonData[i].ToJson()));
        }
        return tempList;
    }
}
