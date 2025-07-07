using EBest.Models;
using EBest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBest.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/Admin/[controller]/{action=Index}/{id?}")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly int PAGE_SIZE = 5;

        public ProductsController(ApplicationDbContext context , IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

     

        public IActionResult Index(int pageIndex,string? search,string ? column , string ? orderBy)
        {
            IQueryable<Product> query = context.Products;

            // search functionality 
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search));
            }

            // sort functionality 

            string[] validColumns = {"Id" ,"Name", "Brand", "Category", "Price", "CreatedAt" };
            if (!validColumns.Contains(column))
            {
                column = "Id";

            }
            string[] validOrders = { "asc", "desc" };

            if (!validOrders.Contains(orderBy))
            {
                orderBy = "desc";
            }




            if (orderBy == "asc")
            {
                switch (column)
                {
                    case "Name":
                        query = query.OrderBy(p => p.Name);
                        break;
                    case "Brand":
                        query = query.OrderBy(p => p.Brand);
                        break;
                    case "Category":
                        query = query.OrderBy(p => p.Category);
                        break;
                    case "Price":
                        query = query.OrderBy(p => p.Price);
                        break;
                    case "CreatedAt":
                        query = query.OrderBy(p => p.CreatedAt);
                        break;
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }
            }
            else
            {
                switch (column)
                {
                    case "Name":
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case "Brand":
                        query = query.OrderByDescending(p => p.Brand);
                        break;
                    case "Category":
                        query = query.OrderByDescending(p => p.Category);
                        break;
                    case "Price":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    case "CreatedAt":
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                    default:
                        query = query.OrderByDescending(p => p.Id);
                        break;
                }
            }
            
            // pagination
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            decimal totalCount = query.Count();
            int totalPages = (int)Math.Ceiling(totalCount/PAGE_SIZE );
            
            query = query.Skip((pageIndex - 1) * PAGE_SIZE).Take(PAGE_SIZE);
           
            var products = query.ToList();
            ViewData["pageIndex"] = pageIndex;
            ViewData["totalPages"] = totalPages;
            ViewData["search"] = search ?? "";
            ViewData["column"] = column;
            ViewData["orderBy"] = orderBy;

            return View(products);
        }
        public IActionResult Create()
        {
         
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDto productDto )
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "please upload image to the product!");
                return View(productDto);
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // saving file 
            string FileName = DateTime.Now.ToString("yyyMMddHHmmssfff");
            FileName += Path.GetExtension(productDto.ImageFile!.FileName);
            string FullPath = environment.WebRootPath + "/products/" + FileName;

            using(var stream  = System.IO.File.Create(FullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            // save product to the database 
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Description = productDto.Description,
                ImageFileName = FileName,
                Price = productDto.Price

            };

            context.Products.Add(product);
            context.SaveChanges();


            return RedirectToAction("Index","Products");
        }

        public IActionResult Edit(int id)
        {

            Product product = context.Products.FirstOrDefault(P => P.Id== id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            ProductDto productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,

            };

            ViewData["id"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("dd/MM/yyyy");


            return View(productDto);

        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {

            Product product = context.Products.FirstOrDefault(P => P.Id == id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {

                ViewData["id"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("dd/MM/yyyy");
                return View(productDto);

            }
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile!=null)
            {
                // saving file 
                newFileName = DateTime.Now.ToString("yyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile!.FileName);
                string FullPath = environment.WebRootPath + "/products/" + newFileName;

                using (var stream = System.IO.File.Create(FullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }
                // delete previos image
                System.IO.File.Delete(environment.WebRootPath + "/products/" + product.ImageFileName);
            }

            // update product

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.Category = productDto.Category;
            product.Brand = productDto.Brand;
            product.ImageFileName = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index","Products");

        }


        public IActionResult  Delete(int id)
        {
            Product product = context.Products.Find(id);
            if (product==null)
            {
                return RedirectToAction("Index", "Products");

            }
            var FileNamePath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(FileNamePath);
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
    }
}

