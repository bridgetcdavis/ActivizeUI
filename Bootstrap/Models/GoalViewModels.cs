using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bootstrap.Models
{
    [Table("UserGoals")]
    public class GoalViewModel
    {
        [Display(Name = "User name")]
        public string Id { get; set; }

        [Display(Name = "Steps Taken Goal")]
        public double StepGoal { get; set; }

        [Display(Name = "Calories Burned Goal")]
        public double CalGoal { get; set; }

        [Display(Name = "Active Minutes Goal")]
        public double MinGoal { get; set; }

        [Display(Name = "Active Miles Goal")]
        public double MileGoal { get; set; }
    }
}