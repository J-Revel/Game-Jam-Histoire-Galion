using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimationDB : MonoBehaviour
{
    [SerializeField]
    private List<MapAnimationData> animatedMapGameObjects;

    private Dictionary<string, GameObject> typeToObjectDico = new Dictionary<string, GameObject>();

    public void Init()
    {
        foreach(MapAnimationData data in animatedMapGameObjects)
        {
            typeToObjectDico.Add(data.eventID, data.objectAnimated);
            //data.objectAnimated.SetActive(false);
        }
    }

    public GameObject Get(string eventID)
    {
        foreach (MapAnimationData mapData in this.animatedMapGameObjects)
        {
            if (mapData.eventID.Equals(eventID))
            {
                if(mapData.objectAnimated != null)
                    Debug.Assert(mapData.objectAnimated.activeInHierarchy == false, "This animation has already been triggered !", this);
                return mapData.objectAnimated;
            }
        }

        Debug.LogError($"No animation have been found for the eventID '{eventID}'", this);
        return null;
    }
}

[Serializable]
public class MapAnimationData
{
    public string eventID;
    public GameObject objectAnimated;
}