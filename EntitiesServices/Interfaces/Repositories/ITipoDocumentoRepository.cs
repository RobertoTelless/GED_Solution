using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface ITipoDocumentoRepository : IRepositoryBase<TIPO_DOCUMENTO>
    {
        TIPO_DOCUMENTO CheckExist(TIPO_DOCUMENTO item, Int32? idAss);
        List<TIPO_DOCUMENTO> GetAllItens(Int32 idAss);
        TIPO_DOCUMENTO GetItemById(Int32 id);
        List<TIPO_DOCUMENTO> GetAllItensAdm(Int32 idAss);
    }
}
