using System;
using System.Linq;
using System.Windows;
using umfg.venda.app.Abstracts;
using umfg.venda.app.ViewModels;

namespace umfg.venda.app.Commands
{
    internal sealed class AdicionarProdutoPedidoCommand : AbstractCommand
    {
        public override void Execute(object? parameter)
        {
            var vm = parameter as ListarProdutosViewModel;

            if (vm is null)
            {
                MessageBox.Show("Parâmetro obrigatório não foi informado");
                return;
            }

            if (vm.Pedido is null || Guid.Empty.Equals(vm.Pedido.Id))
            {
                MessageBox.Show("Pedido não foi criado corretamente! Verifique.");
                return;
            }

            if (vm.ProdutoSelecionado is null || Guid.Empty.Equals(vm.ProdutoSelecionado.Id))
            {
                MessageBox.Show("Nenhum produto selecionado! Verifique.");
                return;
            }

            var result = MessageBox
                .Show("Deseja realmente incluir este item no carrinho?",
                            "Confirmar produto", MessageBoxButton.YesNo);

            if (!MessageBoxResult.Yes.Equals(result))
                return;

            vm.Pedido.Produtos.Add(vm.ProdutoSelecionado);
            vm.Pedido.Total = vm.Pedido.Produtos.Sum(x => x.Valor);

            vm.RaiseCanExecuteChanged();
        }
    }
}
