using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct KeyValue
{
    public string eventId;
    public Sprite sprite;
}

public class EventBoundTexture : MonoBehaviour
{
    public KeyValue[] textures;
    private AnimableNews news;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        news = GetComponentInParent<AnimableNews>();
        foreach(KeyValue texture in textures)
        {
            if(texture.eventId == news.id)
            {
                image.sprite = texture.sprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
