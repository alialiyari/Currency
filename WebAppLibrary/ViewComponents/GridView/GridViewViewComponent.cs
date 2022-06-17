using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GridViewViewComponent : ViewComponent
{
    public GridViewViewComponent()
    {

    }

    public async Task<IViewComponentResult> InvokeAsync(GridViewDTO forGV)
    {
        if (!string.IsNullOrEmpty(forGV.ColumnsSerialazed))
        {
            forGV.Columns = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GridViewColumn>>(forGV.ColumnsSerialazed);
            forGV.Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GridViewAction>>(forGV.ActionsSerialazed);
        }
        await Task.Run(() =>
        {

        });
        return View(forGV);
    }
}

