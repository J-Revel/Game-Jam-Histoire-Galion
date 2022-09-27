using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicPlayer : MonoBehaviour
{
    public GameController gameController;
    public string musicSound = "{682b1e84-0b86-443c-beaf-6cf8c8e4e152}";
    private FMOD.Studio.EventInstance musicInstance;
    public float valueUpdateDuration = 0.5f;
    public float currentValue = 0;
    public float debugValue = 100;
    public bool debugMode = false;
    public string parameterName = "NatioFMOD";

    void Start()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicSound);
        musicInstance.start();
        StartCoroutine(UpdateValueCoroutine());
    }

    void Update()
    {
        if(debugMode)
        {
            StopAllCoroutines();
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, debugValue);
        }
    }

    IEnumerator UpdateValueCoroutine()
    {
        yield return null;
        // currentValue = 100;
        // for(float time=0; time < 10; time += Time.deltaTime)
        // {
        //     musicInstance.setParameterByName("NatioFMOD", currentValue);
        //         yield return null;
        // }
        currentValue = gameController.state.GlobalNationalism;
        while(true)
        {
            while(gameController.state.GlobalNationalism == currentValue)
                yield return null;
            float targetValue = gameController.state.GlobalNationalism;
            float startValue = currentValue;
            for(float time=0; time < valueUpdateDuration; time += Time.deltaTime)
            {
                currentValue = Mathf.Lerp(startValue, targetValue, time / valueUpdateDuration);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, currentValue);
                yield return null;
            }
            currentValue = targetValue;
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, targetValue);
        }

    }
}
