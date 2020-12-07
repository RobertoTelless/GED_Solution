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
    public class MetadadoClasseRepository : RepositoryBase<METADADO_CLASSE>, IMetadadoClasseRepository
    {
        public METADADO_CLASSE GetItemById(Int32 id)
        {
            IQueryable<METADADO_CLASSE> query = Db.METADADO_CLASSE;
            query = query.Where(p => p.MECL_CD_ID == id);
            return query.FirstOrDefault();
        }

        public METADADO_CLASSE CheckExist(METADADO_CLASSE conta, Int32? idAss)
        {
            IQueryable<METADADO_CLASSE> query = Db.METADADO_CLASSE;
            query = query.Where(p => p.MECL_NM_NOME == conta.MECL_NM_NOME);
            return query.FirstOrDefault();
        }

        public List<METADADO_CLASSE> GetAllItens(Int32 idAss)
        {
            IQueryable<METADADO_CLASSE> query = Db.METADADO_CLASSE.Where(p => p.MECK_IN_ATIVO == 1);
            query = query.Where(p => p.CLAS_CD_ID == idAss);
            query = query.OrderByDescending(a => a.MECL_NM_NOME);
            return query.ToList();
        }

        public List<METADADO_CLASSE> GetAllItensAdm(Int32 idAss)
        {
            IQueryable<METADADO_CLASSE> query = Db.METADADO_CLASSE;
            query = query.Where(p => p.CLAS_CD_ID == idAss);
            query = query.OrderByDescending(a => a.MECL_NM_NOME);
            return query.ToList();
        }

    }
}
