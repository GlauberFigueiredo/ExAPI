using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RCN.API.Data;

namespace RCN.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository Repositorio;

        public ProdutosController(IProdutoRepository repositorio)
        {
            Repositorio = repositorio;
        }



        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Criar([FromBody]Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Codigo))
                return BadRequest("Codigo do produto nao informado!");

            if (string.IsNullOrEmpty(produto.Descricao))
                return BadRequest("Descricao do produto nao informada!");
  
            Repositorio.Inserir(produto);
            return Created(nameof(Criar), produto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json","application/xml")]
        public IActionResult Obter()
        {
            var lista = Repositorio.Obter();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult Obter(int id)
        {
            var produto = Repositorio.Obter(id);

            if (produto != null)
                return Ok(produto);
            else
                return NotFound();
        }
        [HttpPut]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult Atualizar([FromBody]Produto produto)
        {
            var prod = Repositorio.Obter(produto.Id);
            if (prod == null)
                return NotFound();
            if (string.IsNullOrEmpty(produto.Codigo))
            {
                return BadRequest("Codigo do produto nao informado!");
            }
            if (string.IsNullOrEmpty(produto.Descricao))
            {
                return BadRequest("Descricao do produto nao informada!");
            }

            Repositorio.Editar(produto);
            return NoContent();


        }
        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Apagar(int id)
        {
            var prod = Repositorio.Obter(id);
            if (prod != null)
            {
                Repositorio.Excluir(prod);
                return Ok();

            }
            else
                return NotFound();
        }




        [HttpGet("{id}")]
        [ApiVersion("2.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult ObterPorCodigo(string codigo)
        {
            //var produto = null;

            //if (produto != null)
            //    return Ok(produto);
            //else
                return NotFound();
        }


        [HttpGet("")]
        [ApiVersion("3.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterTodos(string codigo)
        {
            List<string> lista = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                lista.Add($"indice: {i}");
            }
            return Ok(string.Join(",",lista));
        }
    }
}
