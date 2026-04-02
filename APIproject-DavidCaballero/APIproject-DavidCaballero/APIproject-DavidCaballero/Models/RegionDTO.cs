using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace APIproject_DavidCaballero.Models
{
    [ModelMetadataType(typeof(RegionMetaData))]
    public class RegionDTO /*: Auditable*/
    {
        public int ID { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public Byte[]? RowVersion { get; set; }
        public ICollection<MemberDTO>? Members { get; set; }

    }
}
