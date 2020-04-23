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
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        // GET api/values
        [HttpGet("{id?}")]
        public async Task<IActionResult> Get(int? id)
        {
            using (var _db = new PetShopContext())
            {
                if (id != null)
                {
                    var user = await _db.User.FindAsync(id);
                    return Ok(new User
                    {
                        Id = user.Id,
                        FirshName = user.FirshName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Address = user.Address,
                        Tel = user.Tel,
                        Photo = user.Photo
                    });
                }
                else
                {
                    var users = await _db.User.ToListAsync();
                    return Ok(users);

                }
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post( User value)
        {
            using (var _db = new PetShopContext())
            {
                _db.Add(new User
                {
                    Id = value.Id,
                    FirshName = value.FirshName,
                    LastName = value.LastName,
                    Email = value.Email,
                    Address = value.Address,
                    Tel = value.Tel,
                    Password = value.Password,
                    Photo = value.Photo

                });
                await _db.SaveChangesAsync();
                return Ok(value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User value)
        {
            using (var _db = new PetShopContext())
            {

                try
                {
                    var user = await _db.User.FindAsync(id);
                    user.Id = value.Id;
                    user.FirshName = value.FirshName;
                    user.LastName = value.LastName;
                    user.Email = value.Email;
                    user.Address = value.Address;
                    user.Tel = value.Tel;
                    user.Password = value.Password;
                    user.Photo = value.Photo;
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
                    var user = await _db.User.FindAsync(id);
                    _db.Remove(user);
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
