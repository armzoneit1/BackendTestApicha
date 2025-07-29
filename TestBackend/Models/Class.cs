namespace TestBackend.Models
{
    public class Class
    {
        public class Cert
        {
            public int? id { get; set; }
            public int productId { get; set; }
            public int userId { get; set; }
            public int qty { get; set; }
        }
        public class CertAction
        {
            public int id { get; set; }
        }

        public class AllProducts
        {
            public int id { get; set; }
            public string? code { get; set; }
            public string name { get; set; }
            public int qty { get; set; }
            public decimal price { get; set; }
        }

        public class AllCert
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal price { get; set; }
            public int qty { get; set; }
        }
    }
}
