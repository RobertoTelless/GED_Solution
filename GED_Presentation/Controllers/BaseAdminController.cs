﻿using System;
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

namespace GED_Presentation.Controllers
{
    public class BaseAdminController : Controller
    {
        private readonly IUsuarioAppService baseApp;
        private readonly INoticiaAppService notiApp;
        private readonly ILogAppService logApp;
        private readonly ITarefaAppService tarApp;
        private readonly INotificacaoAppService notfApp;
        private readonly IUsuarioAppService usuApp;
        private readonly IAgendaAppService ageApp;
        private readonly IConfiguracaoAppService confApp;
        private readonly ITipoPessoaAppService tpApp;

        private String msg;
        private Exception exception;
        USUARIO objeto = new USUARIO();
        USUARIO objetoAntes = new USUARIO();
        List<USUARIO> listaMaster = new List<USUARIO>();

        public BaseAdminController(IUsuarioAppService baseApps, ILogAppService logApps, INoticiaAppService notApps, ITarefaAppService tarApps, INotificacaoAppService notfApps, IUsuarioAppService usuApps, IAgendaAppService ageApps, IConfiguracaoAppService confApps, ITipoPessoaAppService tpApps)
        {
            baseApp = baseApps;
            logApp = logApps;
            notiApp = notApps;
            tarApp = tarApps;
            notfApp = notfApps;
            usuApp = usuApps;
            ageApp = ageApps;
            confApp = confApps;
            tpApp = tpApps;
        }

        public ActionResult CarregarAdmin()
        {
            Int32? idAss = (Int32)Session["IdAssinante"];
            ViewBag.Usuarios = baseApp.GetAllUsuarios(idAss.Value).Count;
            ViewBag.Logs = logApp.GetAllItens(idAss.Value).Count;
            ViewBag.UsuariosLista = baseApp.GetAllUsuarios(idAss.Value);
            ViewBag.LogsLista = logApp.GetAllItens(idAss.Value);
            return View();
        }

