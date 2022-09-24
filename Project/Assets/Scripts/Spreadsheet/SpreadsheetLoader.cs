using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadsheetData
{
    public Vector2Int size;
    public Dictionary<Vector2Int, string> data = new Dictionary<Vector2Int, string>();
}

public class SpreadsheetLoader : MonoBehaviour
{
    public static SpreadsheetData Load(TextAsset textAsset)
    {
        SpreadsheetData result = new SpreadsheetData();
        string dataStr = textAsset.text;
        string[] lines = dataStr.Split("\r\n");
        result.size.x = lines.Length;
        for(int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string[] elements = lines[lineIndex].Split('\t');
            if(elements.Length > result.size.y)
                result.size.y = elements.Length;
            for(int colIndex = 0; colIndex < elements.Length; colIndex++)
            {
                result.data[new Vector2Int(lineIndex, colIndex)] = elements[colIndex];
            }
        }
        return result;
    }

}
