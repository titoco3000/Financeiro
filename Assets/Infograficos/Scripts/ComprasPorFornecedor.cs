using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComprasPorFornecedor : Infographic
{
    public Dropdown namesDropdown;
    public Tabela tabela;

    List<Compra> Compras;

    private void Awake()
    {
        //prepara a tabela com titulos
        List<Tabela.Coluna> colunas = new List<Tabela.Coluna>();
        colunas.Add(new Tabela.Coluna("Data", Tabela.TipoDeConteudo.Data, 3.5f));
        colunas.Add(new Tabela.Coluna("Área", Tabela.TipoDeConteudo.String, 6));
        colunas.Add(new Tabela.Coluna("Valor", Tabela.TipoDeConteudo.Float, 4));
        colunas.Add(new Tabela.Coluna("Método", Tabela.TipoDeConteudo.String, 5));
        colunas.Add(new Tabela.Coluna("NF", Tabela.TipoDeConteudo.String, 3));
        colunas.Add(new Tabela.Coluna("Caixa", Tabela.TipoDeConteudo.String, 5));
        colunas.Add(new Tabela.Coluna("Obs", Tabela.TipoDeConteudo.String, 7));


        tabela.Preparar(colunas);
    }

    public override void Setup(List<Compra> compras)
    {
        //salva a posição que estava no dropdown
        int posAnterior = namesDropdown.value;

        //limpa o dropdown
        namesDropdown.ClearOptions();
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();

        //carrega todos os fornecedores para o dropdown
        List<Fornecedor> fornecedores = StorageManager.Fornecedores();
        foreach (var item in fornecedores)
        {
            optionList.Add(new Dropdown.OptionData(item.Nome));
        }
        namesDropdown.AddOptions(optionList);

        while(posAnterior >= namesDropdown.options.Count)
        {
            posAnterior--;
        }

        namesDropdown.value = posAnterior;

        //carrega as compras
        Compras = compras;

        //monta a tabela
        Desenhar();


    }

    public void Desenhar()
    {
        List<Compra> comprasDesseFornecedor = new List<Compra>();

        foreach (var item in Compras)
        {
            if (item.Fornecedor == namesDropdown.options[namesDropdown.value].text)
            {
                comprasDesseFornecedor.Add(item);
            }
        }

        //prepara o conteudo da tabela
        List<List<Tabela.Conteudo>> tableContent = new List<List<Tabela.Conteudo>>();

        //adiciona os outros valores
        foreach (Compra compra in comprasDesseFornecedor)
        {
            List<Tabela.Conteudo> linha = new List<Tabela.Conteudo>();
            linha.Add(new Tabela.Conteudo(compra.Data));
            linha.Add(new Tabela.Conteudo(compra.Area.ToString()));
            linha.Add(new Tabela.Conteudo(compra.Valor, Tabela.TipoDeConteudo.Dinheiro));
            linha.Add(new Tabela.Conteudo(compra.Pagamento.ToString()));
            linha.Add(new Tabela.Conteudo(NF(compra.NotaFiscal)));
            linha.Add(new Tabela.Conteudo(compra.Banco.ToString()));
            linha.Add(new Tabela.Conteudo(compra.Obs));
            tableContent.Add(linha);
        }

        tabela.Desenhar(tableContent);


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
}