        public ActionResult CarregarLandingPage()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }
            return View();
        }

        public JsonResult GetRefreshTime()
        {
            return Json(confApp.GetById(1).CONF_NR_REFRESH_DASH);
        }

        public JsonResult GetConfigNotificacoes()
        {
            Int32? idAss = (Int32)Session["IdAssinante"];
            bool hasNotf;
            var hash = new Hashtable();
            USUARIO usu = (USUARIO)Session["Usuario"];
            CONFIGURACAO conf = confApp.GetById(1);

            if (baseApp.GetAllItensUser(usu.USUA_CD_ID, idAss.Value).Count > 0)
            {
                hasNotf = true;
            }
            else
            {
                hasNotf = false;
            }

            hash.Add("CONF_NM_ARQUIVO_ALARME", conf.CONF_NM_ARQUIVO_ALARME);
            hash.Add("NOTIFICACAO", hasNotf);
            return Json(hash);
        }

        public ActionResult CarregarBase()
        {
            if ((String)Session["Ativa"] == null)
            {
                return RedirectToAction("Login", "ControleAcesso");
            }

            // Carrega listas
            Int32? idAss = (Int32)Session["IdAssinante"];
            if ((Int32)Session["Login"] == 1)
            {
                Session["Perfis"] = baseApp.GetAllPerfis();
                Session["Usuarios"] = usuApp.GetAllUsuarios(idAss.Value);
                Session["UFs"] = usuApp.GetAllUF();
                Session["TiposPessoas"] = tpApp.GetAllItens();
            }
            Session["MensTarefa"] = 0;
            Session["MensNoticia"] = 0;
            Session["MensNotificacao"] = 0;
            Session["MensUsuario"] = 0;
            Session["MensLog"] = 0;
            Session["MensUsuarioAdm"] = 0;
            Session["MensAgenda"] = 0;
            Session["MensTemplate"] = 0;
            Session["MensConfiguracao"] = 0;
            Session["MensTelefone"] = 0;
            Session["MensGrupo"] = 0;
            Session["MensSubGrupo"] = 0;
            Session["MensTab"] = 0;

            USUARIO usu = (USUARIO)Session["Usuario"];
            UsuarioViewModel vm = Mapper.Map<USUARIO, UsuarioViewModel>(usu);

            List<NOTIFICACAO> lista = usu.NOTIFICACAO.ToList();
            Session["Notificacoes"] = lista;
            Session["ListaNovas"] = lista.Where(p => p.NOTI_IN_VISTA == 0).ToList();
            ViewBag.ListaNovas = (List<NOTIFICACAO>)Session["ListaNovas"];
            Session["NovasNotificacoes"] = lista.Where(p => p.NOTI_IN_VISTA == 0).Count();
            Session["Nome"] = usu.USUA_NM_NOME;
            ViewBag.NovasNotificacoes = lista.Where(p => p.NOTI_IN_VISTA == 0).Count();
            Session["VoltaNotificacao"] = 3;

            List<NOTICIA> lista1 = notiApp.GetAllItensValidos(idAss.Value);
            Session["Noticias"] = lista1;
            Session["NoticiasNumero"] = lista1.Count;
            ViewBag.NoticiasNumero = lista1.Count;
            ViewBag.Noticias = lista1;

            List<TAREFA> lista2 = tarApp.GetTarefaStatus(usu.USUA_CD_ID, 1);
            Session["ListaPendentes"]  = lista2;
            Session["TarefasPendentes"]  = lista2.Count;
            List<TAREFA> lista3 = tarApp.GetByUser(usu.USUA_CD_ID);
            Session["TarefasLista"] = lista3;
            Session["Tarefas"] = lista3.Count;
            ViewBag.TarefasPendentes = lista2.Count;
            ViewBag.Tarefas= lista3.Count;

            List<AGENDA> lista4 = usu.AGENDA.ToList();
            Session["Agendas"] = lista4;
            Session["NumAgendas"] = lista4.Count;
            Session["AgendasHoje"] = lista4.Where(p => p.AGEN_DT_DATA == DateTime.Today.Date).ToList();
            Session["NumAgendasHoje"] = lista4.Where(p => p.AGEN_DT_DATA == DateTime.Today.Date).ToList().Count;
            ViewBag.NumAgendas = lista4.Count;
            ViewBag.NumAgendasHoje = lista4.Where(p => p.AGEN_DT_DATA == DateTime.Today.Date).ToList().Count;

            Session["Logs"] = usu.LOG.Count;
            ViewBag.Logs = usu.LOG.Count;

            String frase = String.Empty;
            String nome = usu.USUA_NM_NOME.Substring(0, usu.USUA_NM_NOME.IndexOf(" "));
            if (DateTime.Now.Hour <= 12)
            {
                frase = "Bom dia, " + nome;
            }
            else if (DateTime.Now.Hour > 12 & DateTime.Now.Hour <= 18)
            {
                frase = "Boa tarde, " + nome;
            }
            else
            {
                frase = "Boa noite, " + nome;
            }
            Session["Greeting"] = frase;
            Session["Foto"] = usu.USUA_AQ_FOTO;
            Session["ErroSoma"] = 0;
            ViewBag.Greeting = frase;
            ViewBag.Foto = usu.USUA_AQ_FOTO;

            // Mensagens
            if ((Int32)Session["MensNotificacao"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensNoticia"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensUsuario"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensLog"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensUsuarioAdm"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTemplate"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensConfiguracao"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensGrupo"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensSubGrupo"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            if ((Int32)Session["MensTab"] == 2)
            {
                ModelState.AddModelError("", GED_Resources.ResourceManager.GetString("M0011", CultureInfo.CurrentCulture));
            }
            return View(vm);
        }

        public ActionResult CarregarDesenvolvimento()
        {
            return View();
        }

        public ActionResult VoltarDashboard()
        {
            return RedirectToAction("CarregarDashboardInicial", "BaseAdmin");
        }

        public ActionResult MontarFaleConosco()
        {
            return View();
        }
    }
}