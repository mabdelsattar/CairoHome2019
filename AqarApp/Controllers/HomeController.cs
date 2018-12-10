using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AqarApp.Models;
using AqarApp.Security;

namespace AqarApp.Controllers
{
    public class HomeController : Controller
    {
        AqarDBEntities db;
        public HomeController()
        {
            db = new AqarDBEntities();
        }

        public ActionResult Index(bool? newest,bool? expensive, string searchQuery, int? Region, int? Type, int? PaymentType, int? pricefrom, int? priceto, int? areafrom, int? areato, int? Finishing, int? Floar)
        {
            //prepair filteration data
            var regions = db.Regions.ToList();

            List<SelectListItem> listItemsRegions = new List<SelectListItem>();
            listItemsRegions = regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            listItemsRegions.Insert(0, new SelectListItem() { Text="الكل",Value ="-1"});

            ViewBag.regions = listItemsRegions;


            var types = db.Types.ToList();
            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes = types.Select(x => new SelectListItem
            {
                Text = x.Type1,
                Value = x.Id.ToString()
            }).ToList();
            listItemsTypes.Insert(0, new SelectListItem() { Text = "الكل", Value = "-1" });

            ViewBag.types = listItemsTypes;


            var paymenttypes = db.PaymentTypes.ToList();
            List<SelectListItem> listItemsPaymentTypes = new List<SelectListItem>();
            listItemsPaymentTypes = paymenttypes.Select(x => new SelectListItem
            {
                Text = x.PaymentType1,
                Value = x.Id.ToString()
            }).ToList();
            listItemsPaymentTypes.Insert(0, new SelectListItem() { Text = "الكل", Value = "-1" });

            ViewBag.paymenttypes = listItemsPaymentTypes;

            var floars = db.Floors.ToList();
            List<SelectListItem> listItemsFloars = new List<SelectListItem>();
            listItemsFloars = floars.Select(x => new SelectListItem
            {
                Text = x.FloorName,
                Value = x.Id.ToString()
            }).ToList();
            listItemsFloars.Insert(0, new SelectListItem() { Text = "الكل", Value = "-1" });

            ViewBag.floars = listItemsFloars;

            var finishings = db.Finishings.ToList();
            List<SelectListItem> finishingsTypes = new List<SelectListItem>();
            finishingsTypes = finishings.Select(x => new SelectListItem
            {
                Text = x.FininshName,
                Value = x.Id.ToString()
            }).ToList();
            finishingsTypes.Insert(0, new SelectListItem() { Text = "الكل", Value = "-1" });
            ViewBag.finishings = finishingsTypes;

            //

            var dbAds = db.Ads.OrderByDescending(c=>c.CreationDate).ToList();
            
            if (!string.IsNullOrEmpty(searchQuery))
                dbAds = dbAds.Where(d => d.Name.ToLower().Contains(searchQuery.ToLower())).ToList();
            ViewBag.searchQuery = searchQuery;

            if (Region != null && Region != -1)
                dbAds = dbAds.Where(d => d.RegionId == Region).ToList();
            ViewBag.Region = Region;

            if (Type != null && Type != -1)
                dbAds = dbAds.Where(d => d.TypeId == Type).ToList();
            ViewBag.Type = Type;


            if (PaymentType != null &&  PaymentType != -1)
                dbAds = dbAds.Where(d => d.TypePaymentId == Type).ToList();
            ViewBag.PaymentType = PaymentType;


            if (pricefrom != null)
                dbAds = dbAds.Where(d => d.Price >= pricefrom).ToList();
            ViewBag.pricefrom = pricefrom;


            if (priceto != null)
                dbAds = dbAds.Where(d => d.Price <= priceto).ToList();
            ViewBag.priceto = priceto;


            if (areafrom != null)
                dbAds = dbAds.Where(d => d.Area >= areafrom).ToList();
            ViewBag.areafrom = areafrom;


            if (areato != null)
                dbAds = dbAds.Where(d => d.Area >= areato).ToList();
            ViewBag.areato = areato;


            if (Finishing != null && Finishing != -1)
                dbAds = dbAds.Where(d => d.FinishId >= Finishing).ToList();
            ViewBag.Finishing = Finishing;


            if (Floar != null && Floar != -1)
                dbAds = dbAds.Where(d => d.FloarId >= Floar).ToList();
            ViewBag.Floar = Floar;


            if (newest != null)
            {
                if (newest == false)
                {
                    dbAds = dbAds.OrderBy(c => c.CreationDate).ToList();
                }
            }
            if (expensive != null)
            {
                if (expensive == false)
                {
                    dbAds = dbAds.OrderBy(c => c.Price).ToList();
                }
                else {
                    dbAds = dbAds.OrderByDescending(c => c.Price).ToList();

                }

            }
            List<AdsViewModel> adsList = new List<AdsViewModel>();
            foreach (Ad ad in dbAds)
            {
                //fill the model here please 
                adsList.Add(new AdsViewModel()
                {
                    Id = ad.Id,
                    By = ad.UserInfo.Username,
                    Time = ad.CreationDate.Value,
                    Area = ad.Area.Value,
                    Description = ad.Description,
                    Email = ad.EmailAddress,
                    EnableDiscount = ad.AvailableDecrese.Value,
                    Floar = ad.Floor.FloorName,
                    PaymentType = ad.PaymentType.PaymentType1,
                    Phone1 = ad.PhoneNumber1,
                    Phone2 = ad.PhoneNumber2,
                    Phone3 = ad.PhoneNumber3,
                    Phone4 = ad.PhoneNumber4,
                    Price = ad.Price.Value,
                    Region = ad.Region.Name,
                    Title = ad.Name,
                    Type = ad.Type.Type1,
                    img1Url1 = ad.Image1,
                    img1Url2 = ad.Image2,
                    img1Url3 = ad.Image3,
                    img1Url4 = ad.Image4,
                    Finishing = ad.Finishing.FininshName
                    
                });

            }
            return View(adsList);
        }


        public ActionResult Details(int adId)
        {
            var dbAd = db.Ads.Where(c => c.Id == adId).FirstOrDefault();
            if (dbAd != null)
            {
                //fill the model here please 
                AdsViewModel model = new AdsViewModel()
                {
                    Id = dbAd.Id,
                    By = dbAd.UserInfo.Username,
                    Time = dbAd.CreationDate.Value,
                    Area = dbAd.Area.Value,
                    Description = dbAd.Description,
                    Email = dbAd.EmailAddress,
                    EnableDiscount = dbAd.AvailableDecrese.Value,
                    Floar = dbAd.Floor.FloorName,
                    PaymentType = dbAd.PaymentType.PaymentType1,
                    Phone1 = dbAd.PhoneNumber1,
                    Phone2 = dbAd.PhoneNumber2,
                    Phone3 = dbAd.PhoneNumber3,
                    Phone4 = dbAd.PhoneNumber4,
                    Price = dbAd.Price.Value,
                    Region = dbAd.Region.Name,
                    Title = dbAd.Name,
                    Type = dbAd.Type.Type1,
                    img1Url1 = dbAd.Image1,
                    img1Url2 = dbAd.Image2,
                    img1Url3 = dbAd.Image3,
                    img1Url4 = dbAd.Image4,
                    Finishing = dbAd.Finishing.FininshName,
                    videoUrl = dbAd.videoUrl
                };




                return View(model);
            }

            return View(new AdsViewModel());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}