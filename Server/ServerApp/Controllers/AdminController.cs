using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    public IActionResult Index() => View();
}
