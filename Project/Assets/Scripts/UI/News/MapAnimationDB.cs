using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class MapAnimationDB : MonoBehaviour
{
    [SerializeField]
    private List<MapAnimationData> animatedMapGameObjects;

    private Dictionary<string, GameObject> idToObjectDico = new Dictionary<string, GameObject>();

    public void Init()
    {
        foreach(MapAnimationData data in animatedMapGameObjects)
        {
            idToObjectDico.Add(data.eventID, data.objectAnimated);
            //data.objectAnimated.SetActive(false);
        }
    }

    public bool TryGet(string eventID, out GameObject gameObject)
    {
        if(idToObjectDico.ContainsKey(eventID))
        {
            gameObject = idToObjectDico[eventID];
            return true;
        }
        else
        {
            Debug.LogWarning($"No animation have been found for the eventID '{eventID}'", this);
            gameObject = null;
            return false;
        }
    }
}

[Serializable]
public class MapAnimationData
{
    public string eventID;
    public GameObject objectAnimated;
}