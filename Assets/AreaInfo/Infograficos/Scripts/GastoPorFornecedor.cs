using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GastoPorFornecedor : Infographic
{
    public Tabela3 table;

    public override void Setup(List<Compra> compras)
    {

        List<string> fornecedores = new List<string>();
        List<float> gastoTotal = new List<float>();
        List<int> numeroDeCompras = new List<int>();

        foreach (Compra item in compras)
        {
            int index = fornecedores.IndexOf(item.Fornecedor);
            if(index >= 0)
            {
                gastoTotal[index] += item.Valor;
                gastoTotal[index] = (float)System.Math.Round(gastoTotal[index], 2);
                numeroDeCompras[index] += 1;
            }
            else
            {
                fornecedores.Add(item.Fornecedor);
                gastoTotal.Add(item.Valor);
                numeroDeCompras.Add(1);
            }
        }


        //prepara o conteudo da tabela
        List<List<Tabela3.TableItem>> tableContent = new List<List<Tabela3.TableItem>>();

        //adiciona os outros valores
        for (int i = 0; i < fornecedores.Count; i++)
        {
            List<Tabela3.TableItem> linha = new List<Tabela3.TableItem>();
            linha.Add(new Tabela3.TableItem(fornecedores[i]));
            linha.Add(new Tabela3.TableItem(Ultilities.Money(gastoTotal[i])));
            linha.Add(new Tabela3.TableItem(numeroDeCompras[i]));

            tableContent.Add(linha);
        }
        table.Desenhar(tableContent);
    }
}

/*        private List<Gasto> GastosNoPeriodo;

    public Tabela table;

    public class Gasto
    {
        public string Fornecedor;
        public float Valor;
        public int Quantidade;


        public Gasto(string f,float v)
        {
            Fornecedor = f;
            Valor = v;
            Quantidade = 1;
        }
    }

    public void Start()
    {
        List<Tabela.Column> colunas = new List<Tabela.Column>();
        colunas.Add(new Tabela.Column("Fornecedor", 3f, Tabela.ColumnTypes.String));
        colunas.Add(new Tabela.Column("Valor", 3f, Tabela.ColumnTypes.Float));
        colunas.Add(new Tabela.Column("Quantidade", 3f, Tabela.ColumnTypes.Float));
        table.Preparar(colunas);
    }

    private List<Tabela.Column>  Preparation()
    {
        List<Tabela.Column> colunas = new List<Tabela.Column>();
        colunas.Add(new Tabela.Column("Fornecedor", 3f, Tabela.ColumnTypes.String));
        colunas.Add(new Tabela.Column("Valor", 3f, Tabela.ColumnTypes.Float));
        colunas.Add(new Tabela.Column("Quantidade", 3f, Tabela.ColumnTypes.Float));
        table.Preparar(colunas);
        return colunas;
    }

    public override void Setup(List<Compra> compras)
    {
        if (!table.preparado)
            table.Preparar(Preparation());

        GastosNoPeriodo = new List<Gasto>();
        foreach (var compra in compras)
        {
            int index = Index(GastosNoPeriodo, compra.Fornecedor);
            if(index == -1)
            {
                GastosNoPeriodo.Add(new Gasto(compra.Fornecedor, compra.Valor));
            }
            else
            {
                GastosNoPeriodo[index].Valor += compra.Valor;
                GastosNoPeriodo[index].Quantidade += 1;
            }
        }

        Debug.Log(GastosNoPeriodo.Count());
        
        List<List<Tabela.TableItem>> content = new List<List<Tabela.TableItem>>();

        foreach (var item in GastosNoPeriodo)
        {
            Debug.Log("Adicionando " + item.Fornecedor);
            List<Tabela.TableItem> row = new List<Tabela.TableItem>();
            row.Add(new Tabela.TableItem(item.Fornecedor));
            row.Add(new Tabela.TableItem(item.Valor));
            row.Add(new Tabela.TableItem(item.Quantidade));

            content.Add(row);
        }

        table.Desenhar(content);
        //Draw(GastosNoPeriodo);
    
    }

    private int Index(List<Gasto> gastos, string identifier)
    {
        List<Gasto> achados = gastos.FindAll(s => s.Fornecedor.Equals(identifier));
        if (achados.Count == 0)
            return -1;
        return gastos.IndexOf(achados[0]);
    }


}

    */
