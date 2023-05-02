using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Globalization;

namespace DesktopApplication
{
    public class cls_csv_c5   
    {
        public struct Indexes
        {
            public int indexDataEntSai;
            public int indexEspecie;
            public int indexNroDocumento;
            public int indexSerie;
            public int indexChaveAcesso;
            public int indexPessoa;
            public int indexRazaoSocial;
            public int indexUF;
            public int indexCGO;
            public int indexCFOP;
            public int indexCodTrib;
            public int indexNCM;
            public int indexCodProd;
            public int indexDescProd;
            public int indexQtd;
            public int indexValorItem;
            public int indexValorCont;
            public int indexCstIcms;
            public int indexBaseIcms;
            public int indexAliqIcms;
            public int indexValorIcms;
            public int indexValorOutrasIcms;
            public int indexValorIsentosIcms;
            public int indexMvaIcmsSt;
            public int indexBaseIcmsSt;
            public int indexAliqIcmsSt;
            public int indexValorIcmsSt;
            public int indexValorOutrasIcmsSt;
            public int indexValorIsentosIcmsSt;
            public int indexCstIpi;
            public int indexBaseIpi;
            public int indexAliqIpi;
            public int indexValorIpi;
            public int indexOutrasIpi;
            public int indexIsentosIpi;
            public int indexCstPis;
            public int indexBasePis;
            public int indexAliqPis;
            public int indexValorPis;
            public int indexCstCofins;
            public int indexBaseCofins;
            public int indexAliqCofins;
            public int indexValorCofins;
            public int indexCsosn;
            public int indexBaseSimples;
            public int indexAliqSimples;
            public int indexValorSimples;
            public int indexBaseFcpIcms;
            public int indexValorFcpIcms;
            public int indexBaseFcpIcmsSt;
            public int indexValorFcpIcmsSt;
        }

