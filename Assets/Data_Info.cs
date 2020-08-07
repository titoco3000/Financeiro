using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 Mantém uma lista atualizada das compras dentro do período selecionado
*/
public class Data_Info : MonoBehaviour
{
    public List<Compra> comprasNoPeriodo;

    public Text DataInicial;
    public Text DataFinal;

    public InfoTab tab;

    private void Start()
    {
        tab = FindObjectOfType<InfoTab>();
    }

    IEnumerator AsyncStart()
    {
        yield return new WaitForEndOfFrame();
        RecalcularComprasNoPeriodo();
    }

    public void RecalcularComprasNoPeriodo()
    {
        comprasNoPeriodo = new List<Compra>();
        List<Compra> todasAsCompras = StorageManager.Compras();
        for (int i = 0; i < todasAsCompras.Count; i++)
        {
            Data inicio = new Data(DataInicial.text);
            Data final = new Data(DataFinal.text);
            if (Data.EstaEntre(new Data(todasAsCompras[i].Data), inicio, final))
            {
                comprasNoPeriodo.Add(todasAsCompras[i]);
            }
        }

        //atualiza o gráfico
        tab.Refresh();
    }

    private void OnEnable()
    {
        StartCoroutine(AsyncStart());
    }


}
