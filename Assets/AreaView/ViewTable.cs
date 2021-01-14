using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ViewTable : MonoBehaviour
{
    public Tabela3 table;
    public GameObject confirmationUI;
    public Formulario form;

    private int L;

    private string lastName;
    private int lastNF;

    public void ExcluirCompra(int coluna, int linha)
    {
        Debug.Log("Excluir compra");
        table.HighLight(linha);
        if (linha < table.tableContent.Count)
        {
            L = linha;
            confirmationUI.SetActive(true);
        }
    }

    public void OnEnable()
    {
        RefreshTable();
    }

    public void RefreshItem(Compra compra)
    {
        for (int i = 0; i < table.tableContent.Count; i++)
        {
            if(table.tableContent[i][1].Value == compra.Fornecedor && int.Parse(table.tableContent[i][5].Value) == compra.NotaFiscal)
            {
                
                List<Tabela3.TableItem> linha = new List<Tabela3.TableItem>();
                linha.Add(new Tabela3.TableItem(compra.Data));
                linha.Add(new Tabela3.TableItem(compra.Fornecedor));
                linha.Add(new Tabela3.TableItem(compra.Area.ToString()));
                linha.Add(new Tabela3.TableItem(Ultilities.Money(compra.Valor)));
                linha.Add(new Tabela3.TableItem(compra.Pagamento.ToString()));
                linha.Add(new Tabela3.TableItem(Ultilities.NF(compra.NotaFiscal)));
                linha.Add(new Tabela3.TableItem(compra.Banco.ToString()));
                linha.Add(new Tabela3.TableItem(compra.Obs));
                table.tableContent[i] = linha;


                float currentScroll = table.bar.value;
                table.Desenhar(table.tableContent);
                table.bar.value = currentScroll;
                table.OnScroll(table.bar.value);
                return;
            }
        }
    }

    public void RefreshTable()
    {
        //load a lista de compras
        List<Compra> compras = StorageManager.Compras();

        //prepara o conteudo da tabela
        List<List<Tabela3.TableItem>> tableContent = new List<List<Tabela3.TableItem>>();

        //adiciona os outros valores
        foreach (Compra compra in compras)
        {
            List<Tabela3.TableItem> linha = new List<Tabela3.TableItem>();
            linha.Add(new Tabela3.TableItem(compra.Data));
            linha.Add(new Tabela3.TableItem(compra.Fornecedor));
            linha.Add(new Tabela3.TableItem(compra.Area.ToString()));
            linha.Add(new Tabela3.TableItem(Ultilities.Money(compra.Valor)));
            linha.Add(new Tabela3.TableItem(compra.Pagamento.ToString()));
            linha.Add(new Tabela3.TableItem(Ultilities.NF(compra.NotaFiscal)));
            linha.Add(new Tabela3.TableItem(compra.Banco.ToString()));
            linha.Add(new Tabela3.TableItem(compra.Obs));

            tableContent.Add(linha);
        }
        table.Desenhar(tableContent);
    }

    public void ConfirmarExclusao()
    {
        confirmationUI.SetActive(false);
        string Nome = table.tableContent[L][1].Value;
        int NF = int.Parse(table.tableContent[L][5].Value);

        StorageManager.RemoverCompra(Nome, NF);

        table.tableContent.RemoveAt(L);
        table.PopulateFrom(table.firstVisibleIndex);
    }
    public void CancelarExclusao()
    {
        table.HighLight(-1);
        confirmationUI.SetActive(false);
    }

    public void EditarCompra(int coluna, int linha)
    {
        L = linha;
        form.gameObject.SetActive(true);
        lastName = table.tableContent[linha][1].Value;
        lastNF = int.Parse(table.tableContent[linha][5].Value);
        List<Compra> compras = StorageManager.Compras();
        form.Preencher(compras[Compra.LocateInList(compras, lastName, lastNF)]);
    }


    

}
    