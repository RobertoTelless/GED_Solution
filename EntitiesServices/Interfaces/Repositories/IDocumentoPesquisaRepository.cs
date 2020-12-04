using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IDocumentoPesquisaRepository : IRepositoryBase<DOCUMENTO_PESQUISA>
    {
        DOCUMENTO_PESQUISA GetItemById(Int32 id);
        List<DOCUMENTO_PESQUISA> GetAllItens(Int32 idAss);
        List<DOCUMENTO_PESQUISA> GetAllItensAdm(Int32 idAss);
        List<DOCUMENTO_PESQUISA> ExecuteFilter(Int32? idUsuario, Int32? doc, DateTime? data, Int32? idAss);
    }
}
