using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 organiza qual aba deve estar aberta

*/
public class OrganizadorGeral : MonoBehaviour
{
    public GameObject AreaDoFormulario;
    public GameObject AreaDoInfo;
    public GameObject AreaDoView;

    public Image AbaFormulario;
    public Image AbaInfo;
    public Image AbaView;

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
        AbaInfo.color = corDaAbaOculta;
        AbaView.color = corDaAbaOculta;

        //revela o Formulario
        AreaDoFormulario.SetActive(true);
        AbaFormulario.color = corDaAbaSelecionada;
    }
    public void RevelarInfo()
    {
        //esconde os outros
        AreaDoFormulario.SetActive(false);
        AreaDoView.SetActive(false);
        AbaFormulario.color = corDaAbaOculta;
        AbaView.color = corDaAbaOculta;

        //revela o Info
        AreaDoInfo.SetActive(true);
        AbaInfo.color = corDaAbaSelecionada;
    }
    public void RevelarView()
    {
        //esconde os outros
        AreaDoInfo.SetActive(false);
        AreaDoFormulario.SetActive(false);
        AbaInfo.color = corDaAbaOculta;
        AbaFormulario.color = corDaAbaOculta;

        //revela o View
        AreaDoView.SetActive(true);
        AbaView.color = corDaAbaSelecionada;
    }

    public void AbrirDirectory()
    {
        System.Diagnostics.Process.Start(StorageManager.Path);
    }

}
