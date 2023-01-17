using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace redis.Controllers
{
    public class EmployeeController : Controller
    {
        IMemoryCache _memoryCache;
        public EmployeeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public ActionResult SetCache()
        {
            _memoryCache.Set("employeeName", "hakan");
            return View();
        }
        public ActionResult GetCache()
        {
            _memoryCache.Get<string>("employeeName");
            return View();
        }
        public void RemoveCache()
        {
            _memoryCache.Remove("employeeName");
        }
        public ActionResult GetCache2()
        {
            //TryGetValue Fonksiyonu Cache’de belirtilen key deðerine uygun veriyi sorgular.Veri yoksa ‘false’ eðer varsa ‘true’ döndürerek ‘out’ olan ikinci parametresinde de cacheden datayý döndürür.

            if (_memoryCache.TryGetValue<string>("employeeName", out string? data))
            {
                //data burada elde ediliyor
            }
            return View();
        }
        public IActionResult GetCache3()
        {
            string name = _memoryCache.GetOrCreate<string>("employeeName", entry =>
            {
                entry.SetValue("hakan");
                Console.WriteLine(DateTime.Now);
                return entry.Value.ToString();
            });

            return View();
        }
        public void GetCache4()
        {
            DateTime date = _memoryCache.GetOrCreate<DateTime>("date", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(30); //Cache'de ki datanýn ömrü 10 saniye olarak belirlenmiþtir.
                entry.SlidingExpiration = TimeSpan.FromSeconds(5); //Cache'de ki datanýn ömrü 2 saniye olarak belirlenmiþtir.

                DateTime value = DateTime.Now;
                Console.WriteLine($"*** Set Cache : {value}");
                return value;
            });

            Console.WriteLine($"Get Cache : {date}");
        }
        public IActionResult SetCache1()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
            options.SlidingExpiration = TimeSpan.FromSeconds(5);
            _memoryCache.Set("date", DateTime.Now, options);

            return RedirectToAction(nameof(GetCache));
        }
        public IActionResult GetCache5()
        {
            if (_memoryCache.TryGetValue<DateTime>("date", out DateTime date))
            {
                Console.WriteLine($"Get Cache : {date}");
            }
            return View();
        }
        public void SetCache2()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.Priority = CacheItemPriority.High;
            _memoryCache.Set("date", DateTime.Now, options);
        }
        public void GetCache6()
        {
            DateTime date = _memoryCache.GetOrCreate<DateTime>("date", entry =>
            {
                entry.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    Console.WriteLine($"Key : {key}\nValue : {value}\nReason : {reason}\nState : {state}");
                });
                DateTime value = DateTime.Now;
                Console.WriteLine($"*** Set Cache : {value}");
                return value;
            });

            Console.WriteLine($"Get Cache : {date}");
        }
    }
}


