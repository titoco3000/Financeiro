using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tabela3))]
public class Tabela3Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Tabela3 table = (Tabela3)target;


        if (GUILayout.Button("Gerar Tabela"))
        {
            table.ConstruirTabela();
        }
        base.OnInspectorGUI();
    }
}
