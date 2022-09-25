using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimatedWriter : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;
    public float writeDuration = 3;
    public string typewriterOneshotSound;
    public string typewriterSound;
    
    private FMOD.Studio.EventInstance typewriterOneshotSoundInstance;
    private FMOD.Studio.EventInstance typewriterSoundInstance;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        if(typewriterSound != null && typewriterSound.Length > 0)
            typewriterSoundInstance = FMODUnity.RuntimeManager.CreateInstance(typewriterSound);
        if(typewriterOneshotSound != null && typewriterOneshotSound.Length > 0)
            typewriterOneshotSoundInstance = FMODUnity.RuntimeManager.CreateInstance(typewriterOneshotSound);
    }
    public void WriteText(string newText)
    {
        StopAllCoroutines();
        StartCoroutine(WriteTextCoroutine(newText));
    }

    public IEnumerator WriteTextCoroutine(string newText)
    {
        typewriterOneshotSoundInstance.start();
        for(int cursor = 0; cursor < newText.Length; cursor++)
        {
            typewriterSoundInstance.start();
            text.text = newText.Substring(0, cursor) + "<alpha=#00>"+newText.Substring(cursor, newText.Length - cursor);
            yield return new WaitForSeconds(writeDuration / newText.Length);
        };
    }
}