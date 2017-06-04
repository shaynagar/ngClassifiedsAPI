using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ngClassifiedsAPI;
using ngClassifiedsAPI.Models;
using System.Web.Mvc;

namespace ngClassifiedsAPI.Controllers
{
    public class ItemsController : ApiController
    {
        private ngClassifiedsDBEntities db = new ngClassifiedsDBEntities();

        // GET: api/Items
        public JsonResult GetItems()
        {
            var Items = db.Items.Select(i => new
            {
                id = i.Id,
                title = i.Title,
                description = i.Description,
                price = i.Price,
                posted = i.Posted,
                contact = new
                {
                    name = i.User.FirstName + " " + i.User.LastName,
                    phone = i.User.Phone,
                    email = i.User.Email
                },
                categories = i.Categories.Select(c => c.Name).ToList(),
                image = i.Image,
                views = i.Views
            }).OrderBy(i => i.id);

            JsonResult Data = new JsonResult { Data = Items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return Data;
        }

        // GET: api/Items/5
        public IHttpActionResult GetItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        public JsonResult PutItem(int id, ItemForUpdate item)
        {
            Item itemToEdit = db.Items.Find(id);
            string message = "";
            if (itemToEdit == null)
            {
                message = "Item does not exist";
            }
            itemToEdit.Posted = DateTime.Now;
            if (ModelState.IsValid)
            {
                itemToEdit.Title = item.Title;
                itemToEdit.Description = item.Description;
                itemToEdit.Price = item.Price;
                itemToEdit.Image = item.Image;
                db.SaveChanges();
                message = "Success";
            }
            else
            {
                foreach (var value in ModelState.Values.ToList())
                {
                    foreach (var msv in value.Errors.ToList())
                    {
                        message += msv.ErrorMessage;
                        if (msv.ErrorMessage != "")
                        {
                            message += "/n";
                        }
                    }
                }
                if (message == "")
                {
                    message = "Wrong details inserted";
                }
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // POST: api/Items
        public JsonResult PostItem(Item item)
        {
            string message = "";
            item.UserId = 1;
            item.Posted = DateTime.Now;
            item.Views = 0;

            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                message = "Success";
            }
            else
            {
                foreach(var value in ModelState.Values.ToList())
                {
                    foreach(var msv in value.Errors.ToList())
                    {
                        message += msv.ErrorMessage;
                        if(msv.ErrorMessage != "")
                        {
                            message += "/n";
                        }
                    }
                }
                if (message == "")
                {
                    message = "Wrong details inserted";
                }
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // DELETE: api/Items/5
        public JsonResult DeleteItem(int id)
        {
            Item item = db.Items.Find(id);
            string message = "";
            if (item == null)
            {
                message = "Item not found";
            }
            else
            {
                db.Items.Remove(item);
                db.SaveChanges();
                message = "Success";
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}