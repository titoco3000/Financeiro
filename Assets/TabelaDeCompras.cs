using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabelaDeCompras : MonoBehaviour
{
    public Tabela Tabela;

    public void Setup(List<Compra> compras)
    {
        List<Tabela.Coluna> colunas = new List<Tabela.Coluna>();
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Area", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Data", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));
        colunas.Add(new Tabela.Coluna("Fornecedor", Tabela.TipoDeConteudo.String, 10f));

        Tabela.Preparar();

        List<List<Tabela.Conteudo>> tableContent = new List<List<Tabela.Conteudo>>();
        foreach (var item in compras)
        {
            List<Tabela.Conteudo> LineContent = new List<Tabela.Conteudo>();
            LineContent.Add(new Tabela.Conteudo(item.Fornecedor));
            LineContent.Add(new Tabela.Conteudo(item.Area.ToString()));
            LineContent.Add(new Tabela.Conteudo(item.Data));
            LineContent.Add(new Tabela.Conteudo(item.Valor,Tabela.TipoDeConteudo.Dinheiro));
            LineContent.Add(new Tabela.Conteudo(item.Pagamento.ToString()));
            LineContent.Add(new Tabela.Conteudo(item.NotaFiscal.ToString()));
            LineContent.Add(new Tabela.Conteudo(item.Banco.ToString()));
            LineContent.Add(new Tabela.Conteudo(item.Obs));
            tableContent.Add(LineContent);
        }

        Tabela.Desenhar(tableContent);
    }

    public Color[] CorDasAreas;

    /*public override void Setup(List<Compra> compras)
    {
        int numeroDeAreas = System.Enum.GetValues(typeof(AreasDoHotel)).Length;
        
        float[] valores = new float[numeroDeAreas];
        foreach (Compra compra in compras)
        {
            valores[(int)compra.Area] += compra.Valor;
        }

        string[] nomes = new string[numeroDeAreas];
        for (int i = 0; i < numeroDeAreas; i++)
        {
            nomes[i] = System.Enum.GetNames(typeof(AreasDoHotel))[i];
        }

        pie.Draw(valores, nomes, CorDasAreas);
    }*/
}
