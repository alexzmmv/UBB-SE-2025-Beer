using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
	public class SuccessPageController : Controller
	{
		public IActionResult SuccessPage()
		{
			return View();
		}
	}
}