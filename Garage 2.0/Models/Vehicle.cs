﻿using Garage_2._0.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class Vehicle : IVehicle
    {
        public string Type { get; set; }
        [Key]
        public string License { get; set; }

        public string Color { get; set; }
        [StringLength(20, ErrorMessage = "Length of Make can't be more than 20")]
        public string Make { get; set; }
        [StringLength(20, ErrorMessage = "Length of Model can't be more than 20")]
        public string Model { get; set; }
        [Range(0, int.MaxValue)]
        public int Wheels { get; set; }
        [ReadOnly(true)]
        public DateTime Arrival { get; set; }
        //The error message saids that we need to have an empty constructor so I removed the one we hade.

        // I tried this but then all of the times in all the items changed to the time that I added the last thing. I moved It to the Control in the Create method.

        //public Vehicle() 
        //{
        //    Arrival = DateTime.Now;
        //}
    }
}
