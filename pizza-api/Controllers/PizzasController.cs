using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pizza_api.Data;
using pizza_api.Models;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzasController : ControllerBase  
    {
        private readonly pizza_apiContext _context;

        public PizzasController(pizza_apiContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "pizza")]
        public async Task<List<Pizza>> Index()
        {
            return await _context.Pizza.ToListAsync();
        }


        //[HttpGet(Name = "pizza/my")]
        
        //public string order()
        //{
        //    return "order pizza";
        //}




    }
}
