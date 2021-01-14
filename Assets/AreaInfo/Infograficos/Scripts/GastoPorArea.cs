using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GastoPorArea : Infographic
{
    public Tabela3 table;

    public override void Setup(List<Compra> compras)
    {
        base.Setup(compras);

        List<string> areas = new List<string>();
        List<float> gastoTotal = new List<float>();
        List<int> numeroDeCompras = new List<int>();

        foreach (Compra item in compras)
        {
            int index = areas.IndexOf(item.Area.ToString());
            if (index >= 0)
            {
                gastoTotal[index] += item.Valor;
                gastoTotal[index] = (float)System.Math.Round(gastoTotal[index], 2);
                numeroDeCompras[index] += 1;
            }
            else
            {
                areas.Add(item.Area.ToString());
                gastoTotal.Add(item.Valor);
                numeroDeCompras.Add(1);
            }
        }


        //prepara o conteudo da tabela
        List<List<Tabela3.TableItem>> tableContent = new List<List<Tabela3.TableItem>>();

        //adiciona os outros valores
        for (int i = 0; i < areas.Count; i++)
        {
            List<Tabela3.TableItem> linha = new List<Tabela3.TableItem>();
            linha.Add(new Tabela3.TableItem(areas[i]));
            linha.Add(new Tabela3.TableItem(Ultilities.Money(gastoTotal[i])));
            linha.Add(new Tabela3.TableItem(numeroDeCompras[i]));

            tableContent.Add(linha);
        }
        table.Desenhar(tableContent);
    }



    //public Tabela Tabela;

    //só roda uma vez
    /*private void Awake()
    {
        //prepara a tabela com titulos
        List<Tabela.Coluna> colunas = new List<Tabela.Coluna>();
        colunas.Add(new Tabela.Coluna("Área", Tabela.TipoDeConteudo.String, 10));
        colunas.Add(new Tabela.Coluna("Valor", Tabela.TipoDeConteudo.Float, 10));
       
        Tabela.Preparar(colunas);
        
    }*/

    /*public override void Setup(List<Compra> compras)
    {
        Debug.LogWarning("Unimplemented table");
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
            tableContent[(int)compra.Area][1].Value = (float.Parse(tableContent[(int)compra.Area][1].Value, System.Globalization.CultureInfo.InvariantCulture) + compra.Valor).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        Tabela.Desenhar(tableContent);*/
    //}

}
