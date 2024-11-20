// Handlers/GestaoRequirementHandler.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using HorariosIPBejaMVC.Data;
using HorariosIPBejaMVC.Models;
using HorariosIPBejaMVC.Requirements;
using Microsoft.Extensions.Logging;

public class GestaoRequirementHandler : AuthorizationHandler<GestaoRequirement>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GestaoRequirementHandler> _logger;

    public GestaoRequirementHandler(ApplicationDbContext context, ILogger<GestaoRequirementHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GestaoRequirement requirement)
    {
        _logger.LogInformation("GestaoRequirementHandler chamado para verificação de permissão.");

        // Verificar se o utilizador está na role "Funcionário"
        if (!context.User.IsInRole("Funcionário"))
        {
            _logger.LogWarning("Usuário não está na role 'Funcionário'.");
            return;
        }

        var userIdString = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString))
        {
            _logger.LogWarning("User ID não encontrado nos claims.");
            return;
        }

        if (!int.TryParse(userIdString, out int userId))
        {
            _logger.LogWarning($"Não foi possível converter o userId '{userIdString}' para int.");
            return;
        }

        var funcionario = await _context.FUNCIONARIOs
                                        .Include(f => f.idNavigation)
                                        .FirstOrDefaultAsync(f => f.idNavigation.id == userId);

        if (funcionario != null && funcionario.is_gabinete_gestao)
        {
            _logger.LogInformation($"Acesso permitido para o funcionário ID {userId} com is_gabinete_gestao = True.");
            context.Succeed(requirement); // Permite o acesso
        }
        else
        {
            _logger.LogWarning($"Acesso negado para o funcionário ID {userId}. is_gabinete_gestao = {funcionario?.is_gabinete_gestao}");
        }
    }
}