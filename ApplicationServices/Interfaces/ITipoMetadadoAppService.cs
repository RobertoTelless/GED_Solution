using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ITipoMetadadoAppService : IAppServiceBase<TIPO_METADADO>
    {
        Int32 ValidateCreate(TIPO_METADADO item, USUARIO usuario);
        Int32 ValidateEdit(TIPO_METADADO item, TIPO_METADADO itemAntes, USUARIO usuario);
        Int32 ValidateEdit(TIPO_METADADO item, TIPO_METADADO itemAntes);
        Int32 ValidateDelete(TIPO_METADADO item, USUARIO usuario);
        Int32 ValidateReativar(TIPO_METADADO item, USUARIO usuario);

        TIPO_METADADO CheckExist(TIPO_METADADO item, Int32? idAss);
        List<TIPO_METADADO> GetAllItens(Int32? idAss);
        TIPO_METADADO GetItemById(Int32 id);
        List<TIPO_METADADO> GetAllItensAdm(Int32? idAss);
    }
}
