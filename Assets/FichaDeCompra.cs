using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class FichaDeCompra : MonoBehaviour
{
    public class Retorno : UnityEvent<Compra>
    {
    }
    public Text[] elementos;
    bool aprovado = false;
    bool cancelado = false;

    public void BuscarAprovacao(Compra compra, Retorno retorno)
    {
        StartCoroutine(AprovCoroutine(compra, retorno));
    }

    public void Aprovar()
    {
        aprovado = true;
    }
    public void Cancelar()
    {
        cancelado = true;
    }

    private IEnumerator AprovCoroutine(Compra compra, Retorno retorno)
    {
        elementos[0].text = compra.Fornecedor;
        elementos[1].text = compra.Data;
        elementos[2].text = System.Enum.GetName(typeof(AreasDoHotel), (AreasDoHotel)compra.Area);
        elementos[3].text = "R$ " + Ultilities.Money(compra.Valor);
        elementos[4].text = System.Enum.GetName(typeof(FormaDePagamento), (FormaDePagamento)compra.Pagamento);
        elementos[5].text = Ultilities.FormatarNF(compra.NotaFiscal);
        elementos[6].text = System.Enum.GetName(typeof(Caixa), (Caixa)compra.Banco);
        elementos[7].text = compra.Obs;

        while (aprovado == cancelado)
        {
            yield return 0;
        }
        if (aprovado)
        {
            retorno.Invoke(compra);
        }
        aprovado = false;
        cancelado = false;
        gameObject.SetActive(false);
    }


}
