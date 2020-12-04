using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;
using EntitiesServices.Work_Classes;

namespace ModelServices.Interfaces.EntitiesServices
{
    public interface IDocumentoService : IServiceBase<DOCUMENTO>
    {
        Int32 Create(DOCUMENTO item, LOG log);
        Int32 Create(DOCUMENTO item);
        Int32 Edit(DOCUMENTO item, LOG log);
        Int32 Edit(DOCUMENTO item);
        Int32 Delete(DOCUMENTO item, LOG log);

        DOCUMENTO CheckExist(DOCUMENTO item, Int32? idAss);
        DOCUMENTO GetItemById(Int32 id);
        List<DOCUMENTO> GetAllItens(Int32 idAss);
        List<DOCUMENTO> GetAllItensAdm(Int32 idAss);
        List<DOCUMENTO> ExecuteFilter(Int32? classe, Int32? idCat, Int32? idUsuario, String nome, DateTime? data, Int32? idFormato, String descricao, String ocr, Int32 idAss);
        List<TIPO_DOCUMENTO> GetAllTipos(Int32 idAss);
    }
}
