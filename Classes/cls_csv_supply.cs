using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SistemaEtccom.Classes
{
    class cls_csv_fornec
    {
        public struct Indexes
        {
            public int indexSeqFornecedor;
            public int indexNomeRazao;
            public int indexCnpj;
            public int indexUf;
            public int indexTipoFornec;
            public int indexNroRegTrib;
            public int indexMicroempresa;
            public int indexProdRural;
        }

        public static Indexes SetColumnsIndex(string[] columns)
        {
            Indexes ind = new Indexes();
            ind.indexSeqFornecedor = -1;
            ind.indexNomeRazao = -1;
            ind.indexCnpj = -1;
            ind.indexUf = 3;
            ind.indexTipoFornec = -1;
            ind.indexNroRegTrib = -1;
            ind.indexMicroempresa = -1;
            ind.indexProdRural = -1;

            for (int i = 0; i < columns.Length; i++)
            {
                if (string.IsNullOrEmpty(columns[i]))
                    continue;
                if (columns[i].ToLower() == "seqfornecedor")
                {
                    ind.indexSeqFornecedor = i;
                }
                if (columns[i].ToLower() == "nomerazao")
                {
                    ind.indexNomeRazao = i;
                }
                if (columns[i].ToLower() == "cnpj")
                {
                    ind.indexCnpj = i;
                }
                if (columns[i].ToLower() == "uf")
                {
                    ind.indexUf = i;
                }
                if (columns[i].ToLower() == "tipfornec")
                {
                    ind.indexTipoFornec = i;
                }
                if (columns[i].ToLower() == "nroregtrib")
                {
                    ind.indexNroRegTrib = i;
                }
                if (columns[i].ToLower() == "microempresa")
                {
                    ind.indexMicroempresa = i;
                }
                if (columns[i].ToLower() == "prodrural")
                {
                    ind.indexProdRural = i;
                }
            }
            return ind;
        }

        public static List<Cls_Conf_Fornec> BuildConfC5(StreamReader reader, Indexes ind)
        {
            string line;
            var supply = new List<Cls_Conf_Fornec>();
            Cls_Conf_Fornec C5;
            while ((line = reader.ReadLine()) != null)
            {
                var values = line.Split(';');
                C5 = new Cls_Conf_Fornec();
                if (ind.indexSeqFornecedor != -1)
                {
                    C5.SEQ_FORNECEDOR = values[ind.indexSeqFornecedor];
                }
                else
                {
                    C5.SEQ_FORNECEDOR = "";
                }
                if (ind.indexNomeRazao != -1)
                {
                    C5.NOME_RAZAO = values[ind.indexNomeRazao];
                }
                else
                {
                    C5.NOME_RAZAO = "";
                }
                if (ind.indexCnpj != -1)
                {
                    C5.CNPJ = values[ind.indexCnpj];
                }
                else
                {
                    C5.CNPJ = "";

                }
                if (ind.indexUf != -1)
                {
                    C5.UF = values[ind.indexUf];
                }
                else
                {
                    C5.UF = "";
                }
                if (ind.indexTipoFornec != -1)
                {
                    C5.TIPOFORNEC = values[ind.indexTipoFornec];
                }
                else
                {
                    C5.TIPOFORNEC = "";
                }
                if (ind.indexNroRegTrib != -1)
                {
                    C5.NRO_REGTRIB = values[ind.indexNroRegTrib];
                }
                else
                {
                    C5.NRO_REGTRIB = "";
                }
                if (ind.indexMicroempresa != -1)
                {
                    C5.MICROEMPRESA = values[ind.indexMicroempresa];
                }
                else
                {
                    C5.MICROEMPRESA = "";
                }
                if (ind.indexProdRural != -1)
                {
                    C5.PROD_RURAL = values[ind.indexProdRural];
                }
                else
                {
                    C5.PROD_RURAL = "";
                }
                supply.Add(C5);
            }
            return supply;
        }        
    }
}
