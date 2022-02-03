using Queue.Data;
using Queue.Models;

namespace Queue.Controllers;

public partial class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _dbcontext;
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signinManager;

    public HomeController(ILogger<HomeController> logger,
                            AppDbContext context,
                            UserManager<UserModel> userManager,
                            SignInManager<UserModel> signinManager)
    {
        _logger = logger;
        _dbcontext = context;
        _userManager = userManager;
        _signinManager = signinManager;
    }

    public async Task<IActionResult> Index()
    {
        var queues = await _dbcontext.Queues.Where(q => q.IsActive == true).OrderBy(p => p.CreatedAt).ToListAsync();
        if(queues !=null){
            return View(queues);
        }
        return RedirectToAction("Error");
    }

    [Authorize(Roles = "superadmin")]
    [HttpGet]
    public async Task<IActionResult> AdminPage()
    {
        var result = await _dbcontext.Queues.Where(q => q.IsActive == true).OrderBy(p=>p.CreatedAt).ToListAsync();
    
        return View(result);
    }

    public IActionResult EnqueUser(Guid id)
    {
        if (!_dbcontext.Queues.Any(t => t.ID == id))
        {
            return NotFound();
        }

        var obj = _dbcontext.Queues.FirstOrDefault(o => o.ID == id);
        obj.IsActive=false;
        _dbcontext.Queues.Update(obj);
        _dbcontext.SaveChanges();
        TempData["success"] = " Mijozga hizmat ko'rsatildi.!!";
        return RedirectToAction("AdminPage");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
