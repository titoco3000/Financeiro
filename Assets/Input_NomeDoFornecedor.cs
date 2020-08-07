using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Carrega a lista de fornecedores para gerenciar o autocomplete
     */
public class Input_NomeDoFornecedor : MonoBehaviour
{
    public InputField field;
    public GameObject AvisoDeNovoFornecedor;
    public RectTransform matchesList;
    public Transform contentGrid;

    //campos dependentes
    public Input_Area areaField;
    public Dropdown bancoField;
    public Dropdown metodoField;


    public GameObject listItemPrefab;

    private int maxSugestoes = 10;
    
    private List<Fornecedor> Fornecedores;
    private ScrollRect scroll;

    public float m = 0.0022222f;



    private void Start()
    {
        //carrega a lista ao iniciar
        StartCoroutine(WaitASec());

        scroll = matchesList.GetComponent<ScrollRect>();

        AvisoDeNovoFornecedor.SetActive(false);
    }

    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(0.5f);
        Fornecedores = StorageManager.Fornecedores();
    }

    public void OnFieldUpdate()
    {

        string textoFiltrado = field.text;
        textoFiltrado = textoFiltrado.Replace(";", "");
        textoFiltrado = textoFiltrado.Replace(",", "");

        //determina os fornecedores mais prováveis
        List<Fornecedor> maisProvaveis = MaisProvaveis(textoFiltrado);

        //Se tem algum, exibe a lista de possíveis
        if(maisProvaveis.Count > 0)
        {
            int sugestoes = Mathf.Clamp(maisProvaveis.Count, 0,  maxSugestoes);
            //limpa a lista
            foreach (Transform child in contentGrid)
            {
                Destroy(child.gameObject);
            }


            
            //volta para o começo da lista
            scroll.verticalNormalizedPosition = 1;

            //coloca a lista do tamanho certo
            int height = 21 * sugestoes ;
            float yPos = -(height/2) - 10 ;
            matchesList.sizeDelta = new Vector2(matchesList.rect.width, height );
            matchesList.localPosition = new Vector2(0, yPos);

            for (int i = 0; i < maisProvaveis.Count; i++)
            {
                //adiciona o item
                GameObject item = Instantiate(listItemPrefab, contentGrid);
                //configura o item
                item.GetComponentInChildren<Text>().text = maisProvaveis[i].Nome;
            }
            matchesList.gameObject.SetActive(true);
            
        }
        else
        {
            matchesList.gameObject.SetActive(false);
        }

        field.text = textoFiltrado;
    }

    IEnumerator MoveTextEnd_NextFrame()
    {
        yield return 0; // Skip the first frame in which this is called.
        field.MoveTextEnd(false); // Do this during the next frame.
    }

    public void OnStoppedEditing()
    {
        //verifica se o nome atual existe na lista de fornecedores
        //se não, aciona o aviso de "Novo fornecedor"
        

        //Verifica se o mouse está sobre a lista de sugestões
        if (!Mouse.IsHovering(matchesList.gameObject))
        {
            AvisoDeNovoFornecedor.SetActive(Fornecedor.LocateInList(Fornecedores,field.text) == -1);
            matchesList.gameObject.SetActive(false);
        }
    }

    public void AvisoDeFornecedor(AreasDoHotel area, FormaDePagamento p, Caixa b) {
        //verifica se o fornecedor existe na lista
        int local = Fornecedor.LocateInList(Fornecedores, field.text);
        if (local != -1)
        {
            //Atualiza as informações dele para as atuais
            Debug.Log("Fornecedor existe, atualizando ele");
            Fornecedores[local].Area = area;
            Fornecedores[local].pagamentoPreferido = p;
            Fornecedores[local].bancoPreferido = b;
        }
        else
        {
            Debug.Log("Fornecedor não existe, adicionando ele");

            //Adiciona ele a lista
            Fornecedores.Add(new Fornecedor(field.text, area,p,b));
        }
        StorageManager.SalvarFornecedores(Fornecedores);

        AvisoDeNovoFornecedor.SetActive(false);

    }

    //Retorna em ordem de semelhança com o texto atual
    public List<Fornecedor> MaisProvaveis(string textoAtual)
    {
        textoAtual = textoAtual.ToLower();
        List<Fornecedor> retorno = new List<Fornecedor>();
        if (textoAtual == "")
            return retorno;
        foreach (var item in Fornecedores)
        {
            if (item.Nome.ToLower().Contains(textoAtual))
            {
                retorno.Add(item);
            }
        }
        return retorno;
    }


    //ativado por items da lista de sugestões
    public void Autocompletar(string valor)
    {
        field.text = valor;
        matchesList.gameObject.SetActive(false);

        //atualiza campos dependentes
        areaField.SetValue( (int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].Area );
        bancoField.value = (int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].bancoPreferido;
        metodoField.value = (int)Fornecedores[Fornecedor.LocateInList(Fornecedores, valor)].pagamentoPreferido;

        //seleciona o próximo campo
        GetComponent<SkipToNextField>().NextField.Select();
    }
}
