using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Ativa e desativa o infografico correspondente
     */
public class Infographic_Buttom : MonoBehaviour
{
    public GameObject prefab;
    public InfoTab infoTab;

    private Infographic infografico;
    public Transform WhereToPlace;

    public bool Ativo = false;
    public void CallInfografico()
    {
        infoTab.HideAll();
        Ativo = true;
        if(infografico == null)
        {
            if(prefab != null)
                infografico = Instantiate(prefab,WhereToPlace).GetComponent<Infographic>();
        }
        else
        {
            infografico.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        Ativo = false;
        if (infografico)
        {
            Destroy(infografico.gameObject);
            infografico = null;
        }
    }
}
