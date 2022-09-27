using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EventAudio
{
    public string eventId;
    public string soundEventId;
}
public class EventBoundSound : MonoBehaviour
{
    public EventAudio[] sounds;
    private AnimableNews news;
    
    private FMOD.Studio.EventInstance soundInstance;

    void Start()
    {
        news = GetComponentInParent<AnimableNews>();
        bool soundFound = false;
        foreach(EventAudio sound in sounds)
        {
            if(sound.eventId == news.id && sound.soundEventId.Length > 0)
            {
                soundInstance = FMODUnity.RuntimeManager.CreateInstance(sound.soundEventId);
                soundFound = true;
            }
        }
        if(soundFound)
            soundInstance.start();
    }
}
