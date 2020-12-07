using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ITipoDocumentoAppService : IAppServiceBase<TIPO_DOCUMENTO>
    {
        Int32 ValidateCreate(TIPO_DOCUMENTO item, USUARIO usuario);
        Int32 ValidateEdit(TIPO_DOCUMENTO item, TIPO_DOCUMENTO itemAntes, USUARIO usuario);
        Int32 ValidateEdit(TIPO_DOCUMENTO item, TIPO_DOCUMENTO itemAntes);
        Int32 ValidateDelete(TIPO_DOCUMENTO item, USUARIO usuario);
        Int32 ValidateReativar(TIPO_DOCUMENTO item, USUARIO usuario);

        TIPO_DOCUMENTO CheckExist(TIPO_DOCUMENTO item, Int32? idAss);
        List<TIPO_DOCUMENTO> GetAllItens(Int32? idAss);
        TIPO_DOCUMENTO GetItemById(Int32 id);
        List<TIPO_DOCUMENTO> GetAllItensAdm(Int32? idAss);
    }
}
