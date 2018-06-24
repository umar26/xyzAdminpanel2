using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DatabaseAccessLayer;
using Newtonsoft.Json;
namespace AdminPanel.apiController
{
    public class SubCategoriesController : ApiController
    {
        private AdminPanelEntities db = new AdminPanelEntities();

        // GET: api/SubCategories
        public IQueryable<SubCategory> GetSubCategories()
        {
            return db.SubCategories;
        }

        // GET: api/SubCategories/5
        [ResponseType(typeof(SubCategory))]
        public async Task<IHttpActionResult> GetSubCategory(int id)
        {
            SubCategory subCategory = await db.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return Ok(subCategory);
        }

        [HttpGet]
        [Route("api/SubCategoriesByCategory/{categoryId}")]
       // [ResponseType(typeof(List<ApiModel.SubCategory>))]
        public async Task<IHttpActionResult> SubCategoryByCategoryId(int categoryId)
        {
            List<SubCategory> Subcategories = await db.SubCategories.Where(r => r.ParentCategory == categoryId).ToListAsync();
            List<ApiModel.SubCategory> apiSubCategories = new List<ApiModel.SubCategory>();
            Subcategories.ForEach((item) => apiSubCategories.Add(new ApiModel.SubCategory()
            {
                SubCategoryId = item.Id,
                SubCategoryName = item.Name
            }));

            string str = JsonConvert.SerializeObject(apiSubCategories);
            string str1 = "{'data':" + str + "}";

            return Ok(JsonConvert.DeserializeObject(str1));
        }

        [HttpGet]
        [Route("api/ImageAndContentBySubCatId/{subcategoryId}")]
        public async Task<IHttpActionResult>ImageAndContentByCategoryId(int subcategoryId)
        {
            List<Image> Images = await db.Images.Where(r => r.ParentSubCategory == subcategoryId).ToListAsync();
            List<Content> Contents = await db.Contents.Where(r => r.ParentSubCategory == subcategoryId).ToListAsync();

            List<ApiModel.Image> apiImages = new List<ApiModel.Image>();
            List<ApiModel.Content> apiContent = new List<ApiModel.Content>();
            Images.ForEach((item) => apiImages.Add(new ApiModel.Image()
            {
                ImageId=item.Id,
                ImageUrl= "/Images/"+item.ImagePath
            }));

            Contents.ForEach((item) => apiContent.Add(new ApiModel.Content()
            {
                ContentId = item.Id,
                ContentValue = item.TextContent
            }));
            string Imagestr = JsonConvert.SerializeObject(apiImages);
            string Contentstr = JsonConvert.SerializeObject(apiContent);

            string finalstr = "{'data': {'imageArray':" + Imagestr + ","+"'contentArray':" + Contentstr + "}}";
            return Ok(JsonConvert.DeserializeObject(finalstr));
        }
        // PUT: api/SubCategories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubCategory(int id, SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCategory.Id)
            {
                return BadRequest();
            }

            db.Entry(subCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SubCategories
        [ResponseType(typeof(SubCategory))]
        public async Task<IHttpActionResult> PostSubCategory(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SubCategories.Add(subCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = subCategory.Id }, subCategory);
        }

        // DELETE: api/SubCategories/5
        [ResponseType(typeof(SubCategory))]
        public async Task<IHttpActionResult> DeleteSubCategory(int id)
        {
            SubCategory subCategory = await db.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            db.SubCategories.Remove(subCategory);
            await db.SaveChangesAsync();

            return Ok(subCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCategoryExists(int id)
        {
            return db.SubCategories.Count(e => e.Id == id) > 0;
        }
    }
}