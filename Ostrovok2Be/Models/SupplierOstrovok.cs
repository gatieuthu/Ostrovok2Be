using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Ostrovok2Be.Models
{
    class SupplierOstrovok
    {

        public string SupplierOstId { get; set; }

        public string SupplierOstIds { get; set; }

        public string SupplierBeId { get; set; }

        public string SupplierOstNameEn { get; set; }

        public string SupplierOstNameRu { get; set; }

        public string Policy_descriptionRu { get; set; }

        public string Policy_descriptionEn { get; set; }

        public string DescriptionEn { get; set; }

        public string DescriptionRu { get; set; }

        public string AmenitiesEn { get; set; }

        public string AmenitiesRu { get; set; }

        public string CountryEn { get; set; }

        public string CountryRu { get; set; }

        public string CityRu { get; set; }

        public string CityEn { get; set; }

        public float PriceUsd { get; set; }

        public float PriceRub { get; set; }

        public float PriceVnd { get; set; }

        public float PriceEur { get; set; }

        public string RegionId { get; set; }

        public string Address { get; set; }

        public string Adress_clean { get; set; }

        public string Thumbnail { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Country_code { get; set; }

        public string Kind { get; set; }

    }
}
