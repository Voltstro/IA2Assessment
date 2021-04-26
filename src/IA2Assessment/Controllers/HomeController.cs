using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IA2Assessment.Controllers
{
	/// <summary>
	///		<see cref="Controller"/> for home
	/// </summary>
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
