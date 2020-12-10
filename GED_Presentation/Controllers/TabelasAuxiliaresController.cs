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
    public class TabelasAuxiliaresController : Controller
    {
        private readonly ICategoriaAgendaAppService caApp;
        private readonly ICategoriaNotificacaoAppService cnApp;
        private readonly ICategoriaTelefoneAppService ctApp;
        private readonly ICategoriaUsuarioAppService cuApp;
        private readonly ITipoDocumentoAppService tdApp;
        private readonly ITipoGrupoAppService tgApp;
        private readonly ITipoMetadadoAppService tmApp;

        private String msg;
        private Exception exception;
        CATEGORIA_AGENDA objetoCa = new CATEGORIA_AGENDA();
        CATEGORIA_AGENDA objetoCaAntes = new CATEGORIA_AGENDA();
        List<CATEGORIA_AGENDA> listaMasterCa = new List<CATEGORIA_AGENDA>();
        CATEGORIA_NOTIFICACAO objetoCn = new CATEGORIA_NOTIFICACAO();
        CATEGORIA_NOTIFICACAO objetoCnAntes = new CATEGORIA_NOTIFICACAO();
        List<CATEGORIA_NOTIFICACAO> listaMasterCn= new List<CATEGORIA_NOTIFICACAO>();
        CATEGORIA_TELEFONE objetoCt = new CATEGORIA_TELEFONE();
        CATEGORIA_TELEFONE objetoCtAntes = new CATEGORIA_TELEFONE();
        List<CATEGORIA_TELEFONE> listaMasterCt = new List<CATEGORIA_TELEFONE>();
        CATEGORIA_USUARIO objetoCu = new CATEGORIA_USUARIO();
        CATEGORIA_USUARIO objetoCuAntes = new CATEGORIA_USUARIO();
        List<CATEGORIA_USUARIO> listaMasterCu = new List<CATEGORIA_USUARIO>();
        TIPO_DOCUMENTO objetoTd = new TIPO_DOCUMENTO();
        TIPO_DOCUMENTO objetoTdAntes = new TIPO_DOCUMENTO();
        List<TIPO_DOCUMENTO> listaMasterTd = new List<TIPO_DOCUMENTO>();
        TIPO_GRUPO objetoTg = new TIPO_GRUPO();
        TIPO_GRUPO objetoTgAntes = new TIPO_GRUPO();
        List<TIPO_GRUPO> listaMasterTg = new List<TIPO_GRUPO>();
        TIPO_METADADO objetoTm = new TIPO_METADADO();
        TIPO_METADADO objetoTmAntes = new TIPO_METADADO();
        List<TIPO_METADADO> listaMasterTm = new List<TIPO_METADADO>();

        String extensao;

        public TabelasAuxiliaresController(ICategoriaAgendaAppService caApps, ICategoriaNotificacaoAppService cnApps, ICategoriaTelefoneAppService ctApps, ICategoriaUsuarioAppService cuApps, ITipoDocumentoAppService tdApps, ITipoGrupoAppService tgApps, ITipoMetadadoAppService tmApps)
        {
            caApp = caApps;
            cnApp = cnApps;
            ctApp = ctApps;
            cuApp = cuApps;
            tdApp = tdApps;
            tgApp = tgApps;
            tmApp = tmApps;
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
        public ActionResult MontarTelaCatAgenda()
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            if ((List<CATEGORIA_AGENDA>)Session["ListaCatAgenda"] == null)
            {
                listaMasterCa = caApp.GetAllItens(idAss);
                Session["ListaCatAgenda"] = listaMasterCa;
            }
            ViewBag.Listas = (List<CATEGORIA_AGENDA>)Session["ListaCatAgenda"];
            ViewBag.Title = "Cat.Agenda";

            // Indicadores
            ViewBag.Tipos = ((List<CATEGORIA_AGENDA>)Session["ListaCatAgenda"]).Count;
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensTab"] == 1)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTab"] == 3)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTab"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0031", CultureInfo.CurrentCulture));
            }

            // Abre view
            objetoCa = new CATEGORIA_AGENDA();
            Session["VoltaCa"] = 1;
            Session["MensTab"] = 0;
            return View(objetoCa);
        }

        public ActionResult MostrarTudoCatAgenda()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterCa = caApp.GetAllItensAdm(idAss);
            Session["ListaCatAgenda"] = listaMasterCa;
            return RedirectToAction("MontarTelaCatAgenda");
        }

        public ActionResult VoltarBaseCatAgenda()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaCatAgenda"] = null;
            return RedirectToAction("MontarTelaCatAgenda");
        }

        [HttpGet]
        public ActionResult IncluirCatAgenda(Int32? id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            CATEGORIA_AGENDA item = new CATEGORIA_AGENDA();
            if (id != null)
            {
                Session["VoltaCA"] = id.Value;
            }
            CategoriaAgendaViewModel vm = Mapper.Map<CATEGORIA_AGENDA, CategoriaAgendaViewModel>(item);
            vm.ASSI_CD_ID = usuario.ASSI_CD_ID;
            vm.CAAG_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirCatAgenda(CategoriaAgendaViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CATEGORIA_AGENDA item = Mapper.Map<CategoriaAgendaViewModel, CATEGORIA_AGENDA>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    Int32 volta = caApp.ValidateCreate(item, usuarioLogado);

                    // Retorno
                    if (volta == 1)
                    {
                        Session["MensTab"] = 2;
                        ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0035", CultureInfo.CurrentCulture));
                        return RedirectToAction("MontarTelaCatAgenda");
                    }

                    // Sucesso
                    listaMasterCa = new List<CATEGORIA_AGENDA>();
                    Session["MensTab"] = 0;
                    Session["ListaCatAgenda"] = null;
                    return RedirectToAction("MontarTelaCatAgenda");
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
        public ActionResult EditarCatAgenda(Int32 id)
        {
            CATEGORIA_AGENDA item = caApp.GetItemById(id);
            objetoCaAntes = item;
            Session["CatAgenda"] = item;
            Session["idVolta"] = id;
            CategoriaAgendaViewModel vm = Mapper.Map<CATEGORIA_AGENDA, CategoriaAgendaViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarCatAgenda(CategoriaAgendaViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    CATEGORIA_AGENDA item = Mapper.Map<CategoriaAgendaViewModel, CATEGORIA_AGENDA>(vm);
                    Int32 volta = caApp.ValidateEdit(item, objetoCaAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMasterCa = new List<CATEGORIA_AGENDA>();
                    Session["ListaCatAgenda"] = null;
                    return RedirectToAction("MontarTelaCatAgenda");
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
        public ActionResult ExcluirCatAgenda(Int32 id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            CATEGORIA_AGENDA item = caApp.GetItemById(id);
            objetoCaAntes = item;
            item.CAAG_IN_ATIVO = 0;
            Int32 volta = caApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensTab"] = 3;
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0036", CultureInfo.CurrentCulture));
            }
            listaMasterCa = new List<CATEGORIA_AGENDA>();
            Session["ListaCatAgenda"] = null;
            return RedirectToAction("MontarTelaCatAgenda");
        }

        [HttpGet]
        public ActionResult ReativarCatAgenda(Int32 id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            CATEGORIA_AGENDA item = caApp.GetItemById(id);
            objetoCaAntes = item;
            item.CAAG_IN_ATIVO = 1;
            Int32 volta = caApp.ValidateReativar(item, usuario);
            listaMasterCa = new List<CATEGORIA_AGENDA>();
            Session["ListaCatAgenda"] = null;
            return RedirectToAction("MontarTelaCatAgenda");
        }

        [HttpGet]
        public ActionResult MontarTelaCatNotificacao()
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32 idAss = (Int32)Session["IdAssinante"];
            if ((List<CATEGORIA_NOTIFICACAO>)Session["ListaCatNotificacao"] == null)
            {
                listaMasterCn = cnApp.GetAllItens(idAss);
                Session["ListaCatNotificacao"] = listaMasterCn;
            }
            ViewBag.Listas = (List<CATEGORIA_NOTIFICACAO>)Session["ListaCatNotificacao"];
            ViewBag.Title = "Cat.Notificacao";

            // Indicadores
            ViewBag.Tipos = ((List<CATEGORIA_NOTIFICACAO>)Session["ListaCatNotificacao"]).Count;
            ViewBag.Perfil = usuario.PERFIL.PERF_SG_SIGLA;

            // Mensagem
            if ((Int32)Session["MensTab"] == 1)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0016", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTab"] == 3)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0032", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTab"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0037", CultureInfo.CurrentCulture));
            }

            // Abre view
            objetoCn = new CATEGORIA_NOTIFICACAO();
            Session["VoltaCn"] = 1;
            Session["MensTab"] = 0;
            return View(objetoCn);
        }

        public ActionResult MostrarTudoCatNotificacao()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            listaMasterCn = cnApp.GetAllItensAdm(idAss);
            Session["ListaCatNotificacao"] = listaMasterCn;
            return RedirectToAction("MontarTelaCatNotificacao");
        }

        public ActionResult VoltarBaseCatNotificacao()
        {
            Int32 idAss = (Int32)Session["IdAssinante"];
            Session["ListaCatNotificacao"] = null;
            return RedirectToAction("MontarTelaCatNotificacao");
        }

        [HttpGet]
        public ActionResult IncluirCatNotificacao(Int32? id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Prepara view
            CATEGORIA_NOTIFICACAO item = new CATEGORIA_NOTIFICACAO();
            if (id != null)
            {
                Session["VoltaCN"] = id.Value;
            }
            CategoriaNotificacaoViewModel vm = Mapper.Map<CATEGORIA_NOTIFICACAO, CategoriaNotificacaoViewModel>(item);
            vm.ASSI_CD_ID = usuario.ASSI_CD_ID;
            vm.CANO_IN_ATIVO = 1;
            return View(vm);
        }

        [HttpPost]
        public ActionResult IncluirCatNotificacao(CategoriaNotificacaoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    CATEGORIA_NOTIFICACAO item = Mapper.Map<CategoriaNotificacaoViewModel, CATEGORIA_NOTIFICACAO>(vm);
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    Int32 volta = cnApp.ValidateCreate(item, usuarioLogado);

                    // Retorno
                    if (volta == 1)
                    {
                        Session["MensTab"] = 2;
                        ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0037", CultureInfo.CurrentCulture));
                        return RedirectToAction("MontarTelaCatNotificacao");
                    }

                    // Sucesso
                    listaMasterCn = new List<CATEGORIA_NOTIFICACAO>();
                    Session["MensTab"] = 0;
                    Session["ListaCatNotificacao"] = null;
                    return RedirectToAction("MontarTelaCatNotificacao");
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
        public ActionResult EditarCatNotificacao(Int32 id)
        {
            CATEGORIA_NOTIFICACAO item = cnApp.GetItemById(id);
            objetoCnAntes = item;
            Session["CatNotificacao"] = item;
            Session["idVolta"] = id;
            CategoriaNotificacaoViewModel vm = Mapper.Map<CATEGORIA_NOTIFICACAO, CategoriaNotificacaoViewModel>(item);
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditarCatNotificacao(CategoriaNotificacaoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Executa a operação
                    USUARIO usuarioLogado = (USUARIO)Session["UserCredentials"];
                    Int32 idAss = (Int32)Session["IdAssinante"];
                    CATEGORIA_NOTIFICACAO item = Mapper.Map<CategoriaNotificacaoViewModel, CATEGORIA_NOTIFICACAO>(vm);
                    Int32 volta = cnApp.ValidateEdit(item, objetoCnAntes, usuarioLogado);

                    // Verifica retorno

                    // Sucesso
                    listaMasterCn = new List<CATEGORIA_NOTIFICACAO>();
                    Session["ListaCatNotificacao"] = null;
                    return RedirectToAction("MontarTelaCatNotificacao");
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
        public ActionResult ExcluirCatNotificacao(Int32 id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            CATEGORIA_NOTIFICACAO item = cnApp.GetItemById(id);
            objetoCnAntes = item;
            item.CANO_IN_ATIVO = 0;
            Int32 volta = cnApp.ValidateDelete(item, usuario);
            if (volta == 1)
            {
                Session["MensTab"] = 3;
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0036", CultureInfo.CurrentCulture));
            }
            listaMasterCn = new List<CATEGORIA_NOTIFICACAO>();
            Session["ListaCatNotificacao"] = null;
            return RedirectToAction("MontarTelaCatNotificacao");
        }

        [HttpGet]
        public ActionResult ReativarCatNotificacao(Int32 id)
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
                    Session["MensTab"] = 2;
                    return RedirectToAction("CarregarBase", "BaseAdmin");
                }
            }
            else
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            Int32 idAss = (Int32)Session["IdAssinante"];

            // Executar
            CATEGORIA_NOTIFICACAO item = cnApp.GetItemById(id);
            objetoCnAntes = item;
            item.CANO_IN_ATIVO = 1;
            Int32 volta = cnApp.ValidateReativar(item, usuario);
            listaMasterCn = new List<CATEGORIA_NOTIFICACAO>();
            Session["ListaCatNotificacao"] = null;
            return RedirectToAction("MontarTelaCatNotificacao");
        }








    }
}