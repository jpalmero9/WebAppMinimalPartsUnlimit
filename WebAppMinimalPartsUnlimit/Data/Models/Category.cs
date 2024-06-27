﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppMinimalPartsUnlimit.Data.Models
{
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
