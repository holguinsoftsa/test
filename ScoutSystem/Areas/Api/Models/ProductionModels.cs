using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScoutSystem.Areas.Api.Models
{
    public class ProductionNameIn
    {
        public string Name { get; set; }
    }
    public class Production
    {
        public string Name { get; set; }
        public int ID { get; set; }

        public Production() { }
        public Production(Entities.Production Production)
        {
            this.Name = Production.Name;
            this.ID = Production.ProductionID;
        }
    }
}