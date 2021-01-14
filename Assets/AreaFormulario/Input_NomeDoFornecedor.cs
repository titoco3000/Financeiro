using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Carrega a lista de fornecedores para gerenciar o autocomplete
     */
public class Input_NomeDoFornecedor : AutocompleteField
{
    public GameObject AvisoDeNovoFornecedor;

    //campos dependentes
    public Input_Area areaField;
    public Dropdown bancoField;
    public Dropdown metodoField;


    
    private List<Fornecedor> Fornecedores;




    public void Start()
    {
        //carrega a lista ao iniciar
        StartCoroutine(WaitASec());

        AvisoDeNovoFornecedor.SetActive(false);
        OnEndEditEvent.AddListener(new UnityEngine.Events.UnityAction(GoOnEndEdit));
    }

    IEnumerator WaitASec()
    {
        ValoresASeremProcurados = new List<string>();
        yield return new WaitForSeconds(0.2f);
        
        Fornecedores = StorageManager.Fornecedores();
        foreach (var item in Fornecedores)
        {
            ValoresASeremProcurados.Add(item.Nome);
        } 
    }

    public void GoOnEndEdit()
    {
        //verifica se o nome atual existe na lista de fornecedores
        //se não, aciona o aviso de "Novo fornecedor"
        if(!ValoresASeremProcurados.Contains(field.text))
        {
            Debug.Log(field.text + " is not na lista");
            AvisoDeNovoFornecedor.SetActive(true);
        }
    }

    public void AvisoDeFornecedor(AreasDoHotel area, FormaDePagamento p, Caixa b) {
        //verifica se o fornecedor existe na lista
        int local = Fornecedor.LocateInList(Fornecedores, field.text);
        if (local != -1)
        {
            //Atualiza as informações dele para as atuais
            Fornecedores[local].Area = area;
            Fornecedores[local].pagamentoPreferido = p;
            Fornecedores[local].bancoPreferido = b;
        }
        else
        {
            Debug.Log("Fornecedor não existe, adicionando ele");

            //Adiciona ele a lista
            Fornecedores.Add(new Fornecedor(field.text, area,p,b));
            ValoresASeremProcurados.Add(field.text);
        }
        StorageManager.SalvarFornecedores(Fornecedores);

        AvisoDeNovoFornecedor.SetActive(false);

    }

    public override void Autocompletar(string valor)
    {
        base.Autocompletar(valor);

        //atualiza campos dependentes
        areaField.SetValue((int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].Area);
        bancoField.value = (int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].bancoPreferido;
        metodoField.value = (int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].pagamentoPreferido;

        //seleciona o próximo campo
        GetComponent<SkipToNextField>().NextField.Select();
    }

}
