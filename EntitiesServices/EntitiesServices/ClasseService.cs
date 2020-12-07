using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ModelServices.Interfaces.Repositories;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Data.Entity;
using System.Data;

namespace ModelServices.EntitiesServices
{
    public class ClasseService : ServiceBase<CLASSE>, IClasseService
    {
        private readonly IClasseRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly ISubgrupoRepository _subRepository;
        private readonly IMetadadoClasseRepository _metRepository;
        protected GEDEntities Db = new GEDEntities();

        public ClasseService(IClasseRepository baseRepository, ILogRepository logRepository, ISubgrupoRepository subRepository, IMetadadoClasseRepository metRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _subRepository = subRepository;
            _metRepository = metRepository;
        }

        public CLASSE CheckExist(CLASSE item, Int32? idAss)
        {
            CLASSE volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public CLASSE GetItemById(Int32 id)
        {
            CLASSE item = _baseRepository.GetItemById(id);
            return item;
        }

        public List<CLASSE> GetAllItens(Int32 idAss)
        {
            return _baseRepository.GetAllItens(idAss);
        }

        public List<CLASSE> GetAllItensAdm(Int32 idAss)
        {
            return _baseRepository.GetAllItensAdm(idAss);
        }

        public List<SUBGRUPO> GetAllSubgrupos(Int32 idAss)
        {
            return _subRepository.GetAllItens(idAss);
        }

        public List<CLASSE> ExecuteFilter(Int32? subgrupo, String nome, String descricao, String sigla, Int32 idAss)
        {
            return _baseRepository.ExecuteFilter(subgrupo, nome, descricao, sigla, idAss);

        }

        public Int32 Create(CLASSE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Create(CLASSE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _baseRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }


        public Int32 Edit(CLASSE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CLASSE obj = _baseRepository.GetById(item.CLAS_CD_ID);
                    _baseRepository.Detach(obj);
                    _logRepository.Add(log);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Edit(CLASSE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CLASSE obj = _baseRepository.GetById(item.CLAS_CD_ID);
                    _baseRepository.Detach(obj);
                    _baseRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 Delete(CLASSE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _logRepository.Add(log);
                    _baseRepository.Remove(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public METADADO_CLASSE GetMetadadoById(Int32 id)
        {
            return _metRepository.GetItemById(id);
        }

        public Int32 EditMetadado(METADADO_CLASSE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    METADADO_CLASSE obj = _metRepository.GetById(item.MECL_CD_ID);
                    _metRepository.Detach(obj);
                    _metRepository.Update(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Int32 CreateMetadado(METADADO_CLASSE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    _metRepository.Add(item);
                    transaction.Commit();
                    return 0;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

    }
}
