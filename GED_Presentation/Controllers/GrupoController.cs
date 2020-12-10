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
    public class GrupoController : Controller
    {
        private readonly IGrupoAppService gruApp;
        private readonly IUsuarioAppService usuApp;
        private readonly ITipoGrupoAppService tgApp;

        private String msg;
        private Exception exception;
        GRUPO objetoGru = new GRUPO();
        GRUPO objetoGruAntes = new GRUPO();
        List<GRUPO> listaMasterGru = new List<GRUPO>();
        String extensao;

        public GrupoController(IGrupoAppService gruApps, IUsuarioAppService usuApps, ITipoGrupoAppService tgApps)
        {
            gruApp = gruApps;
            usuApp = usuApps;
            tgApp = tgApps;
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
        public ActionResult MontarTelaGrupo()
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
                    Session["MensGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            if ((List<GRUPO>)Session["ListaGrupo"] == null)
            {
                listaMasterGru = gruApp.GetAllItens(idAss);
                Session["ListaGrupo"] = listaMasterGru;
            }
            ViewBag.Listas = (List<GRUPO>)Session["ListaGrupo"];
            ViewBag.Title = "Grupos";

            // Indicadores
            ViewBag.Grupos = ((List<GRUPO>)Session["ListaGrupo"]).Count;
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensGrupo"] == 1)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensGrupo"] == 3)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensGrupo"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0031", CultureInfo.CurrentCulture));
            }

            // Abre view
            objetoGru = new GRUPO();
            Session["VoltaGrupo"] = 1;
            Session["MensGrupo"] = 0;
            return View(objetoGru);
        }

        public ActionResult MostrarTudoGrupo()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterGru = gruApp.GetAllItensAdm(idAss);
            Session["ListaGrupo"] = listaMasterGru;
            return RedirectToAction("MontarTelaGrupo");
        }

        public ActionResult VoltarBaseGrupo()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaGrupo"] = gruApp.GetAllItens(idAss);
            if ((Int32)Session["VoltaGrupo"] == 2)
            {
                return RedirectToAction("IncluirCC", "CentroCusto");
            }
            if ((Int32)Session["VoltaGrupo"] == 3)
            {
                return RedirectToAction("EditarCC", "CentroCusto");
            }
            return RedirectToAction("MontarTelaGrupo");
        }

        [HttpGet]
        public ActionResult IncluirGrupo(Int32? id)
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
                    Session["MensGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            GRUPO item = new GRUPO();
            if (id != null)
            {
                Session["VoltaGrupo"] = id.Value;
            }
            GrupoViewModel vm = Mapper.Map<GRUPO, GrupoViewModel>(item);
            vm.ASSI_CD_ID = usuario.ASSI_CD_ID;
            vm.GRUP_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirGrupo(GrupoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    GRUPO item = Mapper.Map<GrupoViewModel, GRUPO>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    Int32 volta = gruApp.ValidateCreate(item, usuarioLogado);

                    // Retorno
                    if (volta == 1)
                    {
                        Session["MensGrupo"] = 2;
                        ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0031", CultureInfo.CurrentCulture));
                        return RedirectToAction("MontarTelaGrupo");
                    }

                    // Sucesso
                    listaMasterGru = new List<GRUPO>();
                    Session["MensGrupo"] = 0;
                    Session["ListaGrupo"] = null;
                    return RedirectToAction("MontarTelaGrupo");
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
        public ActionResult EditarGrupo(Int32 id)
        {
            GRUPO item = gruApp.GetItemById(id);
            objetoGruAntes = item;
            Session["Grupo"] = item;
            Session["idVolta"] = id;
            GrupoViewModel vm = Mapper.Map<GRUPO, GrupoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarGrupo(GrupoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    GRUPO item = Mapper.Map<GrupoViewModel, GRUPO>(vm);
                    Int32 volta = gruApp.ValidateEdit(item, objetoGruAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMasterGru = new List<GRUPO>();
                    Session["ListaGrupo"] = null;
                    return RedirectToAction("MontarTelaGrupo");
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
        public ActionResult ExcluirGrupo(Int32 id)
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
                    Session["MensGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            GRUPO item = gruApp.GetItemById(id);
            objetoGruAntes = item;
            item.GRUP_IN_ATIVO = 0;
            Int32 volta = gruApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensGrupo"] = 3;
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
            }
            listaMasterGru = new List<GRUPO>();
            Session["ListaGrupo"] = null;
            return RedirectToAction("MontarTelaGrupo");
        }

        [HttpGet]
        public ActionResult ReativarGrupo(Int32 id)
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
                    Session["MensGrupo"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            GRUPO item = gruApp.GetItemById(id);
            objetoGruAntes = item;
            item.GRUP_IN_ATIVO = 1;
            Int32 volta = gruApp.ValidateReativar(item, usuario);
            listaMasterGru = new List<GRUPO>();
            Session["ListaGrupo"] = null;
            return RedirectToAction("MontarTelaGrupo");
        }
    }
}