using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Possui a referencia a todos os botoes de infografico
     */
public class InfoTab : MonoBehaviour
{
    public List<Infographic_Buttom> buttoms;
    void Start()
    {
        buttoms = new List<Infographic_Buttom>();
        for (int i = 0; i < transform.childCount; i++)
        {
            buttoms.Add(transform.GetChild(i).GetComponent<Infographic_Buttom>());
            buttoms[i].infoTab = this;
        }
    }

    public void HideAll(Infographic_Buttom exception = null)
    {
        foreach (var item in buttoms)
        {
            if(item != exception)
            {
                item.Hide();
            }
        }
    }

    //Atualiza os gráficos
    public void Refresh()
    {
        foreach (var item in buttoms)
        {
            if (item.Ativo)
                item.CallInfografico();
        }
    }

    private void OnEnable()
    {
        Refresh();
    }
}
