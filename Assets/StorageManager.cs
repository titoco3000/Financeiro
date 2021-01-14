using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/*
 Salva e retorna os dados dos fornecedores e das
 compras
     */
public static class StorageManager
{
    public static string Path = Application.persistentDataPath + "/storage/";



    public static List<Fornecedor> Fornecedores()
    {
        List<Fornecedor> retorno = new List<Fornecedor>();

        string strCompleta = File.ReadAllText(Path + "Fornecedores.csv");

        string[] linhas = strCompleta.Split("\n"[0]);

        //para cada linha, criar um novo fornecedor na lista
        foreach (var linha in linhas)
        {
            if (linha != "" && linha != "\n" && linha != " ")
            {
                string[] lineData = (linha.Trim()).Split(","[0]);
                retorno.Add(new Fornecedor(
                    lineData[0],
                    (AreasDoHotel)System.Enum.Parse(typeof(AreasDoHotel), lineData[1]),
                    (FormaDePagamento)System.Enum.Parse(typeof(FormaDePagamento), lineData[2]),
                    (Caixa)System.Enum.Parse(typeof(Caixa), lineData[3])
                    ));

            }
        }

        return retorno;
    }
    public static List<Compra> Compras()
    {
        List<Compra> retorno = new List<Compra>();

        string strCompleta = File.ReadAllText(Path + "Compras.csv");

        string[] linhas = strCompleta.Split("\n"[0]);

        //para cada linha, criar uma nova compra na lista
        foreach (var linha in linhas)
        {
            if (linha != "" && linha != "\n" && linha != " ")
            {
                string[] lineData = (linha.Trim()).Split(","[0]);
                retorno.Add(new Compra(
                    lineData[0],
                    (AreasDoHotel)System.Enum.Parse(typeof(AreasDoHotel), lineData[1]),
                    lineData[2],
                    float.Parse(lineData[3], System.Globalization.CultureInfo.InvariantCulture),
                    (FormaDePagamento)System.Enum.Parse(typeof(FormaDePagamento), lineData[4]),
                    int.Parse(lineData[5]),
                    (Caixa)System.Enum.Parse(typeof(Caixa), lineData[6]),
                    lineData[7]
                    )); ;

            }
        }

        return retorno;
    }


    public static void SalvarCompra(Compra novaCompra)
    {        
        //trata a nova compra
        string novaLinha = novaCompra.Fornecedor + "," + novaCompra.Area.ToString() + "," + novaCompra.Data + "," + novaCompra.Valor.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(",",".") + ","+novaCompra.Pagamento.ToString()+","+novaCompra.NotaFiscal+","+novaCompra.Banco+","+novaCompra.Obs ;
        //carrega as compras anteriores
        string arquivo = File.ReadAllText(Path + "compras.csv");
        //adiciona a nova compra
        arquivo += "\n" + novaLinha;
        //salva o arquivo
        File.WriteAllText(Path + "compras.csv", arquivo);
    }

    public static void RemoverCompra(string fornecedor, int nf)
    {
        List<Compra> compras = Compras();
        foreach (var item in compras)
        {
            if(item.Fornecedor == fornecedor && item.NotaFiscal == nf)
            {
                compras.Remove(item);
                SalvarCompras(compras);
                return;
            }
        }
    }

    public static void SalvarCompras(List<Compra> compras)
    {
        string txt = "";
        foreach (var novaCompra in compras)
        {
            txt = txt+ "\n" + novaCompra.Fornecedor + "," + novaCompra.Area.ToString() + "," + novaCompra.Data + "," + novaCompra.Valor.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(",",".") + "," + novaCompra.Pagamento.ToString() + "," + novaCompra.NotaFiscal + "," + novaCompra.Banco + "," + novaCompra.Obs;
        }
        //salva o arquivo
        File.WriteAllText(Path + "compras.csv", txt);

    }

    public static void SalvarFornecedores(List<Fornecedor> fornecedores)
    {
        string doc = "";
        foreach (var item in fornecedores)
        {
            doc += item.Nome + "," + item.Area+","+item.pagamentoPreferido + ","+item.bancoPreferido + "\n";
        }

        File.WriteAllText(Path + "fornecedores.csv", doc);
    }

    private static void EnsureFileExists(string filePath,string fileName)
    {
        if (!File.Exists(filePath+"/"+fileName))
        {
            if (!Directory.Exists(filePath.Substring(0,filePath.Length - 1))){
                Directory.CreateDirectory(Path.Substring(0,filePath.Length - 1));
                Debug.Log("Created dir");
            }

            File.Create(filePath + fileName);
        }


    }

    public static void EnsureAllNeededFileExists()
    {
        EnsureFileExists(Path, "compras.csv");
        EnsureFileExists(Path, "fornecedores.csv");
    }

}
