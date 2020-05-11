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
    public class PurchalesController : ControllerBase
    {       
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post( Purchale value)
        {
            using (var _db = new PetShopContext())
            {
                var date = DateTime.Now;
                _db.Add(new Purchale
                {
                    Date = date,
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
                    var purchale = await _db.Purchale.FindAsync(id);
                    _db.Remove(purchale);
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
