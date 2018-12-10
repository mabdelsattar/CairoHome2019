using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqarApp.Models
{
    public class AdsModel
    {
        public string Title { get; set; }
        public int Region { get; set; }
        public int Price { get; set; }
        public int Type { get; set; }
        public int PaymentType { get; set; }
        public int Area { get; set; }
        public int Floar { get; set; }
        public int Finishing { get; set; }
        public bool EnableDiscount { get; set; }
        public string Description { get; set; }
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string img4 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Phone4 { get; set; }
        public string Email { get; set; }
        public int? Id { get; set; }
    }
}