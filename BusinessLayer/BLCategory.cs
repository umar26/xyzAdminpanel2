using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace BusinessLayer
{
    public class BLCategory
    {
        private AdminPanelEntities db = new AdminPanelEntities();

        public List<Category> GetCategory()
        {
            return db.Categories.ToList();
        }
        public List<SubCategory> GetSubCategory()
        {
            return db.SubCategories.ToList();
        }
        public IEnumerable<SelectListItem>CategoryDDL()
        {
          return  this.GetCategory().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text=x.Name
            });
        }

        public IEnumerable<SelectListItem> SubCategoryDDLByCategory(int categoryId=0)
        {
            return this.GetSubCategory().Where(r=>r.ParentCategory==categoryId).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        }

        public List<SubCategory> GetSubCategoryByCategory(int categoryid)
        {
            return db.SubCategories.Where(r => r.ParentCategory == categoryid).ToList();
        }

        public void AddCategory(Category entity)
        {
            db.Categories.Add(entity);
            db.SaveChanges();
        }

        public void AddSubCategory(SubCategory entity)
        {
            db.SubCategories.Add(entity);
            db.SaveChanges();
            AddCategory_Subcategory(new Category_SubCategory()
            {
                Category=entity.ParentCategory,
                SubCategory= entity.Id
            });
            
        }

        public void AddCategory_Subcategory(Category_SubCategory entity)
        {
            db.Category_SubCategory.Add(entity);
            db.SaveChanges();
        }

        public void AddImages(Image entity)
        {
            db.Images.Add(entity);
            db.SaveChanges();
        }
        public void AddContent(Content entity)
        {
            db.Contents.Add(entity);
            db.SaveChanges();
        }

        public List<Image> GetImage()
        {
            return db.Images.Include("SubCategory").ToList();
        }

        public List<Content>GetContent()
        {
            return db.Contents.ToList();
        }

        public List<Image>GetImageBySubcategoryId(int subcategoryid)
        {
            return db.Images.Where(r => r.ParentSubCategory == subcategoryid).ToList();
        }

        public List<Content> GetContentBySubcategoryId(int subcategoryid)
        {
            return db.Contents.Where(r => r.ParentSubCategory == subcategoryid).ToList();
        }
    }
}
