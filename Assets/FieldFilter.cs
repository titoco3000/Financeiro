using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldFilter : MonoBehaviour
{
    public InputField field;
    public bool FilterEnabled;

    private void Start()
    {
        FiltrarConteudo();
    }
    public void FiltrarConteudo()
    {
        if (FilterEnabled)
        {
            string txt = field.text;
            txt = txt.Replace(",", ".");
            if (float.TryParse(txt, out float floatValue))
            {
                txt = floatValue.ToString("C2");
                txt = txt.Substring(1, txt.Length - 1).Replace(".", ",");
            }
            else
                txt = "0,00";
            field.text = txt;
        }
    }
}
