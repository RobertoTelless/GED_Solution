using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IMetadadoClasseService : IServiceBase<METADADO_CLASSE>
    {
        Int32 Create(METADADO_CLASSE item, LOG log);
        Int32 Create(METADADO_CLASSE item);
        Int32 Edit(METADADO_CLASSE item, LOG log);
        Int32 Edit(METADADO_CLASSE item);
        Int32 Delete(METADADO_CLASSE item, LOG log);

        METADADO_CLASSE CheckExist(METADADO_CLASSE item, Int32? idAss);
        METADADO_CLASSE GetItemById(Int32 id);
        List<METADADO_CLASSE> GetAllItens(Int32 idAss);
        List<METADADO_CLASSE> GetAllItensAdm(Int32 idAss);
        List<TIPO_METADADO> GetAllTipos(Int32 idAss);
    }
}
