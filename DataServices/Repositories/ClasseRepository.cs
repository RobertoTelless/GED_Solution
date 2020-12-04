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
    public class ClasseRepository : RepositoryBase<CLASSE>, IClasseRepository
    {
        public CLASSE GetItemById(Int32 id)
        {
            IQueryable<CLASSE> query = Db.CLASSE;
            query = query.Where(p => p.CLAS_CD_ID == id);
            return query.FirstOrDefault();
        }

        public CLASSE CheckExist(CLASSE item, Int32? idAss)
        {
            IQueryable<CLASSE> query = Db.CLASSE;
            query = query.Where(p => p.CLAS_NM_NOME == item.CLAS_NM_NOME);
            query = query.Where(p => p.ASSI_CD_ID == item.ASSI_CD_ID);
            return query.FirstOrDefault();
        }

        public List<CLASSE> GetAllItens(Int32 idAss)
        {
            IQueryable<CLASSE> query = Db.CLASSE.Where(p => p.CLAS_IN_ATIVO == 1);
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            query = query.OrderByDescending(a => a.CLAS_NM_NOME);
            return query.ToList();
        }

        public List<CLASSE> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<CLASSE> query = Db.CLASSE;
            query = query.Where(p => p.ASSI_CD_ID == idAss);
            query = query.OrderByDescending(a => a.CLAS_NM_NOME);
            return query.ToList();
        }

        public List<CLASSE> ExecuteFilter(Int32? subgrupo, String nome, String descricao, String sigla, Int32 idAss)
        {
            List<CLASSE> lista = new List<CLASSE>();
            IQueryable<CLASSE> query = Db.CLASSE;
            if (!String.IsNullOrEmpty(nome))
            {
                query = query.Where(p => p.CLAS_NM_NOME.Contains(nome));
            }
            if (subgrupo != null)
            {
                query = query.Where(p => p.SUBG_CD_ID == subgrupo);
            }
            if (!String.IsNullOrEmpty(descricao))
            {
                query = query.Where(p => p.CLAS_DS_DESCRICAO.Contains(descricao));
            }
            if (!String.IsNullOrEmpty(sigla))
            {
                query = query.Where(p => p.CLAS_SG_SIGLA.Contains(sigla));
            }
            if (query != null)
            {
                query = query.Where(p => p.ASSI_CD_ID == idAss);
                query = query.OrderBy(a => a.CLAS_NM_NOME);
                lista = query.ToList<CLASSE>();
            }
            return lista;
        }
    }
}
