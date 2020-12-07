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
    public class TipoMetadadoAppService : AppServiceBase<TIPO_METADADO>, ITipoMetadadoAppService
    {
        private readonly ITipoMetadadoService _baseService;

        public TipoMetadadoAppService(ITipoMetadadoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<TIPO_METADADO> GetAllItens(Int32? idAss)
        {
            List<TIPO_METADADO> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<TIPO_METADADO> GetAllItensAdm(Int32? idAss)
        {
            List<TIPO_METADADO> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public TIPO_METADADO GetItemById(Int32 id)
        {
            TIPO_METADADO item = _baseService.GetItemById(id);
            return item;
        }

        public TIPO_METADADO CheckExist(TIPO_METADADO ag, Int32? idAss)
        {
            TIPO_METADADO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ValidateCreate(TIPO_METADADO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.TIME_IN_ATIVO = 1;
                item.ASSI_CD_ID = usuario.ASSI_CD_ID;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddTIME",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_METADADO>(item)
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

        public Int32 ValidateEdit(TIPO_METADADO item, TIPO_METADADO itemAntes, USUARIO usuario)
        {
            try
            {
                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditTIME",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_METADADO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<TIPO_METADADO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(TIPO_METADADO item, TIPO_METADADO itemAntes)
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

        public Int32 ValidateDelete(TIPO_METADADO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.METADADO_CLASSE.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.TIME_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleTIME",
                    LOG_TX_REGISTRO = "Tipo de Metadado: " + item.TIME_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(TIPO_METADADO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.TIME_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatTIME",
                    LOG_TX_REGISTRO = "Tipo de Metadado: " + item.TIME_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
