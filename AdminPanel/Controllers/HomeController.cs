using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AdminPanel.Models;
using BusinessLayer;
using DatabaseAccessLayer;
namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        BLCategory BL = null;
        public HomeController()
        {
            BL = new BLCategory();
        }
        public ActionResult Index()
        {
            return View();
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

        public ActionResult Category()
        {
            CategoryModel Model = new CategoryModel();
            return View(Model);
        }
        [HttpPost]
        public ActionResult Category(CategoryModel Model)
        {
          //  BLCategory BL = new BLCategory();
            BL.AddCategory(new DatabaseAccessLayer.Category()
            {
                Name = Model.Category,
            });
            TempData["SuccessMsg"] = "Category is Added Successfully";
            return View(Model);
        }

        public ActionResult Category_List()
        {
            //BLCategory BL = new BLCategory();
            return View(BL.GetCategory());
        }
        public ActionResult SubCategory()
        {
            SubCategoryModel Model = new SubCategoryModel();
            //BLCategory BL = new BLCategory();

            Model.Categories = BL.CategoryDDL();

            return View(Model);
        }
        [HttpPost]
        public ActionResult SubCategory(SubCategoryModel Model)
        {  
            //BLCategory BL = new BLCategory();
            BL.AddSubCategory(new DatabaseAccessLayer.SubCategory()
            {
                Name = Model.SubCategory,
                ParentCategory = Model.SelectedCategory,
                UpdateDate = DateTime.UtcNow
            });
            Model.Categories = BL.CategoryDDL();
            TempData["SuccessMsg"] = "Sub Category is Added Successfully";
            return View(Model);
        }

        public ActionResult SubCategory_List()
        {
            // BLCategory BL = new BLCategory();
            return View(BL.GetSubCategory());
        }
        public JsonResult GetSubCategoryByCategory(string categoryId)
        {
            BLCategory BL = new BLCategory();
          IEnumerable<SelectListItem>SubCategory=  BL.SubCategoryDDLByCategory(!string.IsNullOrEmpty(categoryId)?Convert.ToInt32(categoryId):0);

            return Json(SubCategory);
        }
        public ActionResult ImageUpload()
        {
            ImageAndContentModel Model = new ImageAndContentModel();
            Model.Categories = BL.CategoryDDL();
            Model.SubCategories = BL.SubCategoryDDLByCategory();

            return View(Model);
        }
        [HttpPost]
        public ActionResult ImageUpload(ImageAndContentModel Model)
        {

            HttpPostedFileBase file = Model.Files[0];
            Model.Categories = BL.CategoryDDL();
            string fileName = "";
            try
            {
                if (file.ContentLength > 0)
                {
                  
                    string extension=Path.GetExtension(file.FileName);
                     fileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file.SaveAs(filePath);
                }
            }
            catch(Exception ex)
            {
                
            }
            BL.AddImages(new Image()
            {
                ImagePath = fileName,
                ParentSubCategory = Model.SelectedSubCategory,
                UpdateDate = DateTime.Now
            });
            BL.AddContent(new Content()
            {
                TextContent=Model.Content,
                ParentSubCategory = Model.SelectedSubCategory,
                UpdateDate = DateTime.Now
            });
            Model.SubCategories = BL.SubCategoryDDLByCategory();
            TempData["SuccessMsg"] = "Image is Added Successfully";
            return View(Model);
        }

        public ActionResult ImageContent_List()
        {
            List<ImageContentModelForList> Model = new List<ImageContentModelForList>();
            var Images = BL.GetImage();
           
            Images.ForEach((r) => Model.Add(new ImageContentModelForList()
            {
                Id=r.Id,
                ImageUrl= "/Images/"+ r.ImagePath ,
                SubCategory=r.SubCategory.Name,
              //  ParentCategory=r.SubCategory.Category.Name,
               CreatedDate=r.UpdateDate
            }));
            return View(Model);
        }
    }
}