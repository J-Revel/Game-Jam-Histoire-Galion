using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoiceEventData
{
    public string id;
    public string text;
    public List<string>[] choices;
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
                    string[] selectedValues = new string[currentEvent.choices.Length];
                    for(int i=0; i<currentEvent.choices.Length; i++) {
                        if(currentEvent.choices[i].Count > 0)
                            selectedValues[i] = currentEvent.choices[i][Random.Range(0, currentEvent.choices[i].Count)];
                    }
                    Debug.Log(string.Format(currentEvent.text, selectedValues));
                }
                currentEvent = new ChoiceEventData();
                currentEvent.id = sheet.data[new Vector2Int(line, 0)];
                
                currentEvent.choices = new List<string>[sheet.size.y-2];
                for(int i=0; i<currentEvent.choices.Length; i++)
                    currentEvent.choices[i] = new List<string>();
                choiceEvents.Add(currentEvent);
                currentEvent.text = sheet.data[new Vector2Int(line, 1)];
            }
            for(int choiceIndex=0; choiceIndex < sheet.size.y - 2; choiceIndex++)
            {
                if(sheet.data[new Vector2Int(line, choiceIndex + 2)].Length > 0)
                    currentEvent.choices[choiceIndex].Add(sheet.data[new Vector2Int(line, choiceIndex + 2)]);
            }
            
        }
    }
}