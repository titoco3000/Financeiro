using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.IO;

public static class Printer
{
    public static void Print(Tabela3.Coluna[] headers, List<List<Tabela3.TableItem>> content)
    {

        string html = "<!DOCTYPE html>\n<html>\n<head>\n<style>\ntable {\nfont-family: arial, sans - serif;\nborder-collapse:collapse;\nwidth: 900px;\n}\ntd, th {\nborder: 1px solid #dddddd;text - align: left;\npadding: 8px;\n}\n</style>\n</head>\n<body>\n<table>\n<tr>\n";
        for (int i = 0; i < headers.Length; i++)
        {
            html += "<th>" + headers[i].Title +"</th>\n";
        }
        html += "</tr>\n";
        for (int i = 0; i < content.Count; i++)
        {
            html += "<tr>\n";
            for (int j = 0; j < headers.Length; j++)
            {
                html += "<td>";
                html += content[i][j].Value;
                html += "</td>\n";

            }
            html += "</tr>\n";
        }
        html += "</table>\n</body>\n</html>\n";



        string path = Application.persistentDataPath + "\\print.html";
        File.WriteAllText(path, html);
        Application.OpenURL(path);

    }

}
