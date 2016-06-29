using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bigeyedev.Models
{
    public class stockBindingModel
    {
        public int id { get; set; }
        public string model { get; set; }
        public string brand { get; set; }
        public string near { get; set; }
        public string imageUrl { get; set; }
        //there's color 12 item
        public int Black { get; set; }

        public int Choco { get; set; }

        public int Gray { get; set; }

        public int Brown { get; set; }

        public int Blue { get; set; }

        public int Green { get; set; }

        public int Violet { get; set; }

        public int Pink { get; set; }

        public int Silver { get; set; }

        public int Gold { get; set; }

        public int Sky { get; set; }

        public int Red { get; set; }
    }
}