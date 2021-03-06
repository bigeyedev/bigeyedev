﻿using System;
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



    public class checkWebsite
    {

        public int id { get; set; }
        //public int shopid { get; set; }
        public int shop_id { get; set; }
        public string shop_name { get; set; }
        public string promotion { get; set; }
        public string logo_menu_url { get; set; }
        public string logo_top_url { get; set; }

        public string meta_des { get; set; }
        public string meta_keyword { get; set; }

        public string website { get; set; }
        public string tel { get; set; }
        public string face_id { get; set; }
        public string face_qr_url { get; set; }
        public string face_url { get; set; }
        public string line_id { get; set; }
        public string line_qr_url { get; set; }
        public string line_url { get; set; }
        public string line_at_id { get; set; }
        public string insta_id { get; set; }
        public string insta_qr_url { get; set; }
        public string insta_url { get; set; }
        public string email { get; set; }
        public string location_address { get; set; }
        public string location_url { get; set; }

    }

}