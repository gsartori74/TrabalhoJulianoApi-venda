using System.Linq;
using System.Windows;
using umfg.venda.app.Abstracts;
using umfg.venda.app.UserControls;
using umfg.venda.app.ViewModels;

namespace umfg.venda.app.Commands
{
    internal sealed class ReceberPedidoCommand : AbstractCommand
    {
        public override bool CanExecute(object? parameter)
        {
            var vm = parameter as ListarProdutosViewModel;
            return vm is not null && vm.Pedido.Produtos.Any();
        }

        public override void Execute(object? parameter)
        {
            var vm = parameter as ListarProdutosViewModel;

            if (vm is null)
            {
                MessageBox.Show("Parâmetro obrigatório não informado! Verifique.");
                return;
            }

            ucReceberPedido.Exibir(vm.MainWindow, vm.Pedido);
        }
    }
}
