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
    public class DocumentoService : ServiceBase<DOCUMENTO>, IDocumentoService
    {
        private readonly IDocumentoRepository _baseRepository;
        private readonly ILogRepository _logRepository;
        private readonly IClasseRepository _claRepository;
        private readonly ITipoDocumentoRepository _tdRepository;
        private readonly IUsuarioRepository _usuRepository;
        protected GEDEntities Db = new GEDEntities();

        public DocumentoService(IDocumentoRepository baseRepository, ILogRepository logRepository, IClasseRepository claRepository, ITipoDocumentoRepository tdRepository, IUsuarioRepository usuRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _logRepository = logRepository;
            _claRepository = claRepository;
            _tdRepository = tdRepository;
            _usuRepository = usuRepository;
        }

        public DOCUMENTO CheckExist(DOCUMENTO item, Int32? idAss)
        {
            DOCUMENTO volta = _baseRepository.CheckExist(item, idAss);
            return volta;
        }

        public DOCUMENTO GetItemById(Int32 id)
        {
            DOCUMENTO item = _baseRepository.GetItemById(id);
            return item;
        }

        public List<DOCUMENTO> GetAllItens(Int32 idAss)
        {
            return _baseRepository.GetAllItens(idAss);
        }

        public List<DOCUMENTO> GetAllItensAdm(Int32 idAss)
        {
            return _baseRepository.GetAllItensAdm(idAss);
        }

        public List<CLASSE> GetAllClasses(Int32 idAss)
        {
            return _claRepository.GetAllItens(idAss);
        }

        public List<TIPO_DOCUMENTO> GetAllTipos(Int32 idAss)
        {
            return _tdRepository.GetAllItens(idAss);
        }

        public List<USUARIO> GetAllUsuarios(Int32 idAss)
        {
            return _usuRepository.GetAllItens(idAss);
        }

        public List<DOCUMENTO> ExecuteFilter(Int32? classe, Int32? idCat, Int32? idUsuario, String nome, DateTime? data, Int32? idFormato, String descricao, String ocr, Int32 idAss)
        {
            return _baseRepository.ExecuteFilter(classe, idCat, idUsuario, nome, data, idFormato, descricao, ocr, idAss);

        }

        public Int32 Create(DOCUMENTO item, LOG log)
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

        public Int32 Create(DOCUMENTO item)
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


        public Int32 Edit(DOCUMENTO item, LOG log)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    DOCUMENTO obj = _baseRepository.GetById(item.DOCU_CD_ID);
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

        public Int32 Edit(DOCUMENTO item)
        {
            using (DbContextTransaction transaction = Db.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    DOCUMENTO obj = _baseRepository.GetById(item.DOCU_CD_ID);
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

        public Int32 Delete(DOCUMENTO item, LOG log)
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
