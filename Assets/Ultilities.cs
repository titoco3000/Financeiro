using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public static class Ultilities 
{
    public static string NF(int n)
    {
        string txt = n.ToString();
        while (txt.Length < 6)
        {
            txt = "0" + txt;
        }
        return txt;
    }
    public static string Money(float n)
    {
        string txt = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", n);

        txt = txt.Substring(3);

        return txt;
    }

    public static string FormatarNF(float n)
    {
        string text = ((int)n).ToString();
        while (text.Length < 6)
            text = "0" + text;
        while (text.Length > 6)
            text = text.Substring(1, text.Length - 1);
        
        return text;
    }

    public static string PadraoAmericano(string textPadraoBrasileiro)
    {
        string txt = textPadraoBrasileiro.Replace(".", ";");
        txt = txt.Replace(",", ".");
        txt = txt.Replace(";", ",");

        return Regex.Replace(txt, "[^.0-9]", "");
    }

    public static float ParseFloat(string s)
    {
        return float.Parse(Ultilities.PadraoAmericano(s), System.Globalization.CultureInfo.InvariantCulture);
    }

    public static bool IsFileLocked(string filename)
    {
        bool Locked = false;
        try
        {
            FileStream fs =
                File.Open(filename, FileMode.OpenOrCreate,
                FileAccess.ReadWrite, FileShare.None);
            fs.Close();
        }
        catch
        {
            Locked = true;
        }
        return Locked;
    }

    public static void PopUp(string msg)
    {
        Object.Instantiate(Resources.Load("Popup") as GameObject).GetComponent<PopUp>().Setup(msg);
        Debug.Log("popup");
    }
    public static string[][] ParseCSV(string csv)
    {
        string[] linhas = csv.Split("\n"[0]);
        string[][] retorno = new string[linhas.Length][];
        for (int i = 0; i < linhas.Length; i++)
        {
            
            retorno[i] = linhas[i].Trim().Split(","[0]);
        }
        return retorno;
    }
}
