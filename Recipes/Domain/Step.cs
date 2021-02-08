using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Domain
{
    public class Step
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}