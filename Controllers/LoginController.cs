using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiPetShop.DataBases;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ApiPetShop.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(User value)
        {
            using (var _db = new PetShopContext())
            {
                try
                {
                    var user = await _db.User.SingleAsync(u => value.Password == u.Password && u.Email == value.Email);

                    var carShopping = await (from cs in _db.CarShopping
                                             join u in _db.User on cs.IdUser equals u.Id
                                             join p in _db.Product on cs.IdProduct equals p.Id
                                             select new
                                             {
                                                 Id = p.Id,
                                                 Name = p.Name,
                                                 Description = p.Description,
                                                 Stok = p.Stock,
                                                 Photo = p.Photo,
                                                 Price = p.Price,
                                                 IdUser = u.Id
                                             }).ToListAsync();
                    var purchale = await (from cs in _db.Purchale
                                          join u in _db.User on cs.IdUser equals u.Id
                                          join p in _db.Product on cs.IdProduct equals p.Id
                                          select new
                                          {
                                              Id = p.Id,
                                              Name = p.Name,
                                              Description = p.Description,
                                              Stok = p.Stock,
                                              Photo = p.Photo,
                                              Price = p.Price,
                                              IdUser = u.Id
                                          }).ToListAsync();
                    var _userInfo = AutenticarUsuario(user.Email, user.Password);

                    if (user.Password == value.Password && user.Email == value.Email && _userInfo!= null)
                    {

                        var userCar = carShopping.FindAll(p => p.IdUser == user.Id);
                        var userpur = purchale.FindAll(p => p.IdUser == user.Id);
                        return Ok(new { user = user, carShopping = userCar, purchale = userpur, token = GenerarTokenJWT(_userInfo)  });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }


                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }
        }

        private UserModel AutenticarUsuario(string usuario, string password)
        {
            // AQUÍ LA LÓGICA DE AUTENTICACIÓN //

            // Supondremos que el Usuario existe en la Base de Datos.
            // Retornamos un objeto del tipo UsuarioInfo, con toda
            // la información del usuario necesaria para el Token.
            return new UserModel()
            {
                // Id del Usuario en el Sistema de Información (BD)
                Id = new Guid("B5D233F0-6EC2-4950-8CD7-F44D16EC878F"),
                FirshName = "Nombre Usuario",
                LastName = "Apellidos Usuario",
                Email = "email.usuario@dominio.com"
            };

            // Supondremos que el Usuario NO existe en la Base de Datos.
            // Retornamos NULL.
            //return null;
        }

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(UserModel usuarioInfo)
        {
            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()),
                new Claim("firshName", usuarioInfo.FirshName),
                new Claim("lastName", usuarioInfo.LastName),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 24 horas.
                    expires: DateTime.UtcNow.AddHours(24)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }

    }
}
