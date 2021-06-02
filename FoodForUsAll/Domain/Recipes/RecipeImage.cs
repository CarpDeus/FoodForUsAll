using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RecipeImage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSampleImage { get; set; } = false;
        public Guid AuthorId { get; set; }
        public bool IsApproved { get; set; } = false;
        public byte[] Image { get; set; }
    }
}
