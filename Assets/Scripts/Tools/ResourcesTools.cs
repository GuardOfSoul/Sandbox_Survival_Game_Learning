using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源加载工具类
/// </summary>
public sealed class ResourcesTools {

    /// <summary>
    /// 加载文件资源
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="targetDic"></param>
    public static Dictionary<string, Sprite> LoadFolderAssets(string folderName, Dictionary<string, Sprite> targetDic)
    {
        Sprite[] tempList = Resources.LoadAll<Sprite>(folderName);
        for (int i = 0; i < tempList.Length; i++)
        {
            targetDic.Add(tempList[i].name, tempList[i]);
        }
        return targetDic;
    }

    /// <summary>
    /// 通过名字获取资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static Sprite GetSpriteByName(string name, Dictionary<string, Sprite> dic)
    {
        Sprite temp = null;
        dic.TryGetValue(name, out temp);
        return temp;
    }
}
