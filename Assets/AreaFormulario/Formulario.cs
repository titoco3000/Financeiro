using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Possui a função de todos os botões e envia todas as informações para serem salvas
 Avisa o input_NomeDoFornecedor sobre o novo fornecedor
*/
public class Formulario : MonoBehaviour
{
    public bool modoDeEdicao;
    public Input_NomeDoFornecedor NomeDoFornecedor;
    public Dropdown AreaDaCompra;
    public Input_Data DataDaCompra;
    public Input_Valor valorDaCompra;
    public Dropdown formaDePagamento;
    public InputField notaFiscal;
    public Dropdown banco;
    public InputField obs;
    public MensagemVolatil MensagemDeRecusa;

    public FichaDeCompra fichaDeConfirmacao;



    public void ConfirmarCompra()
    {
        //trata as informações que precisam
        
        //verifica se a data está correta
        if(!Data.IsValid(DataDaCompra.field.text))
        {
            RecusarInput("data incorreta");
            return;
        }

        //Verifica o nome do fornecedor
        if(NomeDoFornecedor.field.text == "")
        {
            RecusarInput("Nome do fornecedor vazio");
            return;
        }

        //Verifica se a nota fiscal já existe para esse fornecedor
        if (!modoDeEdicao)
        {
            List<Compra> compras = StorageManager.Compras();
            foreach (Compra compra in compras)
            {
                if (compra.Fornecedor == NomeDoFornecedor.field.text && compra.NotaFiscal == int.Parse(notaFiscal.text))
                {
                    RecusarInput("Nota fiscal já usada em " + compra.Data);
                    return;
                }
            }
        }
        //retira caracteres perigosos da observação
        obs.text = obs.text.Replace(",", "");

        //cria uma nova compra com as informações do formulario
        Compra novaCompra = new Compra(
            NomeDoFornecedor.field.text,
            (AreasDoHotel)AreaDaCompra.value,
            DataDaCompra.field.text,
            valorDaCompra.Valor(),
            (FormaDePagamento)formaDePagamento.value,
            int.Parse(notaFiscal.text),
            (Caixa)banco.value,
            obs.text
            );

        fichaDeConfirmacao.gameObject.SetActive(true);

        FichaDeCompra.Retorno r = new FichaDeCompra.Retorno();
        r.AddListener(CompraFoiVerificada);
        fichaDeConfirmacao.BuscarAprovacao(novaCompra, r);
    }


    public void CompraFoiVerificada(Compra novaCompra)
    {
        //envia para ser salva
        if (modoDeEdicao)
        {
            StorageManager.RemoverCompra(novaCompra.Fornecedor, novaCompra.NotaFiscal);
        }

        StorageManager.SalvarCompra(novaCompra);

        //envia o fornecedor para o campo
        NomeDoFornecedor.AvisoDeFornecedor((AreasDoHotel)AreaDaCompra.value, (FormaDePagamento)formaDePagamento.value, (Caixa)banco.value);

        //Limpa o formmulário
        NomeDoFornecedor.field.text = "";
        DataDaCompra.ResetData();
        valorDaCompra.field.text = "000";
        notaFiscal.text = "00000";
        obs.text = "";

        if (modoDeEdicao)
        {
            gameObject.SetActive(false);
            FindObjectOfType<ViewTable>().RefreshItem(novaCompra);
        }
        else
            MensagemDeRecusa.SetMessage("Compra aprovada", true);
    }

    private void RecusarInput(string msg)
    {
        MensagemDeRecusa.SetMessage(msg, false);
        Debug.Log("Recusado: "+ msg);
    }

    public void Preencher(Compra compra)
    {
        NomeDoFornecedor.field.text = compra.Fornecedor;
        AreaDaCompra.value = (int)compra.Area;
        DataDaCompra.field.text = compra.Data;
        valorDaCompra.field.text = Ultilities.Money(compra.Valor);
        formaDePagamento.value = (int)compra.Pagamento;
        notaFiscal.text = Ultilities.FormatarNF(compra.NotaFiscal);
        banco.value = (int)compra.Banco;
        obs.text = compra.Obs;
    }



}
