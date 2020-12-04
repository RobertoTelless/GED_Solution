using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IClasseService : IServiceBase<CLASSE>
    {
        Int32 Create(CLASSE item, LOG log);
        Int32 Create(CLASSE item);
        Int32 Edit(CLASSE item, LOG log);
        Int32 Edit(CLASSE item);
        Int32 Delete(CLASSE item, LOG log);

        CLASSE CheckExist(CLASSE item, Int32? idAss);
        CLASSE GetItemById(Int32 id);
        List<CLASSE> GetAllItens(Int32 idAss);
        List<CLASSE> GetAllItensAdm(Int32 idAss);
        List<CLASSE> ExecuteFilter(Int32? subgrupo, String nome, String descricao, String sigla, Int32 idAss);
        List<SUBGRUPO> GetAllSubgrupos(Int32 idAss);
    }
}
