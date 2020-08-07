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
    public Text texto;

    private void Start()
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
            texto.text = value.Substring(0,value.Length -2) + "," + value.Substring(value.Length-2,2);

        }

    }

    public float Valor()
    {
        string v = texto.text.Replace(",", ".");
        return float.Parse(v, System.Globalization.CultureInfo.InvariantCulture);
    }
}
