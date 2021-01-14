using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;

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
}
