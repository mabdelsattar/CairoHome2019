using AqarApp.Models;
using AqarApp.Security;
using AqarApp.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AqarApp.Controllers
{
    public class AdminController : Controller
    {
        AqarDBEntities db;

        public AdminController()
        {
            db = new AqarDBEntities();
        }

        [CustomAuthorizeAttripute(Roles = "1")]
        public ActionResult Home()
        {
            return View();
        }

        [CustomAuthorizeAttripute(Roles = "1")]
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
                    Finishing = dbAd.Finishing.FininshName
                };




                return View(model);
            }

            return View(new AdsViewModel());
        }


        [CustomAuthorizeAttripute(Roles = "1")]
        public ActionResult DeleteAd(int adId)
        {
            var dbAd = db.Ads.Where(c => c.Id == adId).FirstOrDefault();
            if (dbAd != null)
            {

                if (!string.IsNullOrEmpty(dbAd.Image1))
                {
                    string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image1);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!string.IsNullOrEmpty(dbAd.Image2))
                {
                    string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image2);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                if (!string.IsNullOrEmpty(dbAd.Image3))
                {
                    string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image3);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                if (!string.IsNullOrEmpty(dbAd.Image4))
                {
                    string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image4);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                //fill the model here please 
                db.Ads.Remove(dbAd);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Admin");
        }






        public ActionResult Login()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel model)
        {
            AccountModel accModel = new AccountModel();

            if (string.IsNullOrEmpty(model.Account.Username)
                ||
   string.IsNullOrEmpty(model.Account.Password)
                ||
                accModel.login(model.Account.Username, model.Account.Password) == null
                 )
            {
                ViewBag.Error = "خطأ باسم المستخدم او كلمة المرور";
                return View("Login");
            }
            SessionPersister.Username = model.Account.Username;
            return RedirectToAction("Home");
        }

        public ActionResult Logout()
        {
            SessionPersister.Username = string.Empty;
            return RedirectToAction("Home");
        }

        [CustomAuthorizeAttripute(Roles = "1")]
        [HttpGet]
        public ActionResult CreateAdv()
        {
            var regions = db.Regions.ToList();
            List<SelectListItem> listItemsRegions = new List<SelectListItem>();
            listItemsRegions = regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.regions = listItemsRegions;


            var types = db.Types.ToList();
            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes = types.Select(x => new SelectListItem
            {
                Text = x.Type1,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.types = listItemsTypes;


            var paymenttypes = db.PaymentTypes.ToList();
            List<SelectListItem> listItemsPaymentTypes = new List<SelectListItem>();
            listItemsPaymentTypes = paymenttypes.Select(x => new SelectListItem
            {
                Text = x.PaymentType1,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.paymenttypes = listItemsPaymentTypes;

            var floars = db.Floors.ToList();
            List<SelectListItem> listItemsFloars = new List<SelectListItem>();
            listItemsFloars = floars.Select(x => new SelectListItem
            {
                Text = x.FloorName,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.floars = listItemsFloars;



            var finishings = db.Finishings.ToList();
            List<SelectListItem> finishingsTypes = new List<SelectListItem>();
            finishingsTypes = finishings.Select(x => new SelectListItem
            {
                Text = x.FininshName,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.finishings = finishingsTypes;

            return View(new AdsModel());
        }

        [CustomAuthorizeAttripute(Roles = "1")]
        [HttpPost]
        public ActionResult CreateAdv(AdsModel model, HttpPostedFileBase inputFileImage1, HttpPostedFileBase inputFileImage2, HttpPostedFileBase inputFileImage3, HttpPostedFileBase inputFileImage4)
        {
            string currentusername = SessionPersister.Username;
            var currentUser = db.UserInfoes.Where(u => u.Username.ToLower() == currentusername.ToLower()).FirstOrDefault();
            Ad ad = new Ad()
            {
                RegionId = model.Region,
                PhoneNumber1 = model.Phone1,
                PhoneNumber2 = model.Phone2,
                PhoneNumber3 = model.Phone3,
                PhoneNumber4 = model.Phone4,
                AvailableDecrese = model.EnableDiscount,
                CreatedById = currentUser.Id,
                FloarId = model.Floar,
                CreationDate = DateTime.UtcNow,
                Description = model.Description,
                EmailAddress = model.Email,
                Name = model.Title,
                Area = model.Area,
                TypeId = model.Type,
                TypePaymentId = model.PaymentType,
                Price = model.Price,
                FinishId = model.Finishing
            };

            if (inputFileImage1 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage1.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                inputFileImage1.SaveAs(pathToSave);
                ad.Image1 = fileName;
            }
            if (inputFileImage2 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage2.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                inputFileImage2.SaveAs(pathToSave);
                ad.Image2 = fileName;
            }

            if (inputFileImage3 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage3.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                inputFileImage3.SaveAs(pathToSave);
                ad.Image3 = fileName;
            }

            if (inputFileImage4 != null)
            {
                var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage4.FileName);
                var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                var pathToSave = Path.Combine(directoryToSave, fileName);
                inputFileImage4.SaveAs(pathToSave);
                ad.Image4 = fileName;
            }





            db.Ads.Add(ad);
            db.SaveChanges();



            return RedirectToAction("Index");
        }


        //edit Ad
        [CustomAuthorizeAttripute(Roles = "1")]
        [HttpGet]
        public ActionResult EditAdv(int adid)
        {
            AdsModel model = new AdsModel();
            model.Id = adid;

            var regions = db.Regions.ToList();
            List<SelectListItem> listItemsRegions = new List<SelectListItem>();
            listItemsRegions = regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.regions = listItemsRegions;


            var types = db.Types.ToList();
            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes = types.Select(x => new SelectListItem
            {
                Text = x.Type1,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.types = listItemsTypes;


            var paymenttypes = db.PaymentTypes.ToList();
            List<SelectListItem> listItemsPaymentTypes = new List<SelectListItem>();
            listItemsPaymentTypes = paymenttypes.Select(x => new SelectListItem
            {
                Text = x.PaymentType1,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.paymenttypes = listItemsPaymentTypes;

            var floars = db.Floors.ToList();
            List<SelectListItem> listItemsFloars = new List<SelectListItem>();
            listItemsFloars = floars.Select(x => new SelectListItem
            {
                Text = x.FloorName,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.floars = listItemsFloars;



            var finishings = db.Finishings.ToList();
            List<SelectListItem> finishingsTypes = new List<SelectListItem>();
            finishingsTypes = finishings.Select(x => new SelectListItem
            {
                Text = x.FininshName,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.finishings = finishingsTypes;


            var dbAd = db.Ads.Where(c => c.Id == model.Id).FirstOrDefault();
            if (dbAd != null)
            {
                model.Area = dbAd.Area.Value;
                model.Description = dbAd.Description;
                model.Email = dbAd.EmailAddress;
                model.EnableDiscount = dbAd.AvailableDecrese.Value;
                model.Finishing = dbAd.FinishId.Value;
                model.Floar = dbAd.FloarId.Value;
                model.Id = adid;
                model.PaymentType = dbAd.TypePaymentId.Value;
                model.Phone1 = dbAd.PhoneNumber1;
                model.Phone2 = dbAd.PhoneNumber2;
                model.Phone3 = dbAd.PhoneNumber3;
                model.Phone4 = dbAd.PhoneNumber4;
                model.Price = dbAd.Price.Value;
                model.Region = dbAd.RegionId.Value;
                model.Title = dbAd.Name;
                model.Type = dbAd.TypeId.Value;
                model.img1 = dbAd.Image1;
                model.img2 = dbAd.Image2;
                model.img3 = dbAd.Image3;
                model.img4 = dbAd.Image4;
            }

            return View(model);
        }

        [CustomAuthorizeAttripute(Roles = "1")]
        [HttpPost]
        public ActionResult EditAdv(AdsModel model, HttpPostedFileBase inputFileImage1, HttpPostedFileBase inputFileImage2, HttpPostedFileBase inputFileImage3, HttpPostedFileBase inputFileImage4,string img1,string img2,string img3,string img4)
        {

            var regions = db.Regions.ToList();
            List<SelectListItem> listItemsRegions = new List<SelectListItem>();
            listItemsRegions = regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.regions = listItemsRegions;


            var types = db.Types.ToList();
            List<SelectListItem> listItemsTypes = new List<SelectListItem>();
            listItemsTypes = types.Select(x => new SelectListItem
            {
                Text = x.Type1,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.types = listItemsTypes;


            var paymenttypes = db.PaymentTypes.ToList();
            List<SelectListItem> listItemsPaymentTypes = new List<SelectListItem>();
            listItemsPaymentTypes = paymenttypes.Select(x => new SelectListItem
            {
                Text = x.PaymentType1,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.paymenttypes = listItemsPaymentTypes;

            var floars = db.Floors.ToList();
            List<SelectListItem> listItemsFloars = new List<SelectListItem>();
            listItemsFloars = floars.Select(x => new SelectListItem
            {
                Text = x.FloorName,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.floars = listItemsFloars;



            var finishings = db.Finishings.ToList();
            List<SelectListItem> finishingsTypes = new List<SelectListItem>();
            finishingsTypes = finishings.Select(x => new SelectListItem
            {
                Text = x.FininshName,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.finishings = finishingsTypes;


            var dbAd = db.Ads.Where(c => c.Id == model.Id).FirstOrDefault();
            if (dbAd != null)
            {

                dbAd.RegionId = model.Region;
                dbAd.PhoneNumber1 = model.Phone1;
                dbAd.PhoneNumber2 = model.Phone2;
                dbAd.PhoneNumber3 = model.Phone3;
                dbAd.PhoneNumber4 = model.Phone4;
                dbAd.AvailableDecrese = model.EnableDiscount;
                dbAd.CreationDate = DateTime.UtcNow;
                dbAd.FloarId = model.Floar;
                dbAd.Description = model.Description;
                dbAd.EmailAddress = model.Email;
                dbAd.Name = model.Title;
                dbAd.Area = model.Area;
                dbAd.TypeId = model.Type;
                dbAd.TypePaymentId = model.PaymentType;
                dbAd.Price = model.Price;
                dbAd.FinishId = model.Finishing;

                #region image1 
                //if the user doesn't change any thing 
                if (!string.IsNullOrEmpty(model.img1))
                {}
                else
                {
                    //if the user removed it
                    if (!string.IsNullOrEmpty(dbAd.Image1))
                    {
                        string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image1);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (inputFileImage1 != null)
                    {

                        var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage1.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        inputFileImage1.SaveAs(pathToSave);
                        dbAd.Image1 = fileName;
                    }
                    else {
                        dbAd.Image1 = null;
                    }

                }


                #endregion

                #region image2

                if (!string.IsNullOrEmpty(model.img2))
                { }
                else
                {
                    //if the user removed it
                    if (!string.IsNullOrEmpty(dbAd.Image2))
                    {
                        string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image2);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (inputFileImage2 != null)
                    {

                        var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage2.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        inputFileImage2.SaveAs(pathToSave);
                        dbAd.Image2 = fileName;
                    }
                    else
                    {
                        dbAd.Image2 = null;
                    }

                }
                #endregion

                #region image3

                if (!string.IsNullOrEmpty(model.img3))
                { }
                else
                {
                    //if the user removed it
                    if (!string.IsNullOrEmpty(dbAd.Image3))
                    {
                        string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image3);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (inputFileImage3 != null)
                    {

                        var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage3.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        inputFileImage3.SaveAs(pathToSave);
                        dbAd.Image3 = fileName;
                    }
                    else
                    {
                        dbAd.Image3 = null;
                    }

                }
                #endregion

                #region image4

                if (!string.IsNullOrEmpty(model.img4))
                { }
                else
                {
                    //if the user removed it
                    if (!string.IsNullOrEmpty(dbAd.Image4))
                    {
                        string fullPath = Request.MapPath("~/Pictures/" + dbAd.Image4);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    if (inputFileImage4 != null)
                    {

                        var fileName = Guid.NewGuid() + Path.GetFileName(inputFileImage4.FileName);
                        var directoryToSave = Server.MapPath(Url.Content("~/Pictures"));

                        var pathToSave = Path.Combine(directoryToSave, fileName);
                        inputFileImage4.SaveAs(pathToSave);
                        dbAd.Image4 = fileName;
                    }
                    else
                    {
                        dbAd.Image4 = null;
                    }

                }
                #endregion
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(new { adId = model.Id});

            //}
        }



        [CustomAuthorizeAttripute(Roles = "1")]
        public ActionResult Index(bool? newest, bool? expensive, string searchQuery, int? Region, int? Type, int? PaymentType, int? pricefrom, int? priceto, int? areafrom, int? areato, int? Finishing, int? Floar)
        {
            //prepair filteration data
            var regions = db.Regions.ToList();

            List<SelectListItem> listItemsRegions = new List<SelectListItem>();
            listItemsRegions = regions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            listItemsRegions.Insert(0, new SelectListItem() { Text = "الكل", Value = "-1" });

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

            var dbAds = db.Ads.OrderByDescending(c => c.CreationDate).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
                dbAds = dbAds.Where(d => d.Name.ToLower().Contains(searchQuery.ToLower())).ToList();
            ViewBag.searchQuery = searchQuery;

            if (Region != null && Region != -1)
                dbAds = dbAds.Where(d => d.RegionId == Region).ToList();
            ViewBag.Region = Region;

            if (Type != null && Type != -1)
                dbAds = dbAds.Where(d => d.TypeId == Type).ToList();
            ViewBag.Type = Type;


            if (PaymentType != null && PaymentType != -1)
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
                else
                {
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


    }


}