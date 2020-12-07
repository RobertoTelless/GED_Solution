using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IMetadadoClasseAppService : IAppServiceBase<METADADO_CLASSE>
    {
        Int32 ValidateCreate(METADADO_CLASSE tarefa, USUARIO usuario);
        Int32 ValidateEdit(METADADO_CLASSE tarefa, METADADO_CLASSE tarefaAntes, USUARIO usuario);
        Int32 ValidateEdit(METADADO_CLASSE tarefa, METADADO_CLASSE tarefaAntes);
        Int32 ValidateDelete(METADADO_CLASSE tarefa, USUARIO usuario);
        Int32 ValidateReativar(METADADO_CLASSE tarefa, USUARIO usuario);

        METADADO_CLASSE CheckExist(METADADO_CLASSE item, Int32? idAss);
        METADADO_CLASSE GetItemById(Int32 id);
        List<METADADO_CLASSE> GetAllItens(Int32 idAss);
        List<METADADO_CLASSE> GetAllItensAdm(Int32 idAss);
        List<TIPO_METADADO> GetAllTipos(Int32 idAss);
        List<CLASSE> GetAllClasses(Int32 idAss);

    }
}
