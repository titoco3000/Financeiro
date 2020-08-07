using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Fica em um botão que, ao ser clicado, preenche o campo com algum valor
     */
public class MatchListItem : MonoBehaviour
{
    public Text label;
    public void OnClick()
    {
        GameObject f = GameObject.Find("Nome do Fornecedor");
        f.GetComponent<Input_NomeDoFornecedor>().Autocompletar(label.text);
    }
}
