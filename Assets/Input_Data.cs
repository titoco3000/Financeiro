using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/*
 Mantém a data bonita
     */
public class Input_Data : MonoBehaviour
{
    public InputField field;

    
    public int defaultDay = 0;

    private void Start()
    {
        ResetData();
    }
    public void OnEdit()
    {
        //verifica se foi backspace
        bool backspace = field.text.Length < 8;

        //get number-only value
        string value = GetNumbers(field.text);



        //busca a posição do caret(editor) original
        int caretPos = field.caretPosition;

        //adiciona zeros para completar
        while (value.Length < 6)
        {
            value += "0";
        }

        //remove numeros a mais
        value = value.Substring(0, 6);


        //adiciona as barras
        value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/" + value.Substring(4, 2);


        //verifica se o caret está na posição das barras
        if (caretPos == 2)
        {
            if (backspace)
            {
                value = value.Substring(0, 1) + value.Substring(2, 6) + "0";
                field.caretPosition = 1;
            }
            else
                field.caretPosition = 3;
        }
        else if (caretPos == 5)
        {
            if (backspace)
            {
                value = value.Substring(0, 4) + "0/" + value.Substring(6, 2);
                field.caretPosition = 4;
            }
            else
                field.caretPosition = 6;
        }

        field.text = value;
    }

    public void ResetData()
    {
        field.text = new Data(System.DateTime.Now.AddDays(defaultDay)).ToString();
        
    }

    private static string GetNumbers(string input)
    {
        string output = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
                output += input[i];
        }
        return output;
    }
}
