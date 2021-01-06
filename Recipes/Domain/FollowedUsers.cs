using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Domain
{
    public class FollowedUsers
    {
        public Guid ObserverId { get; set; }
        public User Observer { get; set; }
        public Guid TargetId { get; set; }
        public User Target { get; set; }
    }
}
