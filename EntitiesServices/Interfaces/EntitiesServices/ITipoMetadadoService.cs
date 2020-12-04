using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface ITipoMetadadoService : IServiceBase<TIPO_METADADO>
    {
        Int32 Create(TIPO_METADADO item, LOG log);
        Int32 Create(TIPO_METADADO item);
        Int32 Edit(TIPO_METADADO item, LOG log);
        Int32 Edit(TIPO_METADADO item);
        Int32 Delete(TIPO_METADADO item, LOG log);

        TIPO_METADADO CheckExist(TIPO_METADADO item, Int32? idAss);
        List<TIPO_METADADO> GetAllItens(Int32? idAss);
        TIPO_METADADO GetItemById(Int32 id);
        List<TIPO_METADADO> GetAllItensAdm(Int32? idAss);
    }
}
