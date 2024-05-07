using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicadorARP.Models.Auth;

namespace PublicadorARP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult CriarUsuario()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

      

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(string email, string senha)
        {
            // Verifique se o e-mail já está em uso
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                // E-mail já está em uso
                ModelState.AddModelError(string.Empty, "O e-mail já está em uso.");
                return BadRequest(ModelState);
            }

            // Crie uma nova instância de ApplicationUser
            var newUser = new User
            {
                UserName = email,
                Email = email
            };

            // Crie o usuário com o UserManager
            var result = await _userManager.CreateAsync(newUser, senha);

            // Verifique se a operação de criação foi bem-sucedida
            if (result.Succeeded)
            {
                // Usuário criado com sucesso
                return Ok("Usuário criado com sucesso.");
            }
            else
            {
                // Houve um erro ao criar o usuário
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }
        }
    }
}