        public static Indexes SetColumnsIndex(string[] columns)
        {
            Indexes ind = new Indexes();
            ind.indexDataEntSai = -1;
            ind.indexDataEntSai = -1;
            ind.indexEspecie = -1;
            ind.indexNroDocumento = -1;
            ind.indexSerie = 3;
            ind.indexChaveAcesso = -1;
            ind.indexPessoa = -1;
            ind.indexRazaoSocial = -1;
            ind.indexUF = -1;
            ind.indexCGO = -1;
            ind.indexCFOP = -1;
            ind.indexCodTrib = -1;
            ind.indexNCM = -1;
            ind.indexCodProd = -1;
            ind.indexDescProd = -1;
            ind.indexQtd = -1;
            ind.indexValorItem = -1;
            ind.indexValorCont = -1;
            ind.indexCstIcms = -1;
            ind.indexBaseIcms = -1;
            ind.indexAliqIcms = -1;
            ind.indexValorIcms = -1;
            ind.indexValorOutrasIcms = -1;
            ind.indexValorIsentosIcms = -1;
            ind.indexMvaIcmsSt = -1;
            ind.indexBaseIcmsSt = -1;
            ind.indexAliqIcmsSt = -1;
            ind.indexValorIcmsSt = -1;
            ind.indexValorOutrasIcmsSt = -1;
            ind.indexValorIsentosIcmsSt = -1;
            ind.indexCstIpi = -1;
            ind.indexBaseIpi = -1;
            ind.indexAliqIpi = -1;
            ind.indexValorIpi = -1;
            ind.indexOutrasIpi = -1;
            ind.indexIsentosIpi = -1;
            ind.indexCstPis = -1;
            ind.indexBasePis = -1;
            ind.indexAliqPis = -1;
            ind.indexValorPis = -1;
            ind.indexCstCofins = -1;
            ind.indexBaseCofins = -1;
            ind.indexAliqCofins = -1;
            ind.indexValorCofins = -1;
            ind.indexCsosn = -1;
            ind.indexBaseSimples = -1;
            ind.indexAliqSimples = -1;
            ind.indexValorSimples = -1;
            ind.indexBaseFcpIcms = -1;
            ind.indexValorFcpIcms = -1;
            ind.indexBaseFcpIcmsSt = -1;
            ind.indexValorFcpIcmsSt = -1;

            for (int i = 0; i < columns.Length; i++)
            {
                if (string.IsNullOrEmpty(columns[i]))
                    continue;
                if (columns[i].Contains("Data Ent/Sa"))
                {
                    ind.indexDataEntSai = i;
                }
                if (columns[i].Contains("Esp"))
                {
                    ind.indexEspecie = i;
                }
                if (columns[i].ToLower() == "nro. documento")
                {
                    ind.indexNroDocumento = i;
                }
                if (columns[i].Contains("Srie"))
                {
                    ind.indexSerie = i;
                }
                if (columns[i].ToLower() == "chave acesso")
                {
                    ind.indexChaveAcesso = i;
                }
                if (columns[i].ToLower() == "pessoa")
                {
                    ind.indexPessoa = i;
                }
                if (columns[i].Contains("Social"))
                {
                    ind.indexRazaoSocial = i;
                }
                if (columns[i].ToLower() == "uf")
                {
                    ind.indexUF = i;
                }
                if (columns[i].ToLower() == "cgo")
                {
                    ind.indexCGO = i;
                }
                if (columns[i].ToLower() == "cfop")
                {
                    ind.indexCFOP = i;
                }
                if (columns[i].Contains("digo Tributa"))
                {
                    ind.indexCodTrib = i;
                }
                if (columns[i].ToLower() == "ncm")
                {
                    ind.indexNCM = i;
                }
                if (columns[i].Contains("d. Produto"))
                {
                    ind.indexCodProd = i;
                }
                if (columns[i].ToLower() == "desc. produto")
                {
                    ind.indexDescProd = i;
                }
                if (columns[i].ToLower() == "qtde.")
                {
                    ind.indexQtd = i;
                }
                if (columns[i].ToLower() == "valor item")
                {
                    ind.indexValorItem = i;
                }
                if (columns[i].Contains("Valor Cont"))
                {
                    ind.indexValorCont = i;
                }
                if (columns[i].ToLower() == "cst icms")
                {
                    ind.indexCstIcms = i;
                }
                if (columns[i].ToLower() == "base icms")
                {
                    ind.indexBaseIcms = i;
                }
                if (columns[i].ToLower() == "aliq icms")
                {
                    ind.indexAliqIcms = i;
                }
                if (columns[i].ToLower() == "valor icms")
                {
                    ind.indexValorIcms = i;
                }
                if (columns[i].ToLower() == "valor outras icms")
                {
                    ind.indexValorOutrasIcms = i;
                }
                if (columns[i].ToLower() == "valor isentos icms")
                {
                    ind.indexValorIsentosIcms = i;
                }
                if (columns[i].ToLower() == "mva icms/st")
                {
                    ind.indexMvaIcmsSt = i;
                }
                if (columns[i].ToLower() == "base icms/st")
                {
                    ind.indexBaseIcmsSt = i;
                }
                if (columns[i].ToLower() == "aliq icms/st")
                {
                    ind.indexAliqIcmsSt = i;
                }
                if (columns[i].ToLower() == "valor icms/st")
                {
                    ind.indexValorIcmsSt = i;
                }
                if (columns[i].ToLower() == "valor outras icms/st")
                {
                    ind.indexValorOutrasIcmsSt = i;
                }
                if (columns[i].ToLower() == "valor isentos icms/st")
                {
                    ind.indexValorIsentosIcmsSt = i;
                }
                if (columns[i].ToLower() == "cst ipi")
                {
                    ind.indexCstIpi = i;
                }
                if (columns[i].ToLower() == "base ipi")
                {
                    ind.indexBaseIpi = i;
                }
                if (columns[i].ToLower() == "aliq. ipi")
                {
                    ind.indexAliqIpi = i;
                }
                if (columns[i].ToLower() == "valor ipi")
                {
                    ind.indexValorIpi = i;
                }
                if (columns[i].ToLower() == "valor outras ipi")
                {
                    ind.indexOutrasIpi = i;
                }
                if (columns[i].ToLower() == "valor isentos ipi")
                {
                    ind.indexIsentosIpi = i;
                }
                if (columns[i].ToLower() == "cst pis")
                {
                    ind.indexCstPis = i;
                }
                if (columns[i].ToLower() == "base pis")
                {
                    ind.indexBasePis = i;
                }
                if (columns[i].ToLower() == "aliq. pis")
                {
                    ind.indexAliqPis = i;
                }
                if (columns[i].ToLower() == "valor pis")
                {
                    ind.indexValorPis = i;
                }
                if (columns[i].ToLower() == "cst cofins")
                {
                    ind.indexCstCofins = i;
                }
                if (columns[i].ToLower() == "base cofins")
                {
                    ind.indexBaseCofins = i;
                }
                if (columns[i].ToLower() == "aliq. cofins")
                {
                    ind.indexAliqCofins = i;
                }
                if (columns[i].ToLower() == "valor cofins")
                {
                    ind.indexValorCofins = i;
                }
                if (columns[i].ToLower() == "csosn")
                {
                    ind.indexCsosn = i;
                }
                if (columns[i].ToLower() == "base simples nacional")
                {
                    ind.indexBaseSimples = i;
                }
                if (columns[i].ToLower() == "aliq. simples nacional")
                {
                    ind.indexAliqSimples = i;
                }
                if (columns[i].ToLower() == "valor simples nacional")
                {
                    ind.indexValorSimples = i;
                }
                if (columns[i].ToLower() == "base fcp icms")
                {
                    ind.indexBaseFcpIcms = i;
                }
                if (columns[i].ToLower() == "valor fcp icms")
                {
                    ind.indexValorFcpIcms = i;
                }
                if (columns[i].ToLower() == "base fcp icms st")
                {
                    ind.indexBaseFcpIcmsSt = i;
                }
                if (columns[i].ToLower() == "valor fcp icms st")
                {
                    ind.indexValorFcpIcmsSt = i;
                }
            }
            return ind;
        }

