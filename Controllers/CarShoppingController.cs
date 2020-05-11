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
    public class CarShoppingController : ControllerBase
    {       
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarShopping value)
        {
            using (var _db = new PetShopContext())
            {
                _db.Add(new CarShopping
                {
                    IdUser = value.IdUser,
                    IdProduct = value.IdProduct

                });
                await _db.SaveChangesAsync();
                return Ok(value);
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
                    var CarShopping = await _db.CarShopping.FindAsync(id);
                    _db.Remove(CarShopping);
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
