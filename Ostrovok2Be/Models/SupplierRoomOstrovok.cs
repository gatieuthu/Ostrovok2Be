using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ostrovok2Be.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("SupplierRoom")]
    class SupplierRoomOstrovok
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string SupplierRoomId { get; set; }

        [StringLength(50)]
        public string SupplierId { get; set; }

        [StringLength(1000)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LangCode { get; set; }

        [Required]
        [StringLength(50)]
        public string LangGroup { get; set; }

        [StringLength(1000)]
        public string PolicyName { get; set; }

        [Column(TypeName = "ntext")]
        public string Policy { get; set; }

        public int? NoofRoom { get; set; }

        public int? Smoking { get; set; }

        public int? NoofAdult { get; set; }

        public int? NoofChildren { get; set; }

        public int? NoofBedroom { get; set; }

        public int? NoofLivingroom { get; set; }

        public int? NoofBathroom { get; set; }

        public int? Breakfast { get; set; }

        [StringLength(2000)]
        public string Preview { get; set; }

        public string Description { get; set; }

        public string Thumbnails { get; set; }

        public string SeoImages { get; set; }

        [StringLength(1000)]
        public string Images { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        [StringLength(50)]
        public string UpdateBy { get; set; }
    }
}
