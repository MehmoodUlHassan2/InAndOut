using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InAndOut.Controllers

{
	// [Route("Expense")]
	[Authorize]
	public class ExpenseController : Controller
	{
		private readonly ApplicationDbContext _db;

		public ExpenseController(ApplicationDbContext db)
		{
			_db = db;
		}


		public IActionResult Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			IEnumerable<Expense> expences = _db.Expenses.Where(x => x.UserId == userId).ToList();
			return View(expences);

		}

		//[Route("abc")]
		//[Route("")]
		//[Route("/")]
		//GET-Create
		public IActionResult Create()
		{
			return View();
		}

		//POST-Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Expense obj)
		{
		

			if (ModelState.IsValid)
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
				obj.UserId = userId;
				_db.Expenses.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//GET-Delete
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var obj = _db.Expenses.Find(id);

			if (obj == null)
			{
				return NotFound();
			}

			return View(obj);
		}

		//Post-Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{

			var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

			var obj = _db.Expenses.FirstOrDefault(x => x.Id == id && x.UserId == userid);

			if (obj == null)
			{
				return NotFound();
			}
			_db.Expenses.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		//GET-Update
		public IActionResult Update(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var obj = _db.Expenses.Find(id);

			if (obj == null)
			{
				return NotFound();
			}

			return View(obj);
		}


		//POST Update
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(Expense obj)
		{
			if (ModelState.IsValid)
			{
				var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
				obj.UserId = userId;
				_db.Expenses.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//[Route("edit/{id:alpha?}")]
		//public IActionResult edit(string id) 
		//{
		//    return Json(id);
		//}

	}
}
