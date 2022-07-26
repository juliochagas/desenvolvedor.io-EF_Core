// See https://aka.ms/new-console-template for more information
using EFCore.Domain;
using EFCore.ValeuObjects;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsultarPedidoCarregamentoAdiantado();
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoraParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            db.Produtos.Add(produto);
            //db.Set<Produto>.Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            //db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total registro(s): {registros}");
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto teste",
                CodigoBarras = "123456789123",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoraParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = " Julio Chagas",
                CEP = "20020902",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Telefone = "21999145865"
            };

            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();
            Console.WriteLine($"Registro(s) {registros}");
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Cleintes where c.Id > 0 select c).ToList();

            var consultaPorMetodo = db.Cleintes.AsNoTracking()
            .Where(p => p.Id > 0)
            .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                db.Cleintes.Find(cliente.Id);
            }
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Cleintes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10,
                    }
                }
            };

            db.Pedidos.Add(pedido);
            db.SaveChanges();
        }
    
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(p => p.Produto)
            .ToList();

            Console.WriteLine(pedidos.Count());
        }
    
    }
}

