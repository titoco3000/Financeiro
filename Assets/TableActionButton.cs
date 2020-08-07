using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableActionButton : MonoBehaviour
{
    public Tabela Tabela;
    public void Metodo()
    {
        Tabela.ButtomMethod(int.Parse(this.name));
    }
}
