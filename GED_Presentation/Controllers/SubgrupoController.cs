using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationServices.Interfaces;
using EntitiesServices.Model;
using System.Globalization;
using EntitiesServices.Work_Classes;
using GED_Presentation.App_Start;
using AutoMapper;
using Ged.ViewModels;
using System.IO;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GED_Presentation.Controllers
{
    public class SubgrupoController : Controller
    {
        private readonly ISubgrupoAppService subApp;
        private readonly IUsuarioAppService usuApp;
        private readonly IGrupoAppService gruApp;

        private String msg;
        private Exception exception;
        SUBGRUPO objetoSub = new SUBGRUPO();
        SUBGRUPO objetoSubAntes = new SUBGRUPO();
        List<SUBGRUPO> listaMasterSub = new List<SUBGRUPO>();
        String extensao;

        public SubgrupoController(ISubgrupoAppService subApps, IUsuarioAppService usuApps, IGrupoAppService gruApps)
        {
            subApp = subApps;
            usuApp = usuApps;
            gruApp = gruApps;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Voltar()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult VoltarGeral()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarBase", "BaseAdmin");
        }

        public ActionResult DashboardAdministracao()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return RedirectToAction("CarregarAdmin", "BaseAdmin");
        }

       [HttpGet]
        public ActionResult MontarTelaSubGrupo()
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensSubGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            if ((List<SUBGRUPO>)Session["ListaSubGrupo"] == null)
            {
                listaMasterSub = subApp.GetAllItens(idAss);
                Session["ListaSubGrupo"] = listaMasterSub;
            }
            ViewBag.Listas = (List<SUBGRUPO>)Session["ListaSubGrupo"];
            ViewBag.Title = "SubGrupos";

            // Indicadores
            ViewBag.SubGrupos = ((List<SUBGRUPO>)Session["ListaSubGrupo"]).Count;
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensSubGrupo"] == 1)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSubGrupo"] == 3)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0034", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSubGrupo"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0033", CultureInfo.CurrentCulture));
            }

            // Abre view
            objetoSub = new SUBGRUPO();
            Session["VoltaSubGrupo"] = 1;
            Session["MensSubGrupo"] = 0;
            return View(objetoSub);
        }

        public ActionResult MostrarTudoSubGrupo()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterSub = subApp.GetAllItensAdm(idAss);
            Session["ListaSubGrupo"] = listaMasterSub;
            return RedirectToAction("MontarTelaSubGrupo");
        }

        public ActionResult VoltarBaseSubGrupo()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaSubGrupo"] = gruApp.GetAllItens(idAss);
            return RedirectToAction("MontarTelaSubGrupo");
        }

        [HttpGet]
        public ActionResult IncluirSubGrupo(Int32? id)
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensSubGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            SUBGRUPO item = new SUBGRUPO();
            if (id != null)
            {
                Session["VoltaGrupo"] = id.Value;
            }
            ViewBag.Grupos = new SelectList(gruApp.GetAllItens(idAss), "GRUP_CD_ID", "GR_NM_EXIBE");
            SubgrupoViewModel vm = Mapper.Map<SUBGRUPO, SubgrupoViewModel>(item);
            vm.ASSI_CD_ID = usuario.ASSI_CD_ID;
            vm.SUBG_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirSubGrupo(SubgrupoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Grupos = new SelectList(gruApp.GetAllItens(idAss), "GRUP_CD_ID", "GR_NM_EXIBE");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    SUBGRUPO item = Mapper.Map<SubgrupoViewModel, SUBGRUPO>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 volta = subApp.ValidateCreate(item, usuarioLogado);

                    // Retorno
                    if (volta == 1)
                    {
                        Session["MensSubGrupo"] = 2;
                        ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0033", CultureInfo.CurrentCulture));
                        return RedirectToAction("MontarTelaSubGrupo");
                    }

                    // Sucesso
                    Session["MensSubGrupo"] = 0;
                    listaMasterSub = new List<SUBGRUPO>();
                    Session["ListaSubGrupo"] = null;
                    return RedirectToAction("MontarTelaSubGrupo");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult EditarSubGrupo(Int32 id)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Grupos = new SelectList(gruApp.GetAllItens(idAss), "GRUP_CD_ID", "GR_NM_EXIBE");
            SUBGRUPO item = subApp.GetItemById(id);
            objetoSubAntes = item;
            Session["SubGrupo"] = item;
            Session["idVolta"] = id;
            SubgrupoViewModel vm = Mapper.Map<SUBGRUPO, SubgrupoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarSubGrupo(SubgrupoViewModel vm)
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            ViewBag.Grupos = new SelectList(gruApp.GetAllItens(idAss), "GRUP_CD_ID", "GR_NM_EXIBE");
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    SUBGRUPO item = Mapper.Map<SubgrupoViewModel, SUBGRUPO>(vm);
                    Int32 volta = subApp.ValidateEdit(item, objetoSubAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMasterSub = new List<SUBGRUPO>();
                    Session["ListaSubGrupo"] = null;
                    return RedirectToAction("MontarTelaSubGrupo");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(vm);
                }
            }
            else
            {
                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult ExcluirSubGrupo(Int32 id)
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensSubGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            SUBGRUPO item = subApp.GetItemById(id);
            objetoSubAntes = item;
            item.SUBG_IN_ATIVO = 0;
            Int32 volta = subApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensGrupo"] = 3;
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0034", CultureInfo.CurrentCulture));
            }
            listaMasterSub = new List<SUBGRUPO>();
            Session["ListaSubGrupo"] = null;
            return RedirectToAction("MontarTelaSubGrupo");
        }

        [HttpGet]
        public ActionResult ReativarSubGrupo(Int32 id)
        {
            // Verifica se tem usuario logado
            USUARIO usuario = new USUARIO();
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            if ((USUARIO)Session["UserCredentials"] != null)
            {
                usuario = (USUARIO)Session["UserCredentials"];

                // Verfifica permissão
                if (usuario.PERFIL.PERF_SG_SIGLA != "ADM")
                {
                    Session["MensSubGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            SUBGRUPO item = subApp.GetItemById(id);
            objetoSubAntes = item;
            item.SUBG_IN_ATIVO = 1;
            Int32 volta = subApp.ValidateReativar(item, usuario);
            listaMasterSub = new List<SUBGRUPO>();
            Session["ListaSubGrupo"] = null;
            return RedirectToAction("MontarTelaSubGrupo");
        }
    }
}