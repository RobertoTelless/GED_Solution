using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;
using ApplicationServices.Interfaces;
using ModelServices.Interfaces.EntitiesServices;
using CrossCutting;
using System.Text.RegularExpressions;

namespace ApplicationServices.Services
{
    public class ClasseAppService : AppServiceBase<CLASSE>, IClasseAppService
    {
        private readonly IClasseService _baseService;

        public ClasseAppService(IClasseService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<CLASSE> GetAllItens(Int32 idAss)
        {
            List<CLASSE> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<CLASSE> GetAllItensAdm(Int32 idAss)
        {
            List<CLASSE> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public CLASSE GetItemById(Int32 id)
        {
            CLASSE item = _baseService.GetItemById(id);
            return item;
        }

        public CLASSE CheckExist(CLASSE ag, Int32 idAss)
        {
            CLASSE item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public List<SUBGRUPO> GetAllSubgrupos(Int32? idAss)
        {
            List<SUBGRUPO> lista = _baseService.GetAllSubgrupos(idAss.Value);
            return lista;
        }

        public Int32 ExecuteFilter(Int32? subgrupo, String nome, String descricao, String sigla, Int32 idAss, out List<CLASSE> objeto)
        {
            try
            {
                objeto = new List<CLASSE>();
                Int32 volta = 0;

                // Processa filtro
                objeto = _baseService.ExecuteFilter(subgrupo, nome, descricao, sigla, idAss);
                if (objeto.Count == 0)
                {
                    volta = 1;
                }
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreate(CLASSE item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.CLAS_IN_ATIVO = 1;
                item.ASSI_CD_ID = usuario.ASSI_CD_ID;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddCLAS",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLASSE>(item)
                };

                // Persiste
                Int32 volta = _baseService.Create(item, log);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(CLASSE item, CLASSE itemAntes, USUARIO usuario)
        {
            try
            {
                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditCLAS",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<CLASSE>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<CLASSE>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(CLASSE item, CLASSE itemAntes)
        {
            try
            {
                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateDelete(CLASSE item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.DOCUMENTO.Count > 0)
                {
                    return 1;
                }
                if (item.METADADO_CLASSE.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.CLAS_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleCLAS",
                    LOG_TX_REGISTRO = "Classe: " + item.CLAS_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(CLASSE item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.CLAS_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatCLAS",
                    LOG_TX_REGISTRO = "Classe: " + item.CLAS_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public METADADO_CLASSE GetMetadadoById(Int32 id)
        {
            METADADO_CLASSE item = _baseService.GetMetadadoById(id);
            return item;
        }

        public Int32 ValidateEditMetadado(METADADO_CLASSE item)
        {
            try
            {
                // Persiste
                return _baseService.EditMetadado(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateCreateMetadado(METADADO_CLASSE item)
        {
            try
            {
                // Persiste
                Int32 volta = _baseService.CreateMetadado(item);
                return volta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
