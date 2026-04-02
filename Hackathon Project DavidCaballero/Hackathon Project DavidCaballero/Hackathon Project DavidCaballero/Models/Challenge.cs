using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon_Project_DavidCaballero.Models
{
    public class Challenge
    {
        public int ID { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public Byte[]? RowVersion { get; set; }
        public string Summary => $"{Code} - {Name}";
        public ICollection<Member>? Members { get; set; }
    }
}
