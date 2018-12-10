using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqarApp.Models
{
    public class AdsViewModel
    {
        public string Title { get; set; }
        public string Region { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public string PaymentType { get; set; }
        public string Finishing { get; set; }
        public int Area { get; set; }
        public DateTime Time { get; set; }
        public string Floar { get; set; }
        public string By { get; set; }
        public bool EnableDiscount { get; set; }
        public string Description { get; set; }
       public string img1Url1 { get; set; }
       public string img1Url2 { get; set; }
       public string img1Url3 { get; set; }
       public string img1Url4 { get; set; }
       public string videoUrl { get; set; }
        public string Phone1 { get; set; }
       public string Phone2 { get; set; }
       public string Phone3 { get; set; }
       public string Phone4 { get; set; }
        public string Email { get; set; }
        public int? Id { get; set; }
    }
}