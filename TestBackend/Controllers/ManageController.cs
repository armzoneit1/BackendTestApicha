using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestBackend.Models;

namespace TestBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ManageController : ControllerBase
{

    private readonly TestContext db = new TestContext();
    private readonly ILogger<ManageController> _logger;

    public ManageController(ILogger<ManageController> logger)
    {
        _logger = logger;
    }
    #region Get
    [HttpGet("getproduct",Name ="GetProduct")]
    public async Task<IActionResult> Get1()
    {
        try
        {
            List<Class.AllProducts> allproduct = new List<Class.AllProducts>();
            var itemall = await db.Products.ToListAsync();
            foreach (var tt in itemall)
            {
                var storeqty = await db.Stores.Where(x => x.ProductsId == tt.Id).FirstOrDefaultAsync();
                allproduct.Add(new Class.AllProducts
                {
                    id = tt.Id,
                    code = tt.Code,
                    name = tt.Name,
                    price = tt.Price,
                    qty = storeqty == null ? 0 : storeqty.Qty
                });

            }
            return Ok(allproduct);

        }
        catch
        {
            throw;
        }
    }
    [HttpGet("getcountitem",Name ="GetCountItem")]
    public async Task<IActionResult> Get3()
    {
        try
        {
            return Ok(await db.Certs.SumAsync(x => x.Qty));
        }
        catch
        {
            throw;
        }
    }
    [HttpGet("getcert", Name = "GetCert")]
    public async Task<IActionResult> Get2()
    {
        try
        {
            List<Class.AllProducts> allcerts = new List<Class.AllProducts>();
            var getall = await db.Certs.ToListAsync();
            foreach (var tt in getall)
            {
                var item = await db.Products.FindAsync(tt.ProductId);
                allcerts.Add(new Class.AllProducts
                {
                    id = tt.Id,
                    qty = tt.Qty,
                    price = item == null ? 0 : item.Price,
                    name = item == null ? "" : item.Name,
                    code = ""
                });
            }
            return Ok(allcerts);

        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region Post

    [HttpPost("makepayment",Name ="makePayment")]
    public async Task<IActionResult> Post1()
    {
        try
        {
            var allcert = await db.Certs.ToListAsync();
            foreach(var tt in allcert)
            {
                var friststore = await db.Stores.Where(x => x.ProductsId == tt.ProductId).FirstOrDefaultAsync();
                if(friststore != null)
                {
                    friststore.Qty -= tt.Qty;
                    
                }
                db.Certs.Remove(tt);
            }
            await db.SaveChangesAsync();
            return Ok("success");
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("add",Name ="Add")]
    public IActionResult Post(Class.AllProducts data)
    {
        try
        {
            var firstcert = db.Certs.Where(x => x.ProductId == data.id).FirstOrDefault();
            if (firstcert == null)
            {
                var pro = db.Certs.Add(new Cert
                {
                    ProductId = data.id,
                    Qty = 1
                });
            }
            else
            {
                firstcert.Qty += 1;
            }
            db.SaveChanges();

           
            return Ok("success");

        }
        catch 
        {
            throw;
        }
    }
    #endregion

    #region Detele
    [HttpDelete("delete",Name ="Delete")]
    public async Task<IActionResult> Delete1([FromQuery] int id)
    {
        try
        {
            var frist = await db.Certs.FindAsync(id);
            if(frist != null)
            {
                db.Certs.Remove(frist);
                await db.SaveChangesAsync();
            }
            return Ok("success");
        }
        catch
        {
            throw;
        }
    }
    [HttpDelete("clear",Name ="Clear")]
    public async Task<IActionResult> Delete2()
    {
        try
        {
            var allcert = await db.Certs.ToListAsync();
            if(allcert.Count > 0)
            {
                db.Certs.RemoveRange(allcert);
                await db.SaveChangesAsync();
            }
            return Ok("Success");
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Update
    [HttpPut("increase",Name = "Increase")]
    public async Task<IActionResult> Update1(Class.CertAction data)
    {
        try
        {
            var first = await db.Certs.FindAsync(data.id);
            if (first != null)
            {
                var store = await db.Stores.Where(x => x.ProductsId == first.ProductId).FirstOrDefaultAsync();
                if (store != null)
                {
                    if (first.Qty < store.Qty)
                    {
                        first.Qty += 1;

                    }
                }
            }

            await db.SaveChangesAsync();
            return Ok("success");
        }
        catch
        {
            throw;
        }
    }
    [HttpPut("decrease", Name = "Decrease")]
    public async Task<IActionResult> Update2(Class.CertAction data)
    {
        try
        {
            var first = await db.Certs.FindAsync(data.id);
            if (first != null)
            {
                var store = await db.Stores.Where(x => x.ProductsId == first.ProductId).FirstOrDefaultAsync();
                if (store != null)
                {
                    store.Qty += 1;
                }
                if (first.Qty == 1)
                {
                    db.Certs.Remove(first);
                }
                else
                {
                    first.Qty -= 1;

                }

            }

            await db.SaveChangesAsync();
            return Ok("success");
        }
        catch
        {
            throw;
        }
    }
    #endregion

}
