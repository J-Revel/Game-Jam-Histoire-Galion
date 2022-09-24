using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class GaugeEffect
{
    public string gauge;
    public int effect;
}

[System.Serializable]
public class ChoiceEffectData
{
    public string text;
    public GaugeEffect[] effects;
}

[System.Serializable]
public class ChoiceSectionData
{
    public List<ChoiceEffectData> options = new List<ChoiceEffectData>();
}

[System.Serializable]
public class ChoiceEventData
{
    public string id;
    public string date;
    public string text;
    public ChoiceSectionData[] sections;
}

public class ScenarioData : MonoBehaviour
{
    public TextAsset spreadsheetAsset;
    public List<ChoiceEventData> choiceEvents = new List<ChoiceEventData>();
    public int maxChoiceCount = 4;
    public void Start()
    {
        SpreadsheetData sheet = SpreadsheetLoader.Load(spreadsheetAsset);
        ChoiceEventData currentEvent = null;
        for(int line=0; line < sheet.size.x; line++)
        {
            if(sheet.data[new Vector2Int(line, 0)].Length > 0)
            {
                if(currentEvent != null)
                {
                    ChoiceEffectData[] selectedValues = new ChoiceEffectData[currentEvent.sections.Length];
                    for(int i=0; i<currentEvent.sections.Length; i++) {
                        if(currentEvent.sections[i].options.Count > 0)
                            selectedValues[i] = currentEvent.sections[i].options[UnityEngine.Random.Range(0, currentEvent.sections[i].options.Count)];
                    }
                }
                currentEvent = new ChoiceEventData();
                currentEvent.id = sheet.data[new Vector2Int(line, 0)];
                
                currentEvent.sections = new ChoiceSectionData[maxChoiceCount];
                for(int i=0; i<currentEvent.sections.Length; i++)
                    currentEvent.sections[i] = new ChoiceSectionData();
                choiceEvents.Add(currentEvent);
                currentEvent.text = sheet.data[new Vector2Int(line, 1)];
                currentEvent.date = sheet.data[new Vector2Int(line, 2)];
            }
            for(int choiceIndex=0; choiceIndex < maxChoiceCount; choiceIndex++)
            {
                int optionTextColumn = choiceIndex * 2 + 3;
                int optionEffectColumn = choiceIndex * 2 + 3;
                if(sheet.data[new Vector2Int(line, optionTextColumn)].Length > 0)
                {
                    ChoiceEffectData effectData = new ChoiceEffectData();
                    effectData.text = sheet.data[new Vector2Int(line, optionTextColumn)];
                    try {
                        effectData.effects = ParseGaugeEffect(sheet.data[new Vector2Int(line, optionEffectColumn)]);
                    } catch(Exception e)
                    {
                        
                    }
                    currentEvent.sections[choiceIndex].options.Add(effectData);
                }
            }
            
        }
    }

    private GaugeEffect[] ParseGaugeEffect(string text)
    {
        Debug.Log("============");
        Debug.Log(" => ");
        string[] splits = text.Replace(" ", "").Split(',');
        GaugeEffect[] result = new GaugeEffect[splits.Length];
        for(int i=0; i<splits.Length; i++)
        {
            string split = splits[i];
            Debug.Log("Split : " + split);
            int separatorIndex = split.IndexOf('(');
            string gaugeName = split.Substring(0, separatorIndex);
            string effectText = split.Substring(separatorIndex+1);
            GaugeEffect effect = new GaugeEffect();
            effect.gauge = gaugeName;
            Debug.Log("PARSE " + effectText.Substring(0, effectText.Length - 1));
            try {
                effect.effect = int.Parse(effectText.Substring(0, effectText.Length - 1));
            }
            catch(Exception e){

            }
            result[i] = effect;
        }
        return result;
    }
}