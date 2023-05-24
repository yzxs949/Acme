using Acme.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Acme.Dapper;

namespace Acme.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<UserInfo> _userInfoRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IRepository<UserInfo> userInfoRepository
            )
        {
            _logger = logger;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<IActionResult> Index()
        {
           int A= VerCode.GetEquationCode(out string equationString);
            var data = await _userInfoRepository.GetPageListBySqlAsync("SELECT * FROM Role", 1, 20);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}