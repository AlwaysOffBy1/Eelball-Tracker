using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EELBALL_TRACKER.Models
{
    //pulled directly from https://docs.telerik.com/data-access/quick-start-scenarios/wpf/quickstart-wpf-viewmodelbase-and-relaycommand
    //Added <object> to action
    public class RelayCommand :ICommand
    {
        private Action<object> methodToExecute;
        private Action<string, string> methodToExecuteTwoParam;
        private Func<bool> canExecuteEvaluator;
        

        public RelayCommand(Action<object> methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }
        public RelayCommand(Action<string, string> methodToExecute)
        {
            this.methodToExecuteTwoParam = methodToExecute;
            this.canExecuteEvaluator = null;
        }
        public RelayCommand(Action<object> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        

        
        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = this.canExecuteEvaluator.Invoke();
                return result;
            }
        }
        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke(parameter);
        }
    }
}
