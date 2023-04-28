using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SistemaEtccom.Classes
{
    class cls_csv_ndd    
    {
        public struct Indexes
        {
            public int indexSerie;
            public int indexNumeroNota;
            public int indexChaveAcesso;
            public int indexRazaoSocial;
            public int indexDataEmissao;
            public int indexCnpjEmitente;
            public int indexCnpjDestinatario;
            public int indexCpfDestinatario;
            public int indexsRazaoSocialDest;
            public int indexUf;
            public int indexModelo;
            public int indexCodNumerico;
            public int indexNaturezaOperacao;
            public int indexIeEmitente;
            public int indexIeDestinatario;
            public int indexUfDestinatario;
            public int indexBaseIcms;
            public int indexBaseIcmsSt;
            public int indexTotalProdutos;
            public int indexValorIcms;
            public int indexValorIcmsSt;
            public int indexValorIpi;
            public int indexValorPis;
            public int indexValorCofins;
            public int indexBaseIss;
            public int indexCnpjTransportadora;
            public int indexPlacaTransportadora;
            public int indexTipoEmissao;
            public int indexStatusNfe;
            public int indexValorTotalNfe;
            public int indexPedidoCompra;
            public int indexUltimaIteracao;
            public int indexUltimoEvento;
        }

       public static Indexes SetColumnsIndex(string[] columns)
        {
            Indexes ind = new Indexes();
            ind.indexSerie = 0;
            ind.indexNumeroNota = -1;
            ind.indexChaveAcesso = -1;
            ind.indexRazaoSocial = -1;
            ind.indexDataEmissao = -1;
            ind.indexCnpjEmitente = -1;
            ind.indexCnpjDestinatario = -1;
            ind.indexCpfDestinatario = -1;
            ind.indexsRazaoSocialDest = -1;
            ind.indexUf = -1;
            ind.indexModelo = -1;
            ind.indexCodNumerico = -1;
            ind.indexNaturezaOperacao = -1;
            ind.indexIeEmitente = -1;
            ind.indexIeDestinatario = -1;
            ind.indexUfDestinatario = -1;
            ind.indexBaseIcms = -1;
            ind.indexBaseIcmsSt = -1;
            ind.indexTotalProdutos = -1;
            ind.indexValorIcms = -1;
            ind.indexValorIcmsSt = -1;
            ind.indexValorIpi = -1;
            ind.indexValorPis = -1;
            ind.indexValorCofins = -1;
            ind.indexBaseIss = -1;
            ind.indexCnpjTransportadora = -1;
            ind.indexPlacaTransportadora = -1;
            ind.indexTipoEmissao = -1;
            ind.indexStatusNfe = -1;
            ind.indexValorTotalNfe = -1;
            ind.indexPedidoCompra = -1;
            ind.indexUltimaIteracao = -1;
            ind.indexUltimoEvento = -1;


            for (int i = 0; i < columns.Length; i++)
            {
                if (string.IsNullOrEmpty(columns[i]))
                    continue;
                if (columns[i].Contains("Srie"))
                {
                    ind.indexSerie = i;
                }
                if (columns[i].Contains("mero da Nota"))
                {
                    ind.indexNumeroNota = i;
                }
                if (columns[i].Contains("Identificador da NF"))
                {
                    ind.indexChaveAcesso = i;
                }
                if (columns[i].Contains("o social do Emitente"))
                {
                    ind.indexRazaoSocial = i;
                }
                if (columns[i].Contains("Data de Emiss"))
                {
                    ind.indexDataEmissao = i;
                }
                if (columns[i].ToLower() == "cnpj do emitente")
                {
                    ind.indexCnpjEmitente = i;
                }
                if (columns[i].Contains("CNPJ do Destinat"))
                {
                    ind.indexCnpjDestinatario = i;
                }
                if (columns[i].Contains("CPF do Destinat"))
                {
                    ind.indexCpfDestinatario = i;
                }
                if (columns[i].Contains("o social do Destinat"))
                {
                    ind.indexsRazaoSocialDest = i;
                }
                if (columns[i].ToLower() == "uf")
                {
                    ind.indexUf = i;
                }
                if (columns[i].ToLower() == "modelo")
                {
                    ind.indexModelo = i;
                }
                if (columns[i].Contains("digo num"))
                {
                    ind.indexCodNumerico = i;
                }
                if (columns[i].Contains("Natureza da Opera"))
                {
                    ind.indexNaturezaOperacao = i;
                }
                if (columns[i].ToLower() == "ie do emitente")
                {
                    ind.indexIeEmitente = i;
                }
                if (columns[i].Contains("IE do Destinat"))
                {
                    ind.indexIeDestinatario = i;
                }
                if (columns[i].Contains("UF Destinat"))
                {
                    ind.indexUfDestinatario = i;
                }
                if ((columns[i].Contains("lculo ICMS")) && (!(columns[i].Contains("Substituto"))))
                {
                    ind.indexBaseIcms = i;
                }
                if (columns[i].Contains("lculo ICMS Substituto"))
                {
                    ind.indexBaseIcmsSt = i;
                }
                if (columns[i].ToLower() == "total de produtos")
                {
                    ind.indexTotalProdutos = i;
                }
                if (columns[i].ToLower() == "valor icms")
                {
                    ind.indexValorIcms = i;
                }

                if (columns[i].ToLower() == "valor icms substituto")
                {
                    ind.indexValorIcmsSt = i;
                }
                if (columns[i].ToLower() == "valor ipi")
                {
                    ind.indexValorIpi = i;
                }
                if (columns[i].ToLower() == "valor pis")
                {
                    ind.indexValorPis = i;
                }
                if (columns[i].ToLower() == "valor cofins")
                {
                    ind.indexValorCofins = i;
                }
                if (columns[i].Contains("lculo ISS"))
                {
                    ind.indexBaseIss = i;
                }
                if (columns[i].ToLower() == "cnpj transportadora")
                {
                    ind.indexCnpjTransportadora = i;
                }

                if (columns[i].Contains("culo Transportadora"))
                {
                    ind.indexPlacaTransportadora = i;
                }
                if (columns[i].Contains("Tipo de Emiss"))
                {
                    ind.indexTipoEmissao = i;
                }
                if (columns[i].Contains("Status da NF"))
                {
                    ind.indexStatusNfe = i;
                }
                if (columns[i].Contains("Valor Total da NF"))
                {
                    ind.indexValorTotalNfe = i;
                }
                if (columns[i].ToLower() == "pedido de compra")
                {
                    ind.indexPedidoCompra = i;
                }
                if (columns[i].Contains("ltima itera"))
                {
                    ind.indexUltimaIteracao = i;
                }
                if (columns[i].Contains("ltimo Evento"))
                {
                    ind.indexUltimoEvento = i;
                }
            }
            return (ind);
        }

        public static List<Cls_Conf_NDD> BuildConfNDD(StreamReader reader, Indexes ind)
        {
            string line;
            var irs = new List<Cls_Conf_NDD>();
            Cls_Conf_NDD C5;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                C5 = new Cls_Conf_NDD();
                if (ind.indexSerie != -1)
                {
                    C5.SERIE = values[ind.indexSerie];
                }
                else
                {
                    C5.SERIE = "";
                }
                if (ind.indexNumeroNota != -1)
                {
                    C5.NUMERO_DA_NOTA = values[ind.indexNumeroNota];
                }
                else
                {
                    C5.NUMERO_DA_NOTA = "";
                }
                if (ind.indexChaveAcesso != -1)
                {
                    C5.CHAVE_ACESSO = values[ind.indexChaveAcesso];
                }
                else
                {
                    C5.CHAVE_ACESSO = "";
                }
                if (ind.indexRazaoSocial != -1)
                {
                    C5.RAZAO_SOCIAL = values[ind.indexRazaoSocial];
                }
                else
                {
                    C5.RAZAO_SOCIAL = "";
                }
                if (ind.indexDataEmissao != -1)
                {
                    C5.DTA_EMISSAO = values[ind.indexDataEmissao];
                }
                else
                {
                    C5.DTA_EMISSAO = "";
                }
                if (ind.indexCnpjEmitente != -1)
                {
                    C5.CNPJ_EMITENTE = values[ind.indexCnpjEmitente];
                }
                else
                {
                    C5.CNPJ_EMITENTE = "";
                }
                if (ind.indexCnpjDestinatario != -1)
                {
                    C5.CNPJ_DESTINATARIO = values[ind.indexCnpjDestinatario];
                }
                else
                {
                    C5.CNPJ_DESTINATARIO = "";
                }
                if (ind.indexCpfDestinatario != -1)
                {
                    C5.CPF_DESTINATARIO = values[ind.indexCpfDestinatario];
                }
                else
                {
                    C5.CPF_DESTINATARIO = "";
                }
                if (ind.indexsRazaoSocialDest != -1)
                {
                    C5.RAZAO_SOCIAL_DESTINATARIO = values[ind.indexsRazaoSocialDest];
                }
                else
                {
                    C5.RAZAO_SOCIAL_DESTINATARIO = "";
                }
                if (ind.indexUf != -1)
                {
                    C5.UF = values[ind.indexUf];
                }
                else
                {
                    C5.UF = "";
                }
                if (ind.indexModelo != -1)
                {
                    C5.MODELO = values[ind.indexModelo];
                }
                else
                {
                    C5.MODELO = "";
                }
                if (ind.indexCodNumerico != -1)
                {
                    C5.COD_NUMERICO = values[ind.indexCodNumerico];
                }
                else
                {
                    C5.COD_NUMERICO = "";
                }
                if (ind.indexNaturezaOperacao != -1)
                {
                    C5.NATUREZA_OPERACAO = values[ind.indexNaturezaOperacao];
                }
                else
                {
                    C5.NATUREZA_OPERACAO = "";
                }
                if (ind.indexIeEmitente != -1)
                {
                    C5.IE_EMITENTE = values[ind.indexIeEmitente];
                }
                else
                {
                    C5.IE_EMITENTE = "";
                }
                if (ind.indexIeDestinatario != -1)
                {
                    C5.IE_DESTINATARIO = values[ind.indexIeDestinatario];
                }
                else
                {
                    C5.IE_DESTINATARIO = "";
                }
                if (ind.indexUfDestinatario != -1)
                {
                    C5.UF_DESTINATARIO = values[ind.indexUfDestinatario];
                }
                else
                {
                    C5.UF_DESTINATARIO = "";
                }
                if (ind.indexBaseIcms != -1)
                {
                    C5.BASE_ICMS = values[ind.indexBaseIcms];
                }
                else
                {
                    C5.BASE_ICMS = "";
                }
                if (ind.indexBaseIcmsSt != -1)
                {
                    C5.BASE_ICMS_ST = values[ind.indexBaseIcmsSt];
                }
                else
                {
                    C5.BASE_ICMS_ST = "";
                }
                if (ind.indexTotalProdutos != -1)
                {
                    C5.TOTAL_PRODUTOS = values[ind.indexTotalProdutos];
                }
                else
                {
                    C5.TOTAL_PRODUTOS = "";
                }
                if (ind.indexValorIcms != -1)
                {
                    C5.VALOR_ICMS = values[ind.indexValorIcms];
                }
                else
                {
                    C5.VALOR_ICMS = "";
                }
                if (ind.indexValorIcmsSt != -1)
                {
                    C5.VALOR_ICMS_ST = values[ind.indexValorIcmsSt];
                }
                else
                {
                    C5.VALOR_ICMS_ST = "";
                }
                if (ind.indexValorIpi != -1)
                {
                    C5.VALOR_IPI = values[ind.indexValorIpi];
                }
                else
                {
                    C5.VALOR_IPI = "";
                }
                if (ind.indexValorPis != -1)
                {
                    C5.VALOR_PIS = values[ind.indexValorPis];
                }
                else
                {
                    C5.VALOR_PIS = "";
                }
                if (ind.indexValorCofins != -1)
                {
                    C5.VALOR_COFINS = values[ind.indexValorCofins];
                }
                else
                {
                    C5.VALOR_COFINS = "";
                }
                if (ind.indexBaseIss != -1)
                {
                    C5.BASE_ISS = values[ind.indexBaseIss];
                }
                else
                {
                    C5.BASE_ISS = "";
                }
                if (ind.indexCnpjTransportadora != -1)
                {
                    C5.CNPJ_TRANSPORTADORA = values[ind.indexCnpjTransportadora];
                }
                else
                {
                    C5.CNPJ_TRANSPORTADORA = "";
                }
                if (ind.indexPlacaTransportadora != -1)
                {
                    C5.PLACA_TRANSPORTADORA = values[ind.indexPlacaTransportadora];
                }
                else
                {
                    C5.PLACA_TRANSPORTADORA = "";
                }
                if (ind.indexTipoEmissao != -1)
                {
                    C5.TIPO_EMISSAO = values[ind.indexTipoEmissao];
                }
                else
                {
                    C5.TIPO_EMISSAO = "";
                }
                if (ind.indexStatusNfe != -1)
                {
                    C5.STATUS_NFE = values[ind.indexStatusNfe];
                }
                else
                {
                    C5.STATUS_NFE = "";
                }
                if (ind.indexValorTotalNfe != -1)
                {
                    C5.VALOR_TOTAL_NFE = values[ind.indexValorTotalNfe];
                }
                else
                {
                    C5.VALOR_TOTAL_NFE = "";
                }
                if (ind.indexPedidoCompra != -1)
                {
                    C5.PEDIDO_COMPRA = values[ind.indexPedidoCompra];
                }
                else
                {
                    C5.PEDIDO_COMPRA = "";
                }
                if (ind.indexUltimaIteracao != -1)
                {
                    C5.ULTIMA_ITERACAO = values[ind.indexUltimaIteracao];
                }
                else
                {
                    C5.ULTIMA_ITERACAO = "";
                }
                if (ind.indexUltimoEvento != -1)
                {
                    C5.ULTIMO_EVENTO = values[ind.indexUltimoEvento];
                }
                else
                {
                    C5.ULTIMO_EVENTO = "";
                }

                irs.Add(C5);
            }
            return irs;
        }

    }
}
