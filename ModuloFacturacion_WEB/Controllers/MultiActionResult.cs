using Microsoft.AspNetCore.Mvc;

namespace ModuloFacturacion_WEB.Controllers
{
    public class MultiActionResult: IActionResult
    {
        private readonly IActionResult[] _results;

        public MultiActionResult(params IActionResult[] results)
        {
            _results = results;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            foreach (var result in _results)
            {
                await result.ExecuteResultAsync(context);
            }
        }
    }
}
