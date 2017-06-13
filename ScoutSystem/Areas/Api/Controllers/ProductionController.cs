using ScoutSystem.Areas.Api.Models;
using ScoutSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ScoutSystem.Areas.Api.Controllers
{
    public class ProductionController : ApiController
    {
        private ScoutSystemDevEntities db = new ScoutSystemDevEntities();

        [HttpGet]
        public JsonResponse GetProductions()
        {
            var prods = new List<Models.Production>();
            foreach (var item in db.Production.ToArray())
            {
                prods.Add(new Models.Production(item));
            }
            return new JsonResponse(true, prods.ToArray().OrderBy(m => m.Name));
        }

        [HttpPost]
        public JsonResponse CreateProduction(ProductionNameIn model)
        {
            bool success = true;
            string error = null;
            model.Name = model.Name.ToUpper();

            //Get All productions
            var productions = db.Production.ToList();

            //Check if prod exists (Case insensitive)
            foreach (var prod in productions)
            {
                if (String.Equals(prod.Name, model.Name, StringComparison.OrdinalIgnoreCase))
                {
                    error = "Production already exists.";
                    success = false;
                    break;
                }
            }

            if (success)
            {
                db.Production.Add(new Entities.Production() { Name = model.Name });
                db.SaveChanges();
            }

            return new JsonResponse(success, error);
        }

        [HttpPost]
        public JsonResponse RemoveUsers(int ID)
        {
            var users = db.UserInfo.Where(m => m.ProductionID == ID);
            var count = users.Count();
            var unassignedID = db.Production.Where(m => m.Name == "Unassigned").First().ProductionID;

            foreach (var item in users)
            {
                item.ProductionID = unassignedID;
            }

            db.SaveChanges();
            return new JsonResponse(true, count);
        }
    }
}
