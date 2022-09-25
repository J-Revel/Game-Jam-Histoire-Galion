using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimationDB : MonoBehaviour
{
    [SerializeField]
    private List<MapAnimationData> animatedMapGameObjects;

    private Dictionary<MapAnimationType, GameObject> typeToObjectDico = new Dictionary<MapAnimationType, GameObject>();

    public void Init()
    {
        foreach(MapAnimationData data in animatedMapGameObjects)
        {
            typeToObjectDico.Add(data.type, data.objectAnimated);
            // data.objectAnimated.SetActive(false);
        }
    }

    public GameObject Get(MapAnimationType type)
    {
        foreach (MapAnimationData mapData in this.animatedMapGameObjects)
        {
            if (mapData.type.Equals(type))
            {
                Debug.Assert(mapData.objectAnimated.activeInHierarchy == false, "This animation has already been triggered !", this);
                return mapData.objectAnimated;
            }
        }

        Debug.LogError($"No animation have been found for the mapAnimationType '{type}'", this);
        return null;
    }
}

[Serializable]
public class MapAnimationData
{
    public MapAnimationType type;
    public GameObject objectAnimated;
}

public enum MapAnimationType
{
    None,
    PinPosition,
    Explosion,
}