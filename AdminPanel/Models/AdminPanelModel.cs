﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AdminPanel.Models
{
    public class AdminPanelModel
    {

    }

    public class CategoryModel
    {
        public string Category { get; set; }
    }

    public class SubCategoryModel
    {
        public IEnumerable<SelectListItem> Categories { get; set;}
        public int SelectedCategory { get; set; }
        public string SubCategory { get; set;}    
    }

    public class SubCategoryModelForList
    {
        public string SubCategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ParentCategory { get; set; }

    }
    public class ImageAndContentModel
    {
        public ImageAndContentModel()
        {
            Files = new List<HttpPostedFileBase>();
        }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int SelectedCategory { get; set; }

        public IEnumerable<SelectListItem> SubCategories { get; set; }
        public int SelectedSubCategory { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }
    }

    public class EditImageAndContentModel
    {
        public EditImageAndContentModel()
        {
            Files = new List<HttpPostedFileBase>();
        }

        
        public int CategoryName { get; set; }

        public IEnumerable<SelectListItem> SubCategories { get; set; }
        public int SubCategoryName { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }

    }
    public class ImageContentModelForList
    {
        public ImageContentModelForList()
        {
          Files=  new List<HttpPostedFileBase>();
        }
        public int Id { get; set; }
        public string ParentCategory { get; set; }
        public string SubCategory { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        public int ContentId { get; set;}
        public int ImageId { get; set;}
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set;}
        public List<HttpPostedFileBase> Files { get; set; }
    }
}