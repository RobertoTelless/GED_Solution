using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface IClasseAppService : IAppServiceBase<CLASSE>
    {
        Int32 ValidateCreate(CLASSE tarefa, USUARIO usuario);
        Int32 ValidateEdit(CLASSE tarefa, CLASSE tarefaAntes, USUARIO usuario);
        Int32 ValidateEdit(CLASSE tarefa, CLASSE tarefaAntes);
        Int32 ValidateDelete(CLASSE tarefa, USUARIO usuario);
        Int32 ValidateReativar(CLASSE tarefa, USUARIO usuario);

        List<CLASSE> GetAllItens(Int32 idAss);
        List<CLASSE> GetAllItensAdm(Int32 idAss);
        CLASSE GetItemById(Int32 id);
        CLASSE CheckExist(CLASSE tarefa, Int32 idUsu);
        List<SUBGRUPO> GetAllSubgrupos(Int32? idAss);

        Int32 ExecuteFilter(Int32? subgrupo, String nome, String descricao, String sigla, Int32 idAss, out List<CLASSE> objeto);
    }
}
