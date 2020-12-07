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
    public class DocumentoPesquisaRepository : RepositoryBase<DOCUMENTO_PESQUISA>, IDocumentoPesquisaRepository
    {
        public DOCUMENTO_PESQUISA GetItemById(Int32 id)
        {
            IQueryable<DOCUMENTO_PESQUISA> query = Db.DOCUMENTO_PESQUISA;
            query = query.Where(p => p.DOPE_CD_ID == id);
            return query.FirstOrDefault();
        }

        public List<DOCUMENTO_PESQUISA> GetAllItens(Int32 idAss)
        {
            IQueryable<DOCUMENTO_PESQUISA> query = Db.DOCUMENTO_PESQUISA;
            query = query.Where(p => p.DOCU_CD_ID == idAss);
            query = query.OrderByDescending(a => a.DOPE_DT_PESQUISA);
            return query.ToList();
        }

        public List<DOCUMENTO_PESQUISA> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<DOCUMENTO_PESQUISA> query = Db.DOCUMENTO_PESQUISA;
            query = query.Where(p => p.DOCU_CD_ID == idAss);
            query = query.OrderByDescending(a => a.DOPE_DT_PESQUISA);
            return query.ToList();
        }

        public List<DOCUMENTO_PESQUISA> ExecuteFilter(Int32? idUsuario, Int32? doc, DateTime? data, Int32? idAss)
        {
            List<DOCUMENTO_PESQUISA> lista = new List<DOCUMENTO_PESQUISA>();
            IQueryable<DOCUMENTO_PESQUISA> query = Db.DOCUMENTO_PESQUISA;
            if (idUsuario != null)
            {
                query = query.Where(p => p.USUA_CD_ID == idUsuario);
            }
            if (doc != null)
            {
                query = query.Where(p => p.DOCU_CD_ID == doc);
            }
            if (data != null)
            {
                query = query.Where(p => DbFunctions.TruncateTime(p.DOPE_DT_PESQUISA) == DbFunctions.TruncateTime(data));
            }
            if (query != null)
            {
                query = query.Where(p => p.DOCU_CD_ID == idAss);
                query = query.OrderBy(a => a.DOPE_DT_PESQUISA);
                lista = query.ToList<DOCUMENTO_PESQUISA>();
            }
            return lista;
        }
    }
}
