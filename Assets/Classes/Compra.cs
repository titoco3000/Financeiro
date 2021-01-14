using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compra
{
    public string Fornecedor;
    public AreasDoHotel Area;
    public string Data;
    public float Valor;
    public FormaDePagamento Pagamento;
    public int NotaFiscal;
    public Caixa Banco;
    public string Obs;

    public Compra(string fornecedor, AreasDoHotel area, string data, float valor, FormaDePagamento pagamento, int notaFiscal, Caixa banco, string obs)
    {
        Fornecedor = fornecedor;
        Area = area;
        Data = data;
        //MonoBehaviour.FindObjectOfType<MensagemVolatil>().SetMessage("Data: " + valor.ToString(),false,true);
        Valor = valor;
        Pagamento = pagamento;
        NotaFiscal = notaFiscal;
        Banco = banco;
        Obs = obs;
    }

    public override string ToString()
    {
        return Fornecedor + ", " + Area.ToString() + ", " + Data + ", " + Valor;
    }

    public static int LocateInList(List<Compra> lista, string Nome , int notaFiscal)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            if (lista[i].NotaFiscal == notaFiscal && lista[i].Fornecedor == Nome)
                return i;
        }
        return -1;
    }
}
