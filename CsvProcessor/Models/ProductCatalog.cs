using System.Collections.Generic;

namespace CsvProcessor.Models
{
    public class ProductCatalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public  int ProductTemplateId { get; set; }
        public  string Price { get; set; }
        public string Tags { get; set; }
        public string Details { get; set; }


    }
    
    public class ProductTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductAggregate
    {
       public  List<ProductCatalog> ProductCatalogs { get; set; }
       public  List<ProductTemplate> ProductTemplates { get; set; }
    }
}