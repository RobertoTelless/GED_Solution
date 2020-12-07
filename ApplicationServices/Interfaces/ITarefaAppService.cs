using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntitiesServices.Model;

namespace ApplicationServices.Interfaces
{
    public interface ITarefaAppService : IAppServiceBase<TAREFA>
    {
        Int32 ValidateCreate(TAREFA tarefa, USUARIO usuario);
        Int32 ValidateEdit(TAREFA tarefa, TAREFA tarefaAntes, USUARIO usuario);
        Int32 ValidateEdit(TAREFA tarefa, TAREFA tarefaAntes);
        Int32 ValidateDelete(TAREFA tarefa, USUARIO usuario);
        Int32 ValidateReativar(TAREFA tarefa, USUARIO usuario);

        TAREFA CheckExist(TAREFA tarefa, Int32 idUsu);
        TAREFA GetItemById(Int32 id);
        List<TAREFA> GetByDate(DateTime data, Int32 idAss);
        List<TAREFA> GetByUser(Int32 user);
        List<TAREFA> GetTarefaStatus(Int32 user, Int32 tipo);
        List<TAREFA> GetAllItens(Int32 idAss);
        List<TAREFA> GetAllItensAdm(Int32 idAss);
        List<USUARIO> GetAllUsers(Int32 idAss);
        List<TIPO_TAREFA> GetAllTipos(Int32 idAss);
        TAREFA_ANEXO GetAnexoById(Int32 id);
        USUARIO GetUserById(Int32 id);

        Int32 ExecuteFilter(Int32? tipoId, String titulo, DateTime? data, Int32 encerrada, Int32 prioridade, Int32 idAss, out List<TAREFA> objeto);
    }
}
