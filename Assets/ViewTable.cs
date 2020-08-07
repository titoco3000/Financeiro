using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTable : Tabela
{
    public GameObject ConfirmationDialog;

    private int currentSelectedRow = -1;


    private void Awake()
    {
        //prepara a tabela com titulos
        List<Tabela.Coluna> colunas = new List<Tabela.Coluna>();
        colunas.Add(new Tabela.Coluna("Data", Tabela.TipoDeConteudo.Data, 4.5f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 7));
        colunas.Add(new Tabela.Coluna("Área", Tabela.TipoDeConteudo.String, 13));
        colunas.Add(new Tabela.Coluna("Valor", Tabela.TipoDeConteudo.Float, 5));
        colunas.Add(new Tabela.Coluna("Método", Tabela.TipoDeConteudo.String, 7));
        colunas.Add(new Tabela.Coluna("NF", Tabela.TipoDeConteudo.String, 3));
        colunas.Add(new Tabela.Coluna("Caixa", Tabela.TipoDeConteudo.String, 5));
        colunas.Add(new Tabela.Coluna("Obs", Tabela.TipoDeConteudo.String, 7));
        colunas.Add(new Tabela.Coluna("", Tabela.TipoDeConteudo.Button,3));


        Preparar(colunas);

    }
    private void OnEnable()
    {
        Refresh();
    }

    public override void ButtomMethod(int i)
    {
        currentSelectedRow = i;
        ConfirmationDialog.SetActive(true);
    }

    public void Recusar()
    {
        ConfirmationDialog.SetActive(false);
    }
    public void Aceitar()
    {
        ConfirmationDialog.SetActive(false);
        string Fornecedor = tableContent[currentSelectedRow][1].Value;
        int nf = int.Parse(tableContent[currentSelectedRow][5].Value);
        //Debug.Log("Vou excluir: " + Fornecedor + ", "+nf );
        StorageManager.RemoverCompra(Fornecedor, nf);

        Refresh();
    }

    public string NF(int n)
    {
        string txt = n.ToString();
        while (txt.Length < 5)
        {
            txt = "0" + txt;
        }
        return txt;
    }

    public void Refresh()
    {
        //load a lista de compras
        List<Compra> compras = StorageManager.Compras();

        //prepara o conteudo da tabela
        List<List<Tabela.Conteudo>> tableContent = new List<List<Tabela.Conteudo>>();

        //adiciona os outros valores
        foreach (Compra compra in compras)
        {
            List<Tabela.Conteudo> linha = new List<Tabela.Conteudo>();
            linha.Add(new Tabela.Conteudo(compra.Data));
            linha.Add(new Tabela.Conteudo(compra.Fornecedor));
            linha.Add(new Tabela.Conteudo(compra.Area.ToString()));
            linha.Add(new Tabela.Conteudo(compra.Valor, Tabela.TipoDeConteudo.Dinheiro));
            linha.Add(new Tabela.Conteudo(compra.Pagamento.ToString()));
            linha.Add(new Tabela.Conteudo(NF(compra.NotaFiscal)));
            linha.Add(new Tabela.Conteudo(compra.Banco.ToString()));
            linha.Add(new Tabela.Conteudo(compra.Obs));
            linha.Add(new Tabela.Conteudo());
            tableContent.Add(linha);
        }

        Desenhar(tableContent);
    }

}
