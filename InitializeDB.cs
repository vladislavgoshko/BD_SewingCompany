using JetBrains.Annotations;
using SewingCompany.DbModels;
using System.Net.Http.Headers;

namespace SewingCompany
{
    public class InitializeDB
    {
        private readonly RequestDelegate _next;
        private readonly SewingCompanyContext dbContext;

        public InitializeDB(RequestDelegate next, SewingCompanyContext context)
        {
            this._next = next;
            this.dbContext = context;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            Random random = new Random();
            List<string> names = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\male_names.txt").ToList();
            List<string> lastNames = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\male_surnames.txt").ToList();
            List<string> positions = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\positions.txt").ToList();
            List<string> sections = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ".ToCharArray().Select(x => "Отдел " + x.ToString()).ToList();
            List<string> providers = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\companies.txt").ToList();
            List<string> nameProducts = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\products.txt").ToList();
            List<string> nameMaterials = File.ReadAllLines("C:\\Users\\ferru\\OneDrive\\Рабочий стол\\5semester\\ПриложенияБД\\CourseProject\\fabrics.txt").ToList();
            string chars = "йцукенгшщзхъфывапролджэячсмитьбю";
            List<string> types = new List<string>();
            int N = 100;
            for (int i = 0; i < 100; ++i)
            {
                string type = "";
                for (int j = 0; j < random.Next(10, 15); ++j) {
                    type+=chars[random.Next(0, chars.Length-1)];
                }
                types.Add(type);
            }
            for (int i = 0; i < N; i++)
            {
                dbContext.Workers.Add(new Worker()
                {
                    Name = names[random.Next(0, names.Count - 1)] + " " + lastNames[random.Next(0, lastNames.Count - 1)],
                    Section = sections[random.Next(0, sections.Count - 1)],
                    Position = positions[random.Next(0, positions.Count - 1)]
                });

                dbContext.Products.Add(new Product()
                {
                    Name = nameProducts[random.Next(0, nameProducts.Count - 1)],
                    Price = (decimal)new Random().NextDouble() * 100 + 10
                });

                dbContext.Customers.Add(new Customer()
                {
                    Name = names[random.Next(0, names.Count - 1)] + " " + lastNames[random.Next(0, lastNames.Count - 1)]
                });
            }
            Console.WriteLine(dbContext.SaveChanges());
            for (int i = 0; i < N; i++)
            {
                dbContext.Providers.Add(new Provider()
                {
                    Name = providers[random.Next(0, providers.Count - 1)],
                    MaterialAmount = new Random().NextDouble() * 100 + 10,
                    Price = (decimal)new Random().NextDouble() * 10 + 10,
                    DeliveryDate = DateTime.Now.AddDays(-new Random().Next(100, 500))
                });
            }
            Console.WriteLine(dbContext.SaveChanges());
            
            for (int i = 0; i < N; i++)
            {
                dbContext.Materials.Add(new Material()
                {
                    Name = nameMaterials[random.Next(0, nameMaterials.Count - 1)],
                    Type = types[random.Next(0, types.Count - 1)],
                    QuantityInStock = new Random().NextDouble() * 100 + 10,
                    ProviderId = new Random().Next(1, dbContext.Providers.OrderByDescending(x => x.Id).First().Id)
                });
            }
            Console.WriteLine(dbContext.SaveChanges());
            for (int i = 0; i < N; i++)
            {
                var d1 = DateTime.Now.AddDays(-new Random().Next(100, 500));
                var d2 = d1.AddDays(new Random().Next(1, 10));
                var d3 = d2.AddDays(new Random().Next(1, 10));
                var d4 = d3.AddDays(new Random().Next(-3, 6));

                dbContext.Orders.Add(new Order()
                {
                    CustomerId = new Random().Next(1, dbContext.Customers.OrderByDescending(x => x.Id).First().Id),
                    ProductId = new Random().Next(1, dbContext.Products.OrderByDescending(x => x.Id).First().Id),
                    WorkerId = new Random().Next(1, dbContext.Workers.OrderByDescending(x => x.Id).First().Id),
                    Amount = new Random().Next(1, 10),
                    OrderDate = d1,
                    ExecutionStartDate = d2,
                    ImplementationDate = d3,
                    DeliveryOrderDate = d4
                });
            }
            Console.WriteLine(dbContext.SaveChanges());
            for (int i = 0; i < N; ++i)
            {
                dbContext.MaterialLists.Add(new MaterialList()
                {
                    MaterialId = new Random().Next(1, dbContext.Materials.OrderByDescending(x => x.Id).First().Id),
                    ProductId = new Random().Next(1, dbContext.Products.OrderByDescending(x => x.Id).First().Id),
                    MaterialAmount = new Random().NextDouble() * 10 + 1
                });
            }

            Console.WriteLine(dbContext.SaveChanges());
            await _next.Invoke(context);
        }
    }
}
