using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 Mantém o campo de valor bonito retorna o valor do campo
     */
public class Input_Valor : MonoBehaviour
{
    public InputField field;

    public void OnValueChanged(string value)
    {
        for (int i = value.Length-1; i >= 0; i--)
        {
            if(value[i] != "1"[0] &&
               value[i] != "2"[0] &&
               value[i] != "3"[0] &&
               value[i] != "4"[0] &&
               value[i] != "5"[0] &&
               value[i] != "6"[0] &&
               value[i] != "7"[0] &&
               value[i] != "8"[0] &&
               value[i] != "9"[0] &&
               value[i] != "0"[0] &&
               value[i] != "."[0] &&
               value[i] != ","[0]   )
            {
                value = value.Remove(i,1);
            }
        }
        field.text = value;
    }

    public void OnExitField(string value)
    {
        field.text = FormatString(value);
    }

    public string FormatString(string value)
    {
        value = value.Replace(".", "");
        bool foundComma = false;
        for (int i = value.Length - 1; i >= 0; i--)
        {
            if (value[i] == ","[0])
            {
                if (foundComma)
                {
                    value = value.Remove(i, 1);
                }
                else
                {
                    foundComma = true;
                }
            }
        }
        print("1- " + value);

        if (!foundComma)
        {
            value += ",00";
        }
        else
        {
            int commaPos = value.LastIndexOf(","[0]);
            value += "00";
            value = value.Substring(0, commaPos + 3);
        }
        return Ultilities.Money(Ultilities.ParseFloat(value));
    }

    public float Valor()
    {
        string v = FormatString(field.text).Replace(".", "").Replace(",", ".");
        print(v);
        return float.Parse(v, System.Globalization.CultureInfo.InvariantCulture);
    }

    /*private void Start()
    {
        OnValueChanged();
    }
    public void OnValueChanged()
    {
        string value = field.text;
        if(value.Length < 3)
        {
            field.text = "0" + value;
        }
        else
        {
            if(value.Length > 3)
            {
                if(value.Substring(0,1) == "0")
                {
                    field.text = value.Substring(1, value.Length - 1);
                    value = field.text;
                }
                //field.text = "0" + value;
            }
            string textoVisual = PlacePontos( value.Substring(0, value.Length - 2) + "," + value.Substring(value.Length - 2, 2) );
            texto.text = textoVisual;

        }

    }

    

    public string PlacePontos(string comVirgula)
    {
        int i = comVirgula.Length - 3;
        
        while (i > 3)
        {
            i -= 3;
            comVirgula = comVirgula.Insert(i, ".");
        }
        return comVirgula;

    }*/
}
