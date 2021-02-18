using UnityEngine;
using UnityEditor;
using System.Collections;
using OfficeOpenXml;
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
                CSVcontent += content[i][j].Value;
                if (j != content[i].Count - 1)
                    CSVcontent += ",";
            }
            if (i != content.Count- 1)
                CSVcontent += "\n";
        }
        print(CSVtitles);
        print(CSVcontent);

        StartCoroutine(SaveExcel(CSVtitles, CSVcontent));
    }

    public bool SaveExcel(string path, string titles, string content)
    {
        return true;
    }

    private IEnumerator SaveExcel(string titles, string content)
    {
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, "export.xlsx", "Escolha um destino", "Salvar");
        
        if (FileBrowser.Success)
        {
            print(FileBrowser.Result[0]);
            Excel xls = new Excel();
            ExcelTable table = new ExcelTable();
            table.TableName = "test";
            string outputPath = FileBrowser.Result[0];
            xls.Tables.Add(table);

            string[][] parsedTitles = Ultilities.ParseCSV(titles);
            for (int i = 0; i < parsedTitles[0].Length; i++)
            {
                xls.Tables[0].SetValue(1, 1 + i, parsedTitles[0][i]);
            }

            string[][] parsedContent = Ultilities.ParseCSV(content);
            for (int i = 0; i < parsedContent.Length; i++)
            {
                for (int j = 0; j < parsedContent[i].Length; j++)
                {
                    xls.Tables[0].SetValue(2+i, 1 + j, parsedContent[i][j]);

                }
            }

            xls.ShowLog();
            if (Ultilities.IsFileLocked(outputPath))
            {
                Ultilities.PopUp("O arquivo está aberto em outro programa. Feche-o e tente novamente");
                yield break;
            }
            ExcelHelper.SaveExcel(xls, outputPath);
            System.Diagnostics.Process.Start(outputPath);
        }
    }

}
/*
 Excel xls = new Excel();
        ExcelTable table = new ExcelTable();
        table.TableName = "test";
        string outputPath = path;
        xls.Tables.Add(table);
        
        for (int i = 0; i < titles.Length; i++)
        {
             xls.Tables[0].SetValue(1, 1+i, titles[i].Title);
        }
        for (int i = 0; i < content.Count; i++)
        {
            for (int j = 0; j < content[i].Count; j++)
            {
                ExcelTableCell cell = xls.Tables[0].SetValue(2+i, 1 + j, content[i][j].Value);
                cell.Type = ExcelTableCellType.Popup;
            }
        }


        xls.ShowLog();
        if (Ultilities.IsFileLocked(path))
        {
            Ultilities.PopUp("O arquivo está aberto em outro programa. Feche-o e tente novamente");
            return false;
        }
        LogFile.Log(null, "Indo salvar");
        ExcelHelper.SaveExcel(xls, outputPath);         
        System.Diagnostics.Process.Start(path);
        return true;
 */
