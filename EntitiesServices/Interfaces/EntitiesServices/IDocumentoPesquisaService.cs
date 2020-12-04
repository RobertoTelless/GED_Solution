using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IDocumentoPesquisaService : IServiceBase<DOCUMENTO_PESQUISA>
    {
        Int32 Create(DOCUMENTO_PESQUISA item, LOG log);
        Int32 Create(DOCUMENTO_PESQUISA item);
        Int32 Edit(DOCUMENTO_PESQUISA item, LOG log);
        Int32 Edit(DOCUMENTO_PESQUISA item);
        Int32 Delete(DOCUMENTO_PESQUISA item, LOG log);

        DOCUMENTO_PESQUISA GetItemById(Int32 id);
        List<DOCUMENTO_PESQUISA> GetAllItens(Int32 idAss);
        List<DOCUMENTO_PESQUISA> GetAllItensAdm(Int32 idAss);
        List<DOCUMENTO_PESQUISA> ExecuteFilter(Int32? idUsuario, Int32? doc, DateTime? data, Int32? idAss);
    }
}
