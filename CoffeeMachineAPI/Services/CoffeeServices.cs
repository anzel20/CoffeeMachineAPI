using CoffeeMachineAPI.Model;
using Microsoft.AspNetCore.Http;

namespace CoffeeMachineAPI.Services
{
    public class CoffeeServices
    {
        private int _requestCount;
        private readonly Func<DateTimeOffset> _getNow;

        // The default constructor is used by the real API and gets the current system date/time.
        public CoffeeServices() : this(() => DateTimeOffset.Now) 
        { 
        
        }
        // This constructor allows tests to provide a fixed date/time, such as April 1st.
        public CoffeeServices(Func<DateTimeOffset> getNow)
        {
            _getNow = getNow;
        }

        public CoffeeResultModel BrewCoffee()
        {
            var now = _getNow();

            // April 1st takes priority over the normal brewing and out-of-coffee rules.
            if (now.Month == 4 && now.Day == 1)
            {
                return new CoffeeResultModel
                {
                    StatusCode = StatusCodes.Status418ImATeapot
                };
            }
            // Interlocked keeps the request counter safe when multiple requests arrive at once.
            var currentCount = Interlocked.Increment(ref _requestCount);

            // Every fifth request means the coffee machine is temporarily out of coffee.
            if (currentCount % 5 == 0) 
            {
                return new CoffeeResultModel
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                };
            }

            return new CoffeeResultModel 
            { 
                StatusCode = StatusCodes.Status200OK,
                Response = new CoffeeResponseModel
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = now.ToString("yyyy-MM-ddTHH:mm:ss") + now.ToString("zzz").Replace(":", "")
                }
            };
        }
    }
}
