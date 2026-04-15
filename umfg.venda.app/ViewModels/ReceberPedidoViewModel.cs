using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using umfg.venda.app.Abstracts;
using umfg.venda.app.Interfaces;
using umfg.venda.app.Models;

namespace umfg.venda.app.ViewModels
{
    internal sealed class ReceberPedidoViewModel : AbstractViewModel
    {
        private readonly PedidoModel _pedido;

        public PedidoModel Pedido => _pedido;


        private string _nome = string.Empty;
        public string Nome
        {
            get => _nome;
            set => SetField(ref _nome, value);
        }

        private string _tipoCartao = string.Empty;
        public string TipoCartao
        {
            get => _tipoCartao;
            set => SetField(ref _tipoCartao, value);
        }

        private string _numeroCartao = string.Empty;
        public string NumeroCartao
        {
            get => _numeroCartao;
            set => SetField(ref _numeroCartao, value);
        }

        private string _mes = string.Empty;
        public string Mes
        {
            get => _mes;
            set => SetField(ref _mes, value);
        }

        private string _ano = string.Empty;
        public string Ano
        {
            get => _ano;
            set => SetField(ref _ano, value);
        }

        private string _cvv = string.Empty;
        public string CVV
        {
            get => _cvv;
            set => SetField(ref _cvv, value);
        }

        public ReceberPagamentoCommand Receber { get; private set; }

        public ReceberPedidoViewModel(UserControl userControl, IObserver observer, PedidoModel pedido)
            : base("Receber Pedido")
        {
            UserControl = userControl;
            MainWindow = observer;
            _pedido = pedido;

            Add(observer);
            Receber = new ReceberPagamentoCommand(this);
        }


        internal bool Validar()
        {
            var erros = string.Empty;

            if (string.IsNullOrWhiteSpace(Nome))
                erros += "• Nome do titular é obrigatório.\n";

            if (string.IsNullOrWhiteSpace(TipoCartao))
                erros += "• Selecione o tipo do cartão (Crédito ou Débito).\n";

            var numeroLimpo = NumeroCartao?.Replace(" ", "").Replace("-", "") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(numeroLimpo))
                erros += "• Número do cartão é obrigatório.\n";
            else if (!numeroLimpo.All(char.IsDigit))
                erros += "• Número do cartão deve conter apenas dígitos.\n";
            else if (numeroLimpo.Length < 13 || numeroLimpo.Length > 19)
                erros += "• Número do cartão deve ter entre 13 e 19 dígitos.\n";
            else if (!ValidarLuhn(numeroLimpo))
                erros += "• Número do cartão inválido (dígito verificador incorreto).\n";

            if (string.IsNullOrWhiteSpace(Mes) || string.IsNullOrWhiteSpace(Ano))
            {
                erros += "• Data de validade é obrigatória.\n";
            }
            else
            {
                int mes = int.Parse(Mes);
                int ano = int.Parse(Ano);
                var dataValidade = new DateTime(ano, mes, 1);
                var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                if (dataValidade <= dataAtual)
                    erros += "• Data de validade deve ser superior ao mês atual.\n";
            }

            if (string.IsNullOrWhiteSpace(CVV))
                erros += "• CVV é obrigatório.\n";
            else if (!CVV.All(char.IsDigit))
                erros += "• CVV deve conter apenas números.\n";
            else if (CVV.Length != 3)
                erros += "• CVV deve ter exatamente 3 dígitos.\n";

            if (!string.IsNullOrEmpty(erros))
            {
                MessageBox.Show(erros, "Campos inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        internal void FinalizarPagamento()
        {
            MessageBox.Show(
                $"Pagamento via {TipoCartao} realizado com sucesso!\nObrigado pela compra.",
                "Sucesso",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            UserControl = null;
            Notify();
        }


        private static bool ValidarLuhn(string numero)
        {
            int soma = 0;
            bool dobrar = false;

            for (int i = numero.Length - 1; i >= 0; i--)
            {
                int digito = numero[i] - '0';

                if (dobrar)
                {
                    digito *= 2;
                    if (digito > 9)
                        digito -= 9;
                }

                soma += digito;
                dobrar = !dobrar;
            }

            return soma % 10 == 0;
        }
    }

    internal sealed class ReceberPagamentoCommand : AbstractCommand
    {
        private readonly ReceberPedidoViewModel _vm;

        public ReceberPagamentoCommand(ReceberPedidoViewModel vm)
        {
            _vm = vm;
        }

        public override void Execute(object? parameter)
        {
            if (!_vm.Validar())
                return;

            _vm.FinalizarPagamento();
        }
    }
}
