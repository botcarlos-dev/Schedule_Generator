// Controllers/FuncionarioController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HorariosIPBejaMVC.Data;
using HorariosIPBejaMVC.Models;
using HorariosIPBejaMVC.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HorariosIPBejaMVC.Controllers
{
    [Authorize(Policy = "FuncionarioGestaoPolicy")]
    public class FuncionarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Funcionario/NovoAnoLetivo
        public IActionResult NovoAnoLetivo()
        {
            return View(new NovoAnoLetivoViewModel());
        }

        // POST: Funcionario/NovoAnoLetivo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoAnoLetivo(NovoAnoLetivoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Desativar o ano letivo ativo
                var anoLetivoAtivo = await _context.ANO_LETIVOs.FirstOrDefaultAsync(a => a.ativo);
                if (anoLetivoAtivo != null)
                {
                    anoLetivoAtivo.ativo = false;
                    _context.ANO_LETIVOs.Update(anoLetivoAtivo);
                }

                // Criar o novo ano letivo
                var novoAnoLetivo = new ANO_LETIVO
                {
                    descricao = model.Descricao,
                    data_inicio = model.DataInicio,
                    data_fim = model.DataFim,
                    ativo = true
                };

                await _context.ANO_LETIVOs.AddAsync(novoAnoLetivo);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Redirecionar para a lista de anos letivos ou outra ação
            }

            return View(model); // Retornar a view com o modelo se houver erros de validação
        }
    }
}
