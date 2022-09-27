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

public enum GaugeComparison
{
    Equals,
    LowerOrEqual,
    Lower,
    HigherOrEqual,
    Higher,
    Different,
}

[System.Serializable]
public class EventCondition
{
    public string leftGauge;
    public string rightGauge;
    public GaugeComparison comparison;
}

[System.Serializable]
public class EventData 
{
    public string id;
    public string date;
    public EventCondition[] conditions;
    public string title;
    public string text; 
}

public class ScenarioData : MonoBehaviour
{
    public TextAsset spreadsheetAsset;
    public TextAsset[] eventSpreadsheetAssets;
    public List<ChoiceEventData> choiceEvents = new List<ChoiceEventData>();
    public List<EventData> events = new List<EventData>();
    public int maxChoiceCount = 4;

    public void Awake()
    {
        SpreadsheetData sheet = SpreadsheetLoader.Load(spreadsheetAsset);
        choiceEvents = ParseChoiceEvents(sheet);

        foreach(TextAsset eventSpreadsheetAsset in eventSpreadsheetAssets)
        {
            SpreadsheetData eventSheet = SpreadsheetLoader.Load(eventSpreadsheetAsset);
            foreach(EventData evt in ParseScenarioEvents(eventSheet)) {
                events.Add(evt);
            }
        }
    }

    private List<ChoiceEventData> ParseChoiceEvents(SpreadsheetData sheet)
    {
        List<ChoiceEventData> result = new List<ChoiceEventData>();
        ChoiceEventData currentEvent = null;
        for(int line=1; line < sheet.size.x; line++)
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
                result.Add(currentEvent);
                currentEvent.text = sheet.data[new Vector2Int(line, 1)];
                currentEvent.date = sheet.data[new Vector2Int(line, 2)];
            }
            for(int choiceIndex=0; choiceIndex < maxChoiceCount; choiceIndex++)
            {
                int optionTextColumn = choiceIndex * 2 + 3;
                int optionEffectColumn = choiceIndex * 2 + 4;
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

        return result;
    }

    private GaugeEffect[] ParseGaugeEffect(string text)
    {
        string[] splits = text.Replace(" ", "").Split(',');
        GaugeEffect[] result = new GaugeEffect[splits.Length];
        for(int i=0; i<splits.Length; i++)
        {
            string split = splits[i];
            int separatorIndex = split.IndexOf('(');
            string gaugeName = split.Substring(0, separatorIndex);
            string effectText = split.Substring(separatorIndex+1);
            GaugeEffect effect = new GaugeEffect();
            effect.gauge = gaugeName;
            try {
                effect.effect = int.Parse(effectText.Substring(0, effectText.Length - 1));
            }
            catch(Exception e){

            }
            result[i] = effect;
        }
        return result;
    }

    private EventCondition ParseEventCondition(string text)
    {
        string operatorChars = "<>=!";
        int startOperatorIndex = text.Length;
        int endOperatorIndex = 0;
        Dictionary<string, GaugeComparison> comparisonStrings = new Dictionary<string,GaugeComparison> {
            { "=", GaugeComparison.Equals}, 
            { "!=", GaugeComparison.Different}, 
            { ">", GaugeComparison.Higher}, 
            { ">=", GaugeComparison.HigherOrEqual}, 
            { "<", GaugeComparison.Lower}, 
            { "<=", GaugeComparison.LowerOrEqual}, 
        };
        bool operatorFound = false;
        for(int i=0; i<text.Length; i++)
        {
            if(operatorChars.Contains(text[i]))
            {
                operatorFound = true;
                if(startOperatorIndex > i)
                    startOperatorIndex = i;
                if(endOperatorIndex < i)
                    endOperatorIndex = i;
            }
        }
        if(!operatorFound)
            return null;
        string leftVariableName = text.Substring(0, startOperatorIndex);
        string rightVariableName = text.Substring(endOperatorIndex + 1, text.Length - endOperatorIndex - 1);
        string operatorText = text.Substring(startOperatorIndex, endOperatorIndex - startOperatorIndex + 1);
        Debug.Log(leftVariableName + " " + operatorText + " " + rightVariableName);
        GaugeComparison comparison = comparisonStrings[operatorText];
        EventCondition result = new EventCondition();
        result.leftGauge = leftVariableName;
        result.rightGauge = rightVariableName;
        result.comparison = comparison;
        return result; 
    }

    public List<EventData> ParseScenarioEvents(SpreadsheetData sheet)
    {
        List<EventData> result = new List<EventData>();
        for(int line=0; line < sheet.size.x; line++)
        {
            EventData evt = new EventData();
            evt.id = sheet.data[new Vector2Int(line, 0)];
            evt.date = sheet.data[new Vector2Int(line, 1)];
            string[] conditionTexts = sheet.data[new Vector2Int(line, 2)].Replace(" ", "").Split(",");
            evt.conditions = new EventCondition[conditionTexts.Length];
            for(int i=0; i<conditionTexts.Length; i++)
            {
                evt.conditions[i] = ParseEventCondition(conditionTexts[i]);
            }
            evt.title = sheet.data[new Vector2Int(line, 3)];
            evt.text = sheet.data[new Vector2Int(line, 4)];
            result.Add(evt);
        }
        return result;
    }

    public void OnEventPick(EventData eventData)
    {
        this.events.Remove(eventData);
    }
}