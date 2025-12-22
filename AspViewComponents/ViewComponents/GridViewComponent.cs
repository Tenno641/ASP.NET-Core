using AspViewComponents.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspViewComponents.ViewComponents;

public class GridViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PersonGrid grid)
    {
        return View(grid);
    }
}
