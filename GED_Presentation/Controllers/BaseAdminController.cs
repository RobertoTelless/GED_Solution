﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GED_Presentation.Controllers
{
    public class BaseAdminController : Controller
    {
        // GET: BaseAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}