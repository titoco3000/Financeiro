using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditarFornecedor : MonoBehaviour
{
    public Input_FornecedorConhecido original;
    public InputField novoNome;

    public Text mensagem;

    public GameObject[] Botoes;

    public List<Compra> compras;

    private void Start()
    {
        StartCoroutine(ChangeAfter());
        compras = StorageManager.Compras();
    }

    public void OnSubjectChanged()
    {
        StartCoroutine(ChangeAfter());
    }
    IEnumerator ChangeAfter()
    {
        yield return new WaitForSeconds(0.14f);
        novoNome.text = original.field.text;
        foreach (var item in Botoes)
        {
            item.SetActive(false);
        }
    }

    public void OnNewNameEdit()
    {
        novoNome.text = novoNome.text.Replace(",", "");

        bool deveAtivar = (novoNome.text != original.field.text && novoNome.text!="");
        foreach (var item in Botoes)
        {
            item.SetActive(deveAtivar);
        }
    }

    public void Cancelar()
    {
        if (!mensagem.gameObject.transform.parent.gameObject.activeSelf)
            novoNome.text = original.field.text;
        else
            mensagem.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void Confirmar()
    {
        mensagem.gameObject.transform.parent.gameObject.SetActive(true);
        //verifica dependencias do novo nome

        //quantas compras vão ser modificadas?
        int numDeCompras = 0;
        foreach (var item in compras)
        {
            if (item.Fornecedor == original.field.text)
                numDeCompras++;
        }
        //vai ser integrado a outro fornecedor?
        bool jaExiste = original.ValoresASeremProcurados.Contains(novoNome.text);

        //coloca a mensagem
        if (jaExiste)
        {
            mensagem.text = "Juntar as " + numDeCompras + " compras de \"" + original.field.text + "\" com \"" + novoNome.text + "\" ?";
        }
        else
        {
            mensagem.text = "Mudar \"" + original.field.text + "\" e suas " + numDeCompras + " compras para \"" + novoNome.text + "\" ?";
        }

        


    }

    public void ConfirmarDefinitivo()
    {
        //união
        if (original.ValoresASeremProcurados.Contains(novoNome.text))
        {
            FundirFornecedores(original.field.text,novoNome.text);
        }
        //edição
        else
        {
            MudarNome(original.field.text,novoNome.text);
        }

        mensagem.gameObject.transform.parent.gameObject.SetActive(false);
        original.ValoresASeremProcurados[original.ValoresASeremProcurados.IndexOf(original.field.text)] = novoNome.text;
        original.field.text = novoNome.text;

        foreach (var item in Botoes)
        {
            item.SetActive(false);
        }

        original.EditarValores(StorageManager.Fornecedores());
    }

    
    private void FundirFornecedores(string nomeOriginal, string novoNome)
    {
        EditarCompras(nomeOriginal, novoNome);
        
        List<Fornecedor> fornecedores = StorageManager.Fornecedores();
        foreach (var item in fornecedores)
        {
            if (item.Nome == nomeOriginal)
            {
                fornecedores.Remove(item);
                return;
            }
        }
    }
    private void MudarNome(string nomeOriginal, string novoNome)
    {
        List<Fornecedor> fornecedores = StorageManager.Fornecedores();
        foreach (var item in fornecedores)
        {
            if (item.Nome == nomeOriginal)
            {
                //edita o fornecedor
                item.Nome = novoNome;

                StorageManager.SalvarFornecedores(fornecedores);

                EditarCompras(nomeOriginal,novoNome);

                original.EditarValores(fornecedores);

                return;
            }
        }

    }


    private void EditarCompras(string nomeOriginal, string novoNome)
    {
        foreach (var c in compras)
        {
            if (c.Fornecedor == nomeOriginal)
            {
                c.Fornecedor = novoNome;
            }
        }
        StorageManager.SalvarCompras(compras);
    }
}
