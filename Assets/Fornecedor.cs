using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fornecedor 
{
    public string Nome;
    public AreasDoHotel Area;
    public FormaDePagamento pagamentoPreferido;
    public Caixa bancoPreferido;

    public Fornecedor(string nome, AreasDoHotel area, FormaDePagamento p, Caixa b)
    {
        Nome = nome;
        Area = area;
        pagamentoPreferido = p;
        bancoPreferido = b;
    }

    public static int LocateInList(List<Fornecedor> lista,  string nome)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].Nome == nome)
                return i;
        }
        return -1;
    }

}
