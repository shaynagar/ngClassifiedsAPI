using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ngClassifiedsAPI.Models
{
    public class ItemForUpdate
    {
        public ItemForUpdate()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Price must contain only numbers")]
        public double Price { get; set; }
        public string Image { get; set; }
    }
}