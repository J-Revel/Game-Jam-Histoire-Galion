using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string hoverStartSound;
    public string hoverEndSound;
    public string clickSound;
    
    private FMOD.Studio.EventInstance hoverStartInstance;
    private FMOD.Studio.EventInstance hoverEndInstance;
    private FMOD.Studio.EventInstance clickInstance;

    void Start()
    {
        if(hoverStartSound != null && hoverStartSound.Length > 0)
            hoverStartInstance = FMODUnity.RuntimeManager.CreateInstance(hoverStartSound);
        if(hoverEndSound != null && hoverEndSound.Length > 0)
            hoverEndInstance = FMODUnity.RuntimeManager.CreateInstance(hoverEndSound);
        if(clickSound != null && clickSound.Length > 0)
            clickInstance = FMODUnity.RuntimeManager.CreateInstance(clickSound);

    }
    public void OnPointerEnter(PointerEventData data)
    {
        hoverStartInstance.start();
    }

    public void OnPointerExit(PointerEventData data)
    {
        hoverEndInstance.start();
    }
    
    public void OnPointerClick(PointerEventData data)
    {
        clickInstance.start();
    }
}
