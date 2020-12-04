using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ITipoDocumentoService : IServiceBase<TIPO_DOCUMENTO>
    {
        Int32 Create(TIPO_DOCUMENTO item, LOG log);
        Int32 Create(TIPO_DOCUMENTO item);
        Int32 Edit(TIPO_DOCUMENTO item, LOG log);
        Int32 Edit(TIPO_DOCUMENTO item);
        Int32 Delete(TIPO_DOCUMENTO item, LOG log);

        TIPO_DOCUMENTO CheckExist(TIPO_DOCUMENTO item, Int32? idAss);
        List<TIPO_DOCUMENTO> GetAllItens(Int32? idAss);
        TIPO_DOCUMENTO GetItemById(Int32 id);
        List<TIPO_DOCUMENTO> GetAllItensAdm(Int32? idAss);
    }
}
