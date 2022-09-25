using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimationDB : MonoBehaviour
{
    [SerializeField]
    private List<MapAnimationData> animatedMapGameObjects;

    private Dictionary<string, MapAnimationData> idToObjectDico = new Dictionary<string, MapAnimationData>();

    public void Init()
    {
        foreach(MapAnimationData data in animatedMapGameObjects)
        {
            idToObjectDico.Add(data.eventID, data);
        }
    }

    public bool TryGet(string eventID, out GameObject gameObject)
    {
        if(idToObjectDico.ContainsKey(eventID))
        {
            gameObject = idToObjectDico[eventID].objectAnimated;
            return true;
        }
        else
        {
            Debug.LogWarning($"No animation have been found for the eventID '{eventID}'", this);
            gameObject = null;
            return false;
        }
    }

    public bool TryGetDesactivable(string eventID, out GameObject gameObject)
    {
        if (idToObjectDico.ContainsKey(eventID))
        {
            var data = idToObjectDico[eventID];
            if (data.shouldBeDesactivated)
            {
                gameObject = data.objectAnimated;
                return true;
            }
        }


        Debug.LogWarning($"No animation have been found for the eventID '{eventID}'", this);
        gameObject = null;
        return false;
    }
}

[Serializable]
public class MapAnimationData
{
    public string eventID;
    public GameObject objectAnimated;
    public bool shouldBeDesactivated;
}