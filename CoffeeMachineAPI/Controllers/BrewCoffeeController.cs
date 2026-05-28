using Microsoft.AspNetCore.Mvc;
using CoffeeMachineAPI.Services;

namespace CoffeeMachineAPI.Controllers
{
    [ApiController]
    [Route("brew-coffee")]
    public class BrewCoffeeController : Controller
    {
        public readonly CoffeeServices _coffeeServices;

        public BrewCoffeeController(CoffeeServices coffeeServices) { 
            
            _coffeeServices = coffeeServices;
        
        }

        [HttpGet]
        public IActionResult BrewCoffee() 
        { 
            var result = _coffeeServices.BrewCoffee();

            // 418 and 503 responses must have an empty response body.
            if (result.Response is null)
            {
                return StatusCode(result.StatusCode);
            }

            return StatusCode(result.StatusCode, result.Response);
        }
    }
}
