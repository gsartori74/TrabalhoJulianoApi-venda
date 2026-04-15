using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using umfg.venda.app.Interfaces;

namespace umfg.venda.app.Abstracts
{
    internal abstract class AbstractViewModel : AbstractNotifyPropertyChange, ISubject
    {
        private readonly ICollection<IObserver> _observers = [];

        private string _titulo = string.Empty;

        public string Titulo
        {
            get => _titulo;
            set => SetField(ref _titulo, value);
        }

        private UserControl _userControl;
        public UserControl UserControl
        {
            get => _userControl;
            protected set => SetField(ref _userControl, value);
        }
        public IObserver MainWindow { get; protected set; }

        protected AbstractViewModel(string titulo)
        {
            Titulo = titulo;
        }

        public void Add(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var item in _observers)
            {
                item.Update(this);
            }
        }
    }
}
