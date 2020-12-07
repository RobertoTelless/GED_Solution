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
    public class TipoDocumentoAppService : AppServiceBase<TIPO_DOCUMENTO>, ITipoDocumentoAppService
    {
        private readonly ITipoDocumentoService _baseService;

        public TipoDocumentoAppService(ITipoDocumentoService baseService): base(baseService)
        {
            _baseService = baseService;
        }

        public List<TIPO_DOCUMENTO> GetAllItens(Int32? idAss)
        {
            List<TIPO_DOCUMENTO> lista = _baseService.GetAllItens(idAss);
            return lista;
        }

        public List<TIPO_DOCUMENTO> GetAllItensAdm(Int32? idAss)
        {
            List<TIPO_DOCUMENTO> lista = _baseService.GetAllItensAdm(idAss);
            return lista;
        }

        public TIPO_DOCUMENTO GetItemById(Int32 id)
        {
            TIPO_DOCUMENTO item = _baseService.GetItemById(id);
            return item;
        }

        public TIPO_DOCUMENTO CheckExist(TIPO_DOCUMENTO ag, Int32 idAss)
        {
            TIPO_DOCUMENTO item = _baseService.CheckExist(ag, idAss);
            return item;
        }

        public Int32 ValidateCreate(TIPO_DOCUMENTO item, USUARIO usuario)
        {
            try
            {
                // Verifica existencia prévia
                if (_baseService.CheckExist(item, usuario.ASSI_CD_ID) != null)
                {
                    return 1;
                }

                // Completa objeto
                item.CADO_IN_ATIVO = 1;
                item.ASSI_CD_ID = usuario.ASSI_CD_ID;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "AddCADO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_DOCUMENTO>(item)
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

        public Int32 ValidateEdit(TIPO_DOCUMENTO item, TIPO_DOCUMENTO itemAntes, USUARIO usuario)
        {
            try
            {
                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_NM_OPERACAO = "EditCADO",
                    LOG_IN_ATIVO = 1,
                    LOG_TX_REGISTRO = Serialization.SerializeJSON<TIPO_DOCUMENTO>(item),
                    LOG_TX_REGISTRO_ANTES = Serialization.SerializeJSON<TIPO_DOCUMENTO>(itemAntes)
                };

                // Persiste
                return _baseService.Edit(item, log);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateEdit(TIPO_DOCUMENTO item, TIPO_DOCUMENTO itemAntes)
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

        public Int32 ValidateDelete(TIPO_DOCUMENTO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial
                if (item.DOCUMENTO.Count > 0)
                {
                    return 1;
                }

                // Acerta campos
                item.CADO_IN_ATIVO = 0;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "DeleCADO",
                    LOG_TX_REGISTRO = "Tipo de Documento: " + item.CADO_NM_NOME
                };

                // Persiste
                return _baseService.Edit(item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 ValidateReativar(TIPO_DOCUMENTO item, USUARIO usuario)
        {
            try
            {
                // Verifica integridade referencial

                // Acerta campos
                item.CADO_IN_ATIVO = 1;

                // Monta Log
                LOG log = new LOG
                {
                    LOG_DT_DATA = DateTime.Now,
                    USUA_CD_ID = usuario.USUA_CD_ID,
                    ASSI_CD_ID = usuario.ASSI_CD_ID,
                    LOG_IN_ATIVO = 1,
                    LOG_NM_OPERACAO = "ReatCADO",
                    LOG_TX_REGISTRO = "Tipo de Documento: " + item.CADO_NM_NOME
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
