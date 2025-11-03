using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using System.Collections.Generic;
 
namespace Projeto.Controllers
{
    public class ChatController : Controller
{
    [HttpGet]
    public IActionResult Chat()
    {
        return View();
    }
        public IActionResult Perguntar(string pergunta)
        {
            var resposta = ChatbotService.ObterResposta(pergunta);
            return Json(new { resposta });
        }
    }
}
