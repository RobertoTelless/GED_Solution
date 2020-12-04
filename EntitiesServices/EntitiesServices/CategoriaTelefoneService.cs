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
    public class CategoriaTelefoneService : ServiceBase<CATEGORIA_TELEFONE>, ICategoriaTelefoneService
    {
        private readonly ICategoriaTelefoneRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        protected GEDEntities Db = new GEDEntities();

        public CategoriaTelefoneService(ICategoriaTelefoneRepository baseRepository, ILogRepository logRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
        }

        public CATEGORIA_TELEFONE GetItemById(Int32 id)
        {
            CATEGORIA_TELEFONE item = _baseRepository.GetItemById(id);
            return item;
        }

        public CATEGORIA_TELEFONE CheckExist(CATEGORIA_TELEFONE item, Int32? idAss)
        {
            CATEGORIA_TELEFONE volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public List<CATEGORIA_TELEFONE> GetAllItens(Int32? id)
        {
            return _baseRepository.GetAllItens(id.Value);
        }

        public List<CATEGORIA_TELEFONE> GetAllItensAdm(Int32? id)
        {
            return _baseRepository.GetAllItensAdm(id.Value);
        }
    
        public Int32 Create(CATEGORIA_TELEFONE item, LOG log)
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

        public Int32 Create(CATEGORIA_TELEFONE item)
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


        public Int32 Edit(CATEGORIA_TELEFONE item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CATEGORIA_TELEFONE obj = _baseRepository.GetById(item.CATE_CD_ID);
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

        public Int32 Edit(CATEGORIA_TELEFONE item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    CATEGORIA_TELEFONE obj = _baseRepository.GetById(item.CATE_CD_ID);
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

        public Int32 Delete(CATEGORIA_TELEFONE item, LOG log)
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
