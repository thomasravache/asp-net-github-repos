﻿using RepositorioGitHub.Business;
using RepositorioGitHub.Business.Contract;
using RepositorioGitHub.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace RepositorioGitHub.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGitHubApiBusiness _business;
        public HomeController(IGitHubApiBusiness business)
        {
            _business = business;
        }
        public async Task<ActionResult> Index()
        {
            
            var model = await _business.Get();
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }
            
            return View(model);
        }

        public async Task<ActionResult> Details(long id)
        {
         
            var model = await _business.GetById(id);
            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetRepositorie(ActionResult<RepositoryViewModel> view)
        {
            ActionResult<RepositoryViewModel> model = new ActionResult<RepositoryViewModel>();
            if (string.IsNullOrEmpty(view.Result?.Name))
            {
                model.IsValid = false;
                model.Message = "O Campo Nome Repositório tem que ser Preenchido";
                TempData["warning"] = model.Message;
                return View(model);
            }

             model = await _business.GetByName(view.Result.Name);

            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetRepositorie()
        {
            ActionResult<RepositoryViewModel> model = new ActionResult<RepositoryViewModel>();

            return View(model);
        }

        public async Task<ActionResult> DetailsRepository(string name, string login)
        {
            ActionResult<GitHubRepositoryViewModel> model = new ActionResult<GitHubRepositoryViewModel>();

            if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(name))
            {
                return RedirectToAction("GetRepositorie");
            }
            else
            {
                
                model = await _business.GetRepository(login, name);

                if (model.IsValid)
                {
                    TempData["success"] = model.Message;
                }
                else
                {
                    TempData["warning"] = model.Message;
                }
            }
            
            return PartialView(model);
        }

        public ActionResult Favorite()
        {

            ActionResult<FavoriteViewModel> model = new ActionResult<FavoriteViewModel>();

            var response = _business.GetFavoriteRepository();
           
            model = response;

            if (model.IsValid)
            {
                TempData["success"] = model.Message;
            }
            else
            {
                TempData["warning"] = model.Message;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult FavoriteSave(string owner, string name, string language, string lastUpdat, string description)
        {     
            ActionResult< FavoriteViewModel> model = new ActionResult<FavoriteViewModel>();

            if(string.IsNullOrEmpty(owner) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(language)
                && string.IsNullOrEmpty(lastUpdat)&& string.IsNullOrEmpty(description))
            {
                model.IsValid = false;
                model.Message = "Não foi possivel realizar esta operação";

                
                

                return Json(new
                {
                    Data = model
                }, JsonRequestBehavior.AllowGet);


            }
            else
            {
                
                FavoriteViewModel view = new FavoriteViewModel() 
                { 
                    Description = description,
                    Language = language,
                    Owner = owner,
                    UpdateLast =  DateTime.Parse(lastUpdat),
                    Name = name
                    
                };

              var response = _business.SaveFavoriteRepository(view);

                model = response;

                if (model.IsValid)
                {
                    TempData["success"] = model.Message;
                }
                else
                {
                    TempData["warning"] = model.Message;
                }

                return Json(new
                {
                    Data = response
                }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}