using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GaugeEffect
{
    string gauge;
    int effect;
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
    public string text;
    public ChoiceSectionData[] sections;
}

public class ScenarioData : MonoBehaviour
{
    public TextAsset spreadsheetAsset;
    public List<ChoiceEventData> choiceEvents = new List<ChoiceEventData>();
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
                            selectedValues[i] = currentEvent.sections[i].options[Random.Range(0, currentEvent.sections[i].options.Count)];
                    }
                }
                currentEvent = new ChoiceEventData();
                currentEvent.id = sheet.data[new Vector2Int(line, 0)];
                
                currentEvent.sections = new ChoiceSectionData[sheet.size.y-2];
                for(int i=0; i<currentEvent.sections.Length; i++)
                    currentEvent.sections[i] = new ChoiceSectionData();
                choiceEvents.Add(currentEvent);
                currentEvent.text = sheet.data[new Vector2Int(line, 1)];
            }
            for(int choiceIndex=0; choiceIndex < sheet.size.y - 2; choiceIndex++)
            {
                if(sheet.data[new Vector2Int(line, choiceIndex + 2)].Length > 0)
                {
                    ChoiceEffectData effectData = new ChoiceEffectData();
                    effectData.text = sheet.data[new Vector2Int(line, choiceIndex + 2)];
                    currentEvent.sections[choiceIndex].options.Add(effectData);
                }
            }
            
        }
    }
}