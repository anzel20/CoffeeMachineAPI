using CoffeeMachineAPI.Services;
using Microsoft.AspNetCore.Http;

namespace CoffeeMachineAPI.Tests
{
    public class CoffeeServicesTests
    {
        [Fact]
        public void BrewCoffee_OnAprilFirst()
        {
            var service = new CoffeeServices(
                () => new DateTimeOffset(2026, 4, 1, 10, 30, 0, TimeSpan.FromHours(8))
            );

            var result = service.BrewCoffee();

            Assert.Equal(StatusCodes.Status418ImATeapot, result.StatusCode);
            Assert.Null(result.Response);
        }

        [Fact]
        public void BrewCoffee_EveryFifthCall_ReturnsServiceUnavailable()
        {
            var service = new CoffeeServices(
                () => new DateTimeOffset(2026, 5, 28, 10, 30, 0, TimeSpan.FromHours(8))
            );

            service.BrewCoffee();
            service.BrewCoffee();
            service.BrewCoffee();
            service.BrewCoffee();

            var result = service.BrewCoffee();

            Assert.Equal(StatusCodes.Status503ServiceUnavailable, result.StatusCode);
            Assert.Null(result.Response);
        }

        [Fact]
        public void BrewCoffee_SixthCall_ReturnsOkAgain()
        {
            var service = new CoffeeServices(
                () => new DateTimeOffset(2026, 5, 28, 10, 30, 0, TimeSpan.FromHours(8))
            );

            service.BrewCoffee();
            service.BrewCoffee();
            service.BrewCoffee();
            service.BrewCoffee();
            service.BrewCoffee();

            var result = service.BrewCoffee();

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Response);
        }


    }
}