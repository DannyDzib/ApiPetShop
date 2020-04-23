using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiPetShop.DataBases;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiPetShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        // GET api/values
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            using (var _db = new PetShopContext())
            {
                if (id != null)
                {
                    var product = await _db.Product.FindAsync(id);
                    return Ok(new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Stock = product.Stock,
                        Description = product.Description,
                        Photo = product.Photo,
                        Price = product.Price
                    });
                }
                else
                {
                    var products = await _db.Product.ToListAsync();
                    return Ok(products);

                }
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product value)
        {
            using (var _db = new PetShopContext())
            {
                _db.Add(new Product
                {
                    Name = value.Name,
                    Stock = value.Stock,
                    Description = value.Description,
                    Photo = value.Photo,
                    Price = value.Price

                });
                await _db.SaveChangesAsync();
                return Ok(value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product value)
        {
            using (var _db = new PetShopContext())
            {

                try
                {
                    var Product = await _db.Product.FindAsync(id);
                    Product.Name = value.Name;
                    Product.Stock = value.Stock;
                    Product.Description = value.Description;
                    Product.Photo = value.Photo;
                    Product.Price = value.Price;
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }

                return Ok();

            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var _db = new PetShopContext())
            {
                try
                {
                    var product = await _db.Product.FindAsync(id);
                    _db.Remove(product);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }
        }
    }
}
