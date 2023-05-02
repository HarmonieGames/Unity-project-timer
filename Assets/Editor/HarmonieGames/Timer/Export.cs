using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Export : MonoBehaviour
{
    private const string Separator = ",";
        
    public static void ExportInCsv(string[] headers, IEnumerable<string[]> data)
    {
        var csv = new List<string>();

        var header = string.Join(Separator, headers);
        csv.Add(header);

        csv.AddRange(data.Select(d => string.Join(Separator, d)));

        //Ask user where to save file
        var filePath = EditorUtility.SaveFilePanel("Save CSV", "", "Export.csv", "csv");
        System.IO.File.WriteAllLines(filePath, csv.ToArray());
    }
}
