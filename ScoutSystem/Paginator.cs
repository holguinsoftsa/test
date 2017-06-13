using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScoutSystem
{
    public class Paginator
    {
        public int Count { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }

        public Paginator() { }


        public List<T> GetResult<T>(List<T> Records)
        {
            this.Count = (int)Math.Ceiling(Records.Count() / (float)this.Size);
            return Records.Skip(this.Number * this.Size).Take(this.Size).ToList();
        }
    }
}