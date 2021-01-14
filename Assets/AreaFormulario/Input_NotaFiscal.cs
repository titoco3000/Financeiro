using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_NotaFiscal : MonoBehaviour
{
    public InputField field;
    public void OnEndEdit()
    {
        while (field.text.Length < 6)
            field.text = "0" + field.text;
        while (field.text.Length > 6)
            field.text = field.text.Substring(1,field.text.Length-1);
        
    }
}
