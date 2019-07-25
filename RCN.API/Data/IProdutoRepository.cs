using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCN.API.Data
{
    public interface IProdutoRepository
    {

        void Inserir(Produto produto);
        void Editar(Produto produto);
        void Excluir(Produto produto);
        Produto Obter(int id);
        IEnumerable<Produto> Obter();




    }
}
