using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimableNewsDB : MonoBehaviour
{
    [SerializeField]
    private List<AnimableNewsData> animatedNewsData;

    private Dictionary<NewsType, AnimableNews> typeToObjectDico = new Dictionary<NewsType, AnimableNews>();

    public void Init()
    {
        foreach (AnimableNewsData data in animatedNewsData)
        {
            typeToObjectDico.Add(data.type, data.news);
            data.news.gameObject.SetActive(false);
        }
    }

    public AnimableNews Get(NewsType type)
    {
        foreach (AnimableNewsData mapData in this.animatedNewsData)
        {
            if (mapData.type.Equals(type))
            {
                return mapData.news;
            }
        }

        Debug.LogError($"No animation have been found for the mapAnimationType '{type}'", this);
        return null;
    }
}

[Serializable]
public class AnimableNewsData
{
    public NewsType type;
    public AnimableNews news;
}

public enum NewsType
{
    Event0,
    Event1,
    Event2,
    Event3,
}
