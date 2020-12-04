using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IMetadadoClasseRepository : IRepositoryBase<METADADO_CLASSE>
    {
        METADADO_CLASSE GetItemById(Int32 id);
        List<METADADO_CLASSE> GetAllItens(Int32 idAss);
        List<METADADO_CLASSE> GetAllItensAdm(Int32 idAss);
    }
}
