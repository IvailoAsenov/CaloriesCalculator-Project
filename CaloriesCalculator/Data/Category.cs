﻿using System.ComponentModel.DataAnnotations;

namespace CaloriesCalculator.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
    }
}
