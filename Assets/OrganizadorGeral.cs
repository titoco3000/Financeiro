using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 organiza qual aba deve estar aberta

*/
public class OrganizadorGeral : MonoBehaviour
{
    public Transform abasAtras;
    public Transform abasNaFrente;

    public GameObject AreaDoFormulario;
    public GameObject AreaDoInfo;
    public GameObject AreaDoView;
    public GameObject AreaDoTools;

    public Image AbaFormulario;
    public Image AbaInfo;
    public Image AbaView;
    public Image AbaTools;

    public Color corDaAbaSelecionada;
    public Color corDaAbaOculta;


    private void Start()
    {
        //general configs
        StorageManager.EnsureAllNeededFileExists();    
        Screen.fullScreen = false;
    }

    public void RevelarFormulario()
    {
        //esconde os outros
        AreaDoInfo.SetActive(false);
        AreaDoView.SetActive(false);
        AreaDoTools.SetActive(false);
        AbaTools.color = corDaAbaOculta;
        AbaInfo.color = corDaAbaOculta;
        AbaView.color = corDaAbaOculta;
        AbaInfo.transform.SetParent( abasAtras);
        AbaView.transform.SetParent( abasAtras);
        AbaTools.transform.SetParent( abasAtras);

        //revela o Formulario
        AreaDoFormulario.SetActive(true);
        AbaFormulario.color = corDaAbaSelecionada;
        AbaFormulario.transform.SetParent( abasNaFrente);

    }
    public void RevelarInfo()
    {
        //esconde os outros
        AreaDoFormulario.SetActive(false);
        AreaDoView.SetActive(false);
        AreaDoTools.SetActive(false);
        AbaTools.color = corDaAbaOculta;
        AbaFormulario.color = corDaAbaOculta;
        AbaView.color = corDaAbaOculta;
        AbaFormulario.transform.SetParent( abasAtras);
        AbaView.transform.SetParent( abasAtras);
        AbaTools.transform.SetParent( abasAtras);

        //revela o Info
        AreaDoInfo.SetActive(true);
        AbaInfo.color = corDaAbaSelecionada;
        AbaInfo.transform.SetParent( abasNaFrente);
    }
    public void RevelarView()
    {
        //esconde os outros
        AreaDoInfo.SetActive(false);
        AreaDoFormulario.SetActive(false);
        AreaDoTools.SetActive(false);
        AbaTools.color = corDaAbaOculta;
        AbaInfo.color = corDaAbaOculta;
        AbaFormulario.color = corDaAbaOculta;
        AbaFormulario.transform.SetParent( abasAtras);
        AbaInfo.transform.SetParent( abasAtras);
        AbaTools.transform.SetParent( abasAtras);


        //revela o View
        AreaDoView.SetActive(true);
        AbaView.color = corDaAbaSelecionada;
        AbaView.transform.SetParent( abasNaFrente);


    }
    public void RevelarTools()
    {
        //esconde os outros
        AreaDoInfo.SetActive(false);
        AreaDoFormulario.SetActive(false);
        AreaDoView.SetActive(false);
        AbaView.color = corDaAbaOculta;
        AbaInfo.color = corDaAbaOculta;
        AbaFormulario.color = corDaAbaOculta;
        AbaFormulario.transform.SetParent( abasAtras);
        AbaInfo.transform.SetParent( abasAtras);
        AbaView.transform.SetParent( abasAtras);

        //revela o View
        AreaDoTools.SetActive(true);
        AbaTools.color = corDaAbaSelecionada;
        AbaTools.transform.SetParent( abasNaFrente);
    }

    public void AbrirDirectory()
    {
        System.Diagnostics.Process.Start(StorageManager.Path);
    }

}
