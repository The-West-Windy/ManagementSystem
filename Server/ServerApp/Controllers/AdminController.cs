using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class AdminController : Controller
{
    public IActionResult Index() => View();
}
