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
    public class MetadadoClasseService : ServiceBase<METADADO_CLASSE>, IMetadadoClasseService
    {
        private readonly IMetadadoClasseRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly IClasseRepository _claRepository;
        private readonly ITipoMetadadoRepository _tmRepository;
        protected GEDEntities Db = new GEDEntities();

        public MetadadoClasseService(IMetadadoClasseRepository baseRepository, ILogRepository logRepository, IClasseRepository claRepository, ITipoMetadadoRepository tmRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _claRepository = claRepository;
            _tmRepository = tmRepository;
        }

        public METADADO_CLASSE CheckExist(METADADO_CLASSE item, Int32? idAss)
        {
            METADADO_CLASSE volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public METADADO_CLASSE GetItemById(Int32 id)
        {
            METADADO_CLASSE item = _baseRepository.GetItemById(id);
            return item;
        }

        public List<METADADO_CLASSE> GetAllItens(Int32 idAss)
        {
            return _baseRepository.GetAllItens(idAss);
        }

        public List<METADADO_CLASSE> GetAllItensAdm(Int32 idAss)
        {
            return _baseRepository.GetAllItensAdm(idAss);
        }

        public List<TIPO_METADADO> GetAllTipos(Int32 idAss)
        {
            return _tmRepository.GetAllItens(idAss);
        }

        public List<CLASSE> GetAllClasses(Int32 idAss)
        {
            return _claRepository.GetAllItens(idAss);
        }

        public Int32 Create(METADADO_CLASSE item, LOG log)
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

        public Int32 Create(METADADO_CLASSE item)
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


        public Int32 Edit(METADADO_CLASSE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    METADADO_CLASSE obj = _baseRepository.GetById(item.MECL_CD_ID);
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

        public Int32 Edit(METADADO_CLASSE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    METADADO_CLASSE obj = _baseRepository.GetById(item.MECL_CD_ID);
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

        public Int32 Delete(METADADO_CLASSE item, LOG log)
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
    }
}
