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

namespace AdminPanel.apiController
{
    public class Category_SubCategoryController : ApiController
    {
        private AdminPanelEntities db = new AdminPanelEntities();

        // GET: api/Category_SubCategory
        public IQueryable<Category_SubCategory> GetCategory_SubCategory()
        {
            return db.Category_SubCategory;
        }

        // GET: api/Category_SubCategory/5
        [ResponseType(typeof(Category_SubCategory))]
        public async Task<IHttpActionResult> GetCategory_SubCategory(int id)
        {
            Category_SubCategory category_SubCategory = await db.Category_SubCategory.FindAsync(id);

            if (category_SubCategory == null)
            {
                return NotFound();
            }

            return Ok(category_SubCategory);
        }


        // PUT: api/Category_SubCategory/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory_SubCategory(int id, Category_SubCategory category_SubCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category_SubCategory.Id)
            {
                return BadRequest();
            }

            db.Entry(category_SubCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Category_SubCategoryExists(id))
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

        // POST: api/Category_SubCategory
        [ResponseType(typeof(Category_SubCategory))]
        public async Task<IHttpActionResult> PostCategory_SubCategory(Category_SubCategory category_SubCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Category_SubCategory.Add(category_SubCategory);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category_SubCategory.Id }, category_SubCategory);
        }

        // DELETE: api/Category_SubCategory/5
        [ResponseType(typeof(Category_SubCategory))]
        public async Task<IHttpActionResult> DeleteCategory_SubCategory(int id)
        {
            Category_SubCategory category_SubCategory = await db.Category_SubCategory.FindAsync(id);
            if (category_SubCategory == null)
            {
                return NotFound();
            }

            db.Category_SubCategory.Remove(category_SubCategory);
            await db.SaveChangesAsync();

            return Ok(category_SubCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Category_SubCategoryExists(int id)
        {
            return db.Category_SubCategory.Count(e => e.Id == id) > 0;
        }
    }
}