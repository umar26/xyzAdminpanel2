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
            return RedirectToAction("Category");
        }

        public ActionResult Category_List()
        {
            //BLCategory BL = new BLCategory();
            List<Category> List = BL.GetCategory();
            return View(List);
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
            return RedirectToAction("SubCategory");
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
                if (file != null &&  file.ContentLength > 0)
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
            return RedirectToAction("ImageUpload");
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

        public ActionResult EditImageContent( int Id)
        {

            Image imagedata = BL.GetImageById(Id);
            Content contentdata = BL.GetContentById(Id);

            ImageContentModelForList model = new ImageContentModelForList();
            model.Content = contentdata.TextContent;
            model.ContentId = contentdata.Id;
            model.ImageUrl = "/Images/" + imagedata.ImagePath;
            model.ImageId = imagedata.Id;
            model.SubCategory = imagedata.SubCategory.Name;
            model.Id = Id;

            return View(model);
            
        }
        [HttpPost]
        public ActionResult EditImageContent(ImageContentModelForList Model)
        {
            HttpPostedFileBase file = Model.Files[0];
            string fileName = "";
            try
            {
                if (file!=null && file.ContentLength > 0)
                {

                    string extension = Path.GetExtension(file.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(Server.MapPath("~/Images"), fileName);
                    file.SaveAs(filePath);
                }
                BL.UpdateImageContent(Model.ContentId, Model.ImageId, Model.Content, fileName);
                TempData["SuccessMsg"] = "ImageContent is Updated Successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "ImageContent is not  Updated Successfully";
            }
            return RedirectToAction("EditImageContent", new { SubCategoryId = Model.Id });
        }


        public ActionResult DeleteImageContent(int Id)
        {

            BL.DeleteImageContent(Id);
            
            return RedirectToAction("ImageContent_List");
        }

        [HttpPost]
        public ActionResult Edit_Category(string Id,string Text)
        {
            BL.UpdateCategory(Convert.ToInt32(Id), Text);
            
            
            return Json("true");
        }

        [HttpDelete]
        public ActionResult Delete_Category(string Id)
        {
            BL.DeleteCategory(Convert.ToInt32(Id));
            return Json("true");
        }
    }
}