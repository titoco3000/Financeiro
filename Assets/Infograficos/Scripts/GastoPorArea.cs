using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GastoPorArea : Infographic
{
    public Tabela Tabela;

    //só roda uma vez
    private void Awake()
    {
        //prepara a tabela com titulos
        List<Tabela.Coluna> colunas = new List<Tabela.Coluna>();
        colunas.Add(new Tabela.Coluna("Área", Tabela.TipoDeConteudo.String, 10));
        colunas.Add(new Tabela.Coluna("Valor", Tabela.TipoDeConteudo.Float, 10));
       
        Tabela.Preparar(colunas);
        
    }

    public override void Setup(List<Compra> compras)
    {
        //prepara o conteudo da tabela
        List<List<Tabela.Conteudo>> tableContent = new List<List<Tabela.Conteudo>>();
        //adiciona todas as areas com valor 0$
        foreach (var item in System.Enum.GetNames(typeof(AreasDoHotel)))
        {
            List<Tabela.Conteudo> lista = new List<Tabela.Conteudo>();
            lista.Add(new Tabela.Conteudo(item));
            lista.Add(new Tabela.Conteudo(0f,Tabela.TipoDeConteudo.Dinheiro));
            tableContent.Add(lista);
        }

        //adiciona os outros valores
        foreach (Compra compra in compras)
        {
            tableContent[(int)compra.Area][1].Value = (float.Parse(tableContent[(int)compra.Area][1].Value) + compra.Valor).ToString();
        }

        Tabela.Desenhar(tableContent);
    }


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
