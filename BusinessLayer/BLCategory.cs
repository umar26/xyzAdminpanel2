using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Transactions;
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
        public IEnumerable<SelectListItem> CategoryDDL()
        {
            return this.GetCategory().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
        }

        public IEnumerable<SelectListItem> SubCategoryDDLByCategory(int categoryId = 0)
        {
            return this.GetSubCategory().Where(r => r.ParentCategory == categoryId).Select(x => new SelectListItem
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
                Category = entity.ParentCategory,
                SubCategory = entity.Id
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

        public List<Content> GetContent()
        {
            return db.Contents.ToList();
        }

        public List<Image> GetImageBySubcategoryId(int subcategoryid)
        {
            return db.Images.Include("SubCategory").Where(r => r.ParentSubCategory == subcategoryid).ToList();
        }

        public Image GetImageById(int id)
        {
            return db.Images.Include("SubCategory").Where(r => r.Id == id).FirstOrDefault();
        }

        public List<Content> GetContentBySubcategoryId(int subcategoryid)
        {
            return db.Contents.Where(r => r.ParentSubCategory == subcategoryid).ToList();
        }

        public Content GetContentById(int id)
        {
            return db.Contents.Where(r => r.Id == id).FirstOrDefault();
        }

        public int UpdateImageContent(int imageId, int contentId, string content, string imageurl = "")
        {


            using (DbContextTransaction t = db.Database.BeginTransaction())
            {
                try
                {
                    Content DbContent = db.Contents.Where(r => r.Id == contentId).FirstOrDefault();
                    DbContent.TextContent = content;
                    DbContent.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(imageurl))
                    {
                        Image DbImage = db.Images.Where(r => r.Id == imageId).FirstOrDefault();
                        DbImage.ImagePath = imageurl;
                        DbImage.UpdateDate = DateTime.Now;
                    }

                    db.SaveChanges();
                    t.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    return 0;
                }

            }
        }

        public int DeleteImageContent(int id)
        {

            using (DbContextTransaction t = db.Database.BeginTransaction())
            {
                try
                {

                    Image dbEntityImage = db.Images.Where(r => r.Id == id).FirstOrDefault();
                    Content dbEntityContent = db.Contents.Where(r => r.Id == id).FirstOrDefault();
                    db.Images.Remove(dbEntityImage);
                    db.Contents.Remove(dbEntityContent);

                    db.SaveChanges();
                    t.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    return 0;
                }
            }

        }

        public int UpdateCategory(int Id, string Name)
        {
            Category entity = db.Categories.Where(r => r.Id == Id).FirstOrDefault();
            entity.Name = Name;
            return db.SaveChanges();
        }

        public int UpdateSubCategory(int Id, string Name)
        {
            SubCategory entity = db.SubCategories.Where(r => r.Id == Id).FirstOrDefault();
            entity.Name = Name;
            return db.SaveChanges();
        }

        public int DeleteCategory(int id)
        {


            using (DbContextTransaction t = db.Database.BeginTransaction())
            {
                try
                {
                    List<Category_SubCategory> Cat_Subcate = db.Category_SubCategory.ToList();
                    List<SubCategory> SubCategory = db.SubCategories.ToList();
                    List<int> subcatlist = Cat_Subcate.Where(r => r.Category == id).Select(k => k.SubCategory).ToList();

                    foreach (var item in subcatlist)
                    {
                        SubCategory subcat = SubCategory.Where(r => r.Id == item).FirstOrDefault();
                        if (subcat != null)
                            db.SubCategories.Remove(subcat);
                    }

                    Category category = db.Categories.Where(r => r.Id == id).FirstOrDefault();
                    db.Categories.Remove(category);

                    db.SaveChanges();
                    t.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    return 0;
                }
            }

        }

        public int DeleteSubCategory(int id)
        {
            using (DbContextTransaction t = db.Database.BeginTransaction())
            {
                try
                {
                    SubCategory SubCategory = db.SubCategories.Where(r => r.Id == id).FirstOrDefault();
                    db.SubCategories.Remove(SubCategory);
                    db.SaveChanges();
                    t.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    return 0;
                }
            }

        }
    }
}
