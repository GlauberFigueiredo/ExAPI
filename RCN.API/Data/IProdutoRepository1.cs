using System.Collections.Generic;

namespace RCN.API.Data
{
    public interface IProdutoRepository1
    {
        void Editar(Produto produto);
        void Excluir(Produto produto);
        void Inserir(Produto produto);
        IEnumerable<Produto> Obter();
        Produto Obter(int id);
    }
}