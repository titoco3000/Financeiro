using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/*
 Toda vez que o programa abre, garante que exista um backup atualizado dos arquivos
     */
public class BackupManager : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Backup());
    }
    IEnumerator Backup()
    {
        //Certifica que as pastas existem
        if (!Directory.Exists(StorageManager.Path+"Backup/fornecedores"))
                Directory.CreateDirectory(StorageManager.Path + "Backup/fornecedores");
        if (!Directory.Exists(StorageManager.Path+"Backup/compras"))
                Directory.CreateDirectory(StorageManager.Path + "Backup/compras");


        System.DateTime americamFormat = System.DateTime.Now;
        int AnoAtual = americamFormat.Year;
        string month = (americamFormat.Month < 10 ? "0" + americamFormat.Month.ToString() : americamFormat.Month.ToString());
        string data = americamFormat.Day + "_" + month + "_" + americamFormat.Year.ToString();

        //Verifica se já existe um backup nesse mesmo dia
        if (!File.Exists(StorageManager.Path + "Backup/compras/" + data + ".csv"))
        {
            //File.Create(StorageManager.Path + "Backup/compras/" + data + ".csv");
            //dá um delay para garantir que estejam criadas
            yield return new WaitForSeconds(1);
            File.Copy(StorageManager.Path + "compras.csv", StorageManager.Path + "Backup/compras/" + data + ".csv");
        }
        if (!File.Exists(StorageManager.Path + "Backup/fornecedores/" + data + ".csv"))
        {
            //File.Create(StorageManager.Path + "Backup/fornecedores/" + data + ".csv");
            //dá um delay para garantir que estejam criadas
            yield return new WaitForSeconds(1);
         File.Copy(StorageManager.Path + "fornecedores.csv", StorageManager.Path + "Backup/fornecedores/" + data + ".csv");
        }


        //Com o backup do dia criado, verifica se há algum backup velho demais

        foreach (var item in Directory.GetFiles(StorageManager.Path + "Backup/fornecedores"))
        {
            if (item.Contains((AnoAtual - 1).ToString()))
            {
                File.Delete(item);
            }
        }
        foreach (var item in Directory.GetFiles(StorageManager.Path + "Backup/compras"))
        {
            if (item.Contains((AnoAtual - 1).ToString()))
            {
                File.Delete(item);
            }
        }

    }

}
