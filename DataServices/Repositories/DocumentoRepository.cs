using System;
using System.Collections.Generic;
using EntitiesServices.Model;  
using ModelServices.Interfaces.Repositories;
using System.Linq;
using EntitiesServices.Work_Classes;
using System.Data.Entity;
using CrossCutting;

namespace DataServices.Repositories
{
    public class DocumentoRepository : RepositoryBase<DOCUMENTO>, IDocumentoRepository
    {
        public DOCUMENTO GetItemById(Int32 id)
        {
            IQueryable<DOCUMENTO> query = Db.DOCUMENTO;
            query = query.Where(p => p.DOCU_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<DOCUMENTO> GetAllItens(Int32 idAss)
        {
            IQueryable<DOCUMENTO> query = Db.DOCUMENTO.Where(p => p.DOCU_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            query = query.OrderByDescending(a => a.DOCU_NM_NOME);
            return query.ToList();
        }

        public List<DOCUMENTO> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<DOCUMENTO> query = Db.DOCUMENTO;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            query = query.OrderByDescending(a => a.DOCU_NM_NOME);
            return query.ToList();
        }

        public List<DOCUMENTO> ExecuteFilter(Int32? classe, Int32? idCat, Int32? idUsuario, String nome, DateTime? data, Int32? idFormato, String descricao, String ocr, Int32 idAss)
        {
            List<DOCUMENTO> lista = new List<DOCUMENTO>();
            IQueryable<DOCUMENTO> query = Db.DOCUMENTO;
            if (classe != null)
            {
                query = query.Where(p => p.CLAS_CD_ID == classe);
            }
            if (idCat != null)
            {
                query = query.Where(p => p.CADO_CD_ID == idCat);
            }
            if (idUsuario != null)
            {
                query = query.Where(p => p.USUA_CD_ID == idUsuario);
            }
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.DOCU_NM_NOME.Contains(nome));
            }
            if (data != null)
            {
                query = query.Where(p => DbFunctions.TruncateTime(p.DOCU_DT_CADASTRO) == DbFunctions.TruncateTime(data));
            }
            if (idFormato != null)
            {
                query = query.Where(p => p.DOCU_IN_FORMATO == idUsuario);
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.DOCU_DS_DESCRICAO.Contains(descricao));
            }
            if (!String.IsNullOrEmpty(ocr))
            {
                query = query.Where(p => p.DOCU_TX_OCR.Contains(ocr));
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.DOCU_NM_NOME);
                lista = query.ToList<DOCUMENTO>();
            }
            return lista;
        }
    }
}