        public static List<Cls_Conf_system> BuildConfC5(StreamReader reader, Indexes ind)
        {
            string line;
            var system = new List<Cls_Conf_system>();            
            Cls_Conf_system C5;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                C5 = new Cls_Conf_system();
                if (ind.indexDataEntSai != -1)
                {
                    C5.DTA_ENT_SAIDA = values[ind.indexDataEntSai];
                }
                else
                {
                    C5.DTA_ENT_SAIDA = "";
                }
                if (ind.indexEspecie != -1)
                {
                    C5.ESPECIE = values[ind.indexEspecie];
                }
                else
                {
                    C5.ESPECIE = "";
                }
                if (ind.indexNroDocumento != -1)
                {
                    C5.NRO_DOCUMENTO = values[ind.indexNroDocumento];
                }
                else
                {
                    C5.NRO_DOCUMENTO = "";

                }
                if (ind.indexSerie != -1)
                {
                    C5.SERIE = values[ind.indexSerie];
                }
                else
                {
                    C5.SERIE = "";
                }
                if (ind.indexChaveAcesso != -1)
                {
                    C5.CHAVE_ACESSO = values[ind.indexChaveAcesso];
                }
                else
                {
                    C5.CHAVE_ACESSO = "";
                }
                if (ind.indexPessoa != -1)
                {
                    C5.PESSOA = values[ind.indexPessoa];
                }
                else
                {
                    C5.PESSOA = "";
                }
                if (ind.indexRazaoSocial != -1)
                {
                    C5.RAZAO_SOCIAL = values[ind.indexRazaoSocial];
                }
                else
                {
                    C5.RAZAO_SOCIAL = "";
                }
                if (ind.indexUF != -1)
                {
                    C5.UF = values[ind.indexUF];
                }
                else
                {
                    C5.UF = "";
                }
                if (ind.indexCGO != -1)
                {
                    C5.CGO = values[ind.indexCGO];
                }
                else
                {
                    C5.CGO = "";
                }
                if (ind.indexCFOP != -1)
                {
                    C5.CFOP = values[ind.indexCFOP];
                }
                else
                {
                    C5.CFOP = "";
                }
                if (ind.indexCodTrib != -1)
                {
                    C5.COD_TRIBUTACAO = values[ind.indexCodTrib];
                }
                else
                {
                    C5.COD_TRIBUTACAO = "";
                }
                if (ind.indexNCM != -1)
                {
                    C5.NCM = values[ind.indexNCM];
                }
                else
                {
                    C5.NCM = "";
                }
                if (ind.indexCodProd != -1)
                {
                    C5.COD_PRODUTO = values[ind.indexCodProd];
                }
                else
                {
                    C5.COD_PRODUTO = "";
                }
                if (ind.indexDescProd != -1)
                {
                    C5.DESC_PRODUTO = values[ind.indexDescProd];
                }
                else
                {
                    C5.DESC_PRODUTO = "";
                }
                if (ind.indexQtd != -1)
                {
                    C5.QUANTIDADE = values[ind.indexQtd];
                }
                else
                {
                    C5.QUANTIDADE = "";
                }
                if (ind.indexValorItem != -1)
                {
                    C5.VALOR_ITEM = values[ind.indexValorItem];
                }
                else
                {
                    C5.VALOR_ITEM = "";
                }
                if (ind.indexValorCont != -1)
                {
                    C5.VALOR_CONTABIL = values[ind.indexValorCont];
                }
                else
                {
                    C5.VALOR_CONTABIL = "";
                }
                if (ind.indexCstIcms != -1)
                {
                    C5.CST_ICMS = values[ind.indexCstIcms];
                }
                else
                {
                    C5.CST_ICMS = "";
                }
                if (ind.indexBaseIcms != -1)
                {
                    C5.BASE_ICMS = values[ind.indexBaseIcms];
                }
                else
                {
                    C5.BASE_ICMS = "";
                }
                if (ind.indexAliqIcms != -1)
                {
                    C5.ALIQ_ICMS = values[ind.indexAliqIcms];
                }
                else
                {
                    C5.ALIQ_ICMS = "";
                }
                if (ind.indexValorIcms != -1)
                {
                    C5.VALOR_ICMS = values[ind.indexValorIcms];
                }
                else
                {
                    C5.VALOR_ICMS = "";
                }
                if (ind.indexValorOutrasIcms != -1)
                {
                    C5.VALOR_OUTRAS_ICMS = values[ind.indexValorOutrasIcms];
                }
                else
                {
                    C5.VALOR_OUTRAS_ICMS = "";
                }
                if (ind.indexValorIsentosIcms != -1)
                {
                    C5.VALOR_ISENTOS_ICMS = values[ind.indexValorIsentosIcms];
                }
                else
                {
                    C5.VALOR_ISENTOS_ICMS = "";
                }
                if (ind.indexMvaIcmsSt != -1)
                {
                    C5.MVA_ICMS_ST = values[ind.indexMvaIcmsSt];
                }
                else
                {
                    C5.MVA_ICMS_ST = "";
                }
                if (ind.indexBaseIcmsSt != -1)
                {
                    C5.BASE_ICMS_ST = values[ind.indexBaseIcmsSt];
                }
                else
                {
                    C5.BASE_ICMS_ST = "";
                }
                if (ind.indexAliqIcmsSt != -1)
                {
                    C5.ALIQ_ICMS_ST = values[ind.indexAliqIcmsSt];
                }
                else
                {
                    C5.ALIQ_ICMS_ST = "";
                }
                if (ind.indexValorIcmsSt != -1)
                {
                    C5.VALOR_ICMS_ST = values[ind.indexValorIcmsSt];
                }
                else
                {
                    C5.VALOR_ICMS_ST = "";
                }
                if (ind.indexValorOutrasIcmsSt != -1)
                {
                    C5.VALOR_OUTRAS_ICMS_ST = values[ind.indexValorOutrasIcmsSt];
                }
                else
                {
                    C5.VALOR_OUTRAS_ICMS_ST = "";
                }
                if (ind.indexValorIsentosIcmsSt != -1)
                {
                    C5.VALOR_ISENTOS_ICMS_ST = values[ind.indexValorIsentosIcmsSt];
                }
                else
                {
                    C5.VALOR_ISENTOS_ICMS_ST = "";
                }
                if (ind.indexCstIpi != -1)
                {
                    C5.CST_IPI = values[ind.indexCstIpi];
                }
                else
                {
                    C5.CST_IPI = "";
                }
                if (ind.indexBaseIpi != -1)
                {
                    C5.BASE_IPI = values[ind.indexBaseIpi];
                }
                else
                {
                    C5.BASE_IPI = "";
                }
                if (ind.indexAliqIpi != -1)
                {
                    C5.ALIQ_IPI = values[ind.indexAliqIpi];
                }
                else
                {
                    C5.ALIQ_IPI = "";
                }
                if (ind.indexValorIpi != -1)
                {
                    C5.VALOR_IPI = values[ind.indexValorIpi];
                }
                else
                {
                    C5.VALOR_IPI = "";
                }
                if (ind.indexOutrasIpi != -1)
                {
                    C5.VALOR_OUTRAS_IPI = values[ind.indexOutrasIpi];
                }
                else
                {
                    C5.VALOR_OUTRAS_IPI = "";
                }
                if (ind.indexIsentosIpi != -1)
                {
                    C5.VALOR_ISENTOS_IPI = values[ind.indexIsentosIpi];
                }
                else
                {
                    C5.VALOR_ISENTOS_IPI = "";
                }
                if (ind.indexCstPis != -1)
                {
                    C5.CST_PIS = values[ind.indexCstPis];
                }
                else
                {
                    C5.CST_PIS = "";
                }
                if (ind.indexBasePis != -1)
                {
                    C5.BASE_PIS = values[ind.indexBasePis];
                }
                else
                {
                    C5.BASE_PIS = "";
                }
                if (ind.indexAliqPis != -1)
                {
                    C5.ALIQ_PIS = values[ind.indexAliqPis];
                }
                else
                {
                    C5.ALIQ_PIS = "";
                }
                if (ind.indexValorPis != -1)
                {
                    C5.VALOR_PIS = values[ind.indexValorPis];
                }
                else
                {
                    C5.VALOR_PIS = "";
                }
                if (ind.indexCstCofins != -1)
                {
                    C5.CST_COFINS = values[ind.indexCstCofins];
                }
                else
                {
                    C5.CST_COFINS = "";
                }
                if (ind.indexBaseCofins != -1)
                {
                    C5.BASE_COFINS = values[ind.indexBaseCofins];
                }
                else
                {
                    C5.BASE_COFINS = "";
                }
                if (ind.indexAliqCofins != -1)
                {
                    C5.ALIQ_COFINS = values[ind.indexAliqCofins];
                }
                else
                {
                    C5.ALIQ_COFINS = "";
                }
                if (ind.indexValorCofins != -1)
                {
                    C5.VALOR_COFINS = values[ind.indexValorCofins];
                }
                else
                {
                    C5.VALOR_COFINS = "";
                }
                if (ind.indexCsosn != -1)
                {
                    C5.CSOSN = values[ind.indexCsosn];
                }
                else
                {
                    C5.CSOSN = "";
                }
                if (ind.indexBaseSimples != -1)
                {
                    C5.BASE_SIMPLES_NAC = values[ind.indexBaseSimples];
                }
                else
                {
                    C5.BASE_SIMPLES_NAC = "";
                }
                if (ind.indexAliqSimples != -1)
                {
                    C5.ALIQ_SIMPLES_NAC = values[ind.indexAliqSimples];
                }
                else
                {
                    C5.ALIQ_SIMPLES_NAC = "";
                }
                if (ind.indexValorSimples != -1)
                {
                    C5.VALOR_SIMPLES_NAC = values[ind.indexValorSimples];
                }
                else
                {
                    C5.VALOR_SIMPLES_NAC = "";

                }
                if (ind.indexBaseFcpIcms != -1)
                {
                    C5.BASE_FCP_ICMS = values[ind.indexBaseFcpIcms];
                }
                else
                {
                    C5.BASE_FCP_ICMS = "";
                }
                if (ind.indexValorFcpIcms != -1)
                {
                    C5.VALOR_FCP_ICMS = values[ind.indexValorFcpIcms];
                }
                else
                {
                    C5.VALOR_FCP_ICMS = "";
                }
                if (ind.indexBaseFcpIcmsSt != -1)
                {
                    C5.BASE_FCP_ICMS_ST = values[ind.indexBaseFcpIcmsSt];
                }
                else
                {
                    C5.BASE_FCP_ICMS_ST = "";
                }
                if (ind.indexValorFcpIcmsSt != -1)
                {
                    C5.VALOR_FCP_ICMS_ST = values[ind.indexValorFcpIcmsSt];
                }
                else
                {
                    C5.VALOR_FCP_ICMS_ST = "";
                }
                system.Add(C5);
            }
            return system;
        }
    }
}
