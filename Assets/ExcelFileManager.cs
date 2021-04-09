using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using LitJson;
using System.Text;

using SimpleFileBrowser;


public class ExcelFileManager:MonoBehaviour
{
    private void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Excel", ".xlsx"));
        FileBrowser.SetDefaultFilter(".xlsx");

    }
    public void SaveExcel( Tabela3.Coluna[] titles, List<List<Tabela3.TableItem>> content)
    {
        if(titles.Length == 0 || content.Count == 0)
        {
            return;
        }

        string CSVtitles = "";
        for (int i = 0; i < titles.Length; i++)
        {
            CSVtitles += titles[i].Title;
            if(i != titles.Length-1)
                CSVtitles += ",";
        }
        string CSVcontent = "";
        for (int i = 0; i < content.Count; i++)
        {
            for (int j = 0; j < content[i].Count; j++)
            {
                CSVcontent += content[i][j].Value.Replace(",",".");
                if (j != content[i].Count - 1)
                    CSVcontent += ",";
            }
            if (i != content.Count- 1)
                CSVcontent += "\n";
        }
       
        StartCoroutine(SaveExcel(CSVtitles, CSVcontent));
    }

    private IEnumerator SaveExcel(string titles, string content)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, "export.xlsx", "Escolha um destino", "Salvar");
        
        if (FileBrowser.Success)
        {
            try
            {

                File.WriteAllText(FileBrowser.Result[0].Substring(0, FileBrowser.Result[0].Length - 4) + "csv", titles + "\n" + content);

                string cmd = Path.GetDirectoryName(Application.dataPath) + "/converter.exe";


                string[] adress = cmd.Split("/"[0]);
                cmd = "";
                for (int i = 0; i < adress.Length; i++)
                {
                    if (adress[i].Contains(" "))
                    {
                        adress[i] = "\"" + adress[i] + "\"";
                    }
                    cmd += adress[i]+"/";
                }
                cmd = cmd.Substring(0, cmd.Length - 1);
                
                

                string origin = FileBrowser.Result[0].Substring(0, FileBrowser.Result[0].Length - 4) + "csv";
                string destination = FileBrowser.Result[0];
                origin = origin.Replace("\\", "/");
                destination = destination.Replace("\\", "/");

                File.WriteAllText(Path.GetDirectoryName(Application.dataPath) + "/conversionData.csv", origin+","+destination);

                System.Diagnostics.Process.Start("converter.exe");
            }
            catch (IOException)
            {
                print("Deu erro.");
                Ultilities.PopUp("Arquivo em uso. Feche-o e tente novamente");
            } 
        }
    }

}

