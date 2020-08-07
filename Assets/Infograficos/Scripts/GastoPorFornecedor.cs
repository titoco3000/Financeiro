using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GastoPorFornecedor : Infographic
{
        public GameObject linha;
        public RectTransform content;

        public Metodo metodoDeOrganizacao = Metodo.nome;

        private List<Gasto> GastosNoPeriodo;

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

    public override void Setup(List<Compra> compras)
    {
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
        
        Draw(GastosNoPeriodo);
    
    }

    private int Index(List<Gasto> gastos, string identifier)
    {
        List<Gasto> achados = gastos.FindAll(s => s.Fornecedor.Equals(identifier));
        if (achados.Count == 0)
            return -1;
        return gastos.IndexOf(achados[0]);
    }

    public void Draw(List<Gasto> gastos)
    {
        //arruma a lista
        if (metodoDeOrganizacao == Metodo.nome)
            gastos.Sort((a, b) => a.Fornecedor.CompareTo(b.Fornecedor));
        else if (metodoDeOrganizacao == Metodo.valor)
            gastos.Sort((a, b) => b.Valor.CompareTo(a.Valor));
        else if (metodoDeOrganizacao == Metodo.quantidade)
            gastos.Sort((a, b) => b.Quantidade.CompareTo(a.Quantidade));

        //limpa 
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        int numeroDeItems = 0;
        for (int i = 0; i < gastos.Count; i++)
        {
            GameObject novaLinha = Instantiate(linha, content);
            Text[] textos = novaLinha.GetComponentsInChildren<Text>();
            textos[0].text = gastos[i].Fornecedor;
            textos[1].text = gastos[i].Valor.ToString().Replace(".", ",") + " R$";
            textos[2].text = gastos[i].Quantidade.ToString();
            numeroDeItems++;
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, numeroDeItems * 33 + 10);
    }

    public enum Metodo
    {
        nome,
        valor,
        quantidade
    }

    public void ModificarMetodo(int novoMetodo)
    {
        metodoDeOrganizacao = (Metodo)novoMetodo;
        Draw(GastosNoPeriodo);
    }

}


