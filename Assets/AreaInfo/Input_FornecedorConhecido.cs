using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_FornecedorConhecido : AutocompleteField
{
    private void Start()
    {
        EditarValores(StorageManager.Fornecedores());
    }

    public void EditarValores(List<Fornecedor> f)
    {
        ValoresASeremProcurados = new List<string>();
        foreach (var item in f)
        {
            ValoresASeremProcurados.Add(item.Nome);
        }
        if (ValoresASeremProcurados.Count > 0)
        {
            field.text = ValoresASeremProcurados[0];
            matchesList.gameObject.SetActive(false);
        }



    }

}
