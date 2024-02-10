using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkCore.Domain
{
    public class League : BaseDomainModel
    {
        public string Name { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
