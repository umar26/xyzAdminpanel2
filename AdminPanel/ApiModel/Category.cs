using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.ApiModel
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class Content
    {

        public int ContentId { get; set; }
        public string ContentValue { get; set; }
    }

    public class Image
    {

        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
    }
}