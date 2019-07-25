using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RCN.API.Data
{
    public class ProdutoContexto: DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public ProdutoContexto(DbContextOptions options)
        : base(options)
        {


        }

    }
}
