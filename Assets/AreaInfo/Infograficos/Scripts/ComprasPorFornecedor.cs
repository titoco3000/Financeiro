using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComprasPorFornecedor : Infographic
{
    public Input_FornecedorConhecido input;
    public Tabela3 table;
    List<Compra> comprasPassiveis;

    bool jaExistia = false;

    public override void Setup(List<Compra> compras)
    {
        base.Setup(compras);
        comprasPassiveis = compras;
        StartCoroutine(Desenhar());

    }

    public IEnumerator Desenhar()
    {
        yield return 0;

        if (jaExistia)
        {
            print("mudando fornecedor");
            PlayerPrefs.SetString("FornecedorCpF", input.field.text);
        }
        else
        {
            print("mudando outra coisa");
            string lastName = PlayerPrefs.GetString("FornecedorCpF");
            if (input.ValoresASeremProcurados.Contains(lastName))
            {
                print("Possuia o nome " + lastName);
                input.field.text = lastName;
            }
            else
            {
                print("Não possuia o nome " + lastName);
            }
        }

        jaExistia = true;

        //PlayerPrefs.SetString("FornecedorCpF", input.field.text);

        //prepara o conteudo da tabela
        List<List<Tabela3.TableItem>> tableContent = new List<List<Tabela3.TableItem>>();

        foreach (Compra item in comprasPassiveis)
        {
            if (item.Fornecedor == input.field.text)
            {
                List<Tabela3.TableItem> linha = new List<Tabela3.TableItem>();
                linha.Add(new Tabela3.TableItem(item.Data));
                linha.Add(new Tabela3.TableItem(item.Area.ToString()));
                linha.Add(new Tabela3.TableItem(Ultilities.Money(item.Valor)));
                linha.Add(new Tabela3.TableItem(item.Pagamento.ToString()));
                linha.Add(new Tabela3.TableItem(NF(item.NotaFiscal)));
                linha.Add(new Tabela3.TableItem(item.Banco.ToString()));
                linha.Add(new Tabela3.TableItem(item.Obs));

                tableContent.Add(linha);
            }
        }
        table.Desenhar(tableContent);

    }
    public string NF(int n)
    {
        string txt = n.ToString();
        while (txt.Length < 6)
        {
            txt = "0" + txt;
        }
        return txt;
    }
}

    /*
    public override void Setup(List<Compra> compras)
    {
        base.Setup(compras);
        if (!table.preparado)
        {
            //prepara a tabela com titulos
            List<Tabela.Column> colunas = new List<Tabela.Column>();

            colunas.Add(new Tabela.Column("Data", 2f, Tabela.ColumnTypes.Data));
            colunas.Add(new Tabela.Column("Área", 2f, Tabela.ColumnTypes.String));
            colunas.Add(new Tabela.Column("Valor", 2f, Tabela.ColumnTypes.Float));
            colunas.Add(new Tabela.Column("Método", 2f, Tabela.ColumnTypes.String));
            colunas.Add(new Tabela.Column("NF", 1f, Tabela.ColumnTypes.Float));
            colunas.Add(new Tabela.Column("Caixa", 2f, Tabela.ColumnTypes.String));
            colunas.Add(new Tabela.Column("Obs", 4f, Tabela.ColumnTypes.String));

            table.Preparar(colunas);
        }

        comprasNoPeriodo = compras;

    }

    public void Desenhar()
    {
        //prepara o conteudo da tabela
        List<List<Tabela.TableItem>> tableContent = new List<List<Tabela.TableItem>>();

        foreach (Compra item in comprasNoPeriodo)
        {
            if (item.Fornecedor == input.field.text)
            {
                List<Tabela.TableItem> linha = new List<Tabela.TableItem>();
                linha.Add(new Tabela.TableItem(item.Data));
                linha.Add(new Tabela.TableItem(item.Area.ToString()));
                linha.Add(new Tabela.TableItem(item.Valor));
                linha.Add(new Tabela.TableItem(item.Pagamento.ToString()));
                linha.Add(new Tabela.TableItem(item.NotaFiscal));
                linha.Add(new Tabela.TableItem(item.Banco.ToString()));
                linha.Add(new Tabela.TableItem(item.Obs));

                tableContent.Add(linha);
            }
        }
        table.Desenhar(tableContent);
    }
}

    /*
    //public Tabela tabela;

    public Input_FornecedorConhecido input;

    List<Compra> Compras;

    private void Awake()
    {/*
        input.OnEndEditEvent.AddListener(new UnityEngine.Events.UnityAction(Desenhar));

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

        //carrega as compras
        Compras = compras;

        //monta a tabela
        Desenhar();


    }

    public void Desenhar()
    {
        Debug.LogWarning("Unimplemented table");

        
        Debug.Log("Compras: " + Compras.Count);



        List<Compra> comprasDesseFornecedor = new List<Compra>();

        foreach (var item in Compras)
        {
            if (item.Fornecedor == input.field.text)
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
}*/
