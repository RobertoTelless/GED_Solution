using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ModelServices.Interfaces.Repositories
{
    public interface IDocumentoRepository : IRepositoryBase<DOCUMENTO>
    {
        DOCUMENTO GetItemById(Int32 id);
        List<DOCUMENTO> GetAllItens(Int32 idAss);
        List<DOCUMENTO> GetAllItensAdm(Int32 idAss);
        List<DOCUMENTO> ExecuteFilter(Int32? classe, Int32? idCat, Int32? idUsuario, String nome, DateTime? data, Int32? idFormato, String descricao, String ocr, Int32 idAss);
    }
}
