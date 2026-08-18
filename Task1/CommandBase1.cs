using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Task1
{
    public abstract class BaseCommand : ICommand
    {
        public abstract Boolean CanExecute(object Parameter);
        public abstract void Execute(object Parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    }   // базовая команда . Окончание

    public class CommandCompress : BaseCommand
    {

        private Task1VM LOVM;

        public CommandCompress() : this(null) { }
        public CommandCompress(Task1VM LParVM)
        {
            LOVM = LParVM;
        }

        public override Boolean CanExecute(object Parameter)
        {
            string LCData = Parameter as string;
            Boolean LLResuLt = false;
            if ((this.LOVM != null) && (LCData != null))
            {
                LLResuLt = LOVM.CanCompress(LCData);

            }
            else
            {
                LLResuLt = false;
            }

            return LLResuLt;
        }   // можно ли выполнить

        public override void Execute(object Parameter)
        {
            string LCData = Parameter as string;

            if ((LOVM != null) && (LCData != null))
            {
                LOVM.Compress(LCData);
            }
        }   // выполнить

    }   // CommandCompress класс . Окончание

    // команда развертки
    public class CommandDecompress : BaseCommand
    {
        private Task1VM LOVM;

        public CommandDecompress() : this(null) { }
        public CommandDecompress(Task1VM LParVM)
        {
            LOVM = LParVM;
        }

        // можно ли выполнить
        public override Boolean CanExecute(object Parameter)
        {
            Boolean LLResuLt = false;
            string LCData = Parameter as string;

            if ((this.LOVM != null) && (LCData != null))
            {
                LLResuLt = LOVM.CanDecompress(LCData);
            }
            else
            {
                LLResuLt = false;
            }
            return LLResuLt;
        }   // можно ли выполнить развертку . Окончание
        
        // выполнить развертку
        public override void Execute(object Parameter)
        {
            string LCData = Parameter as string;
            if ((this.LOVM != null) && (LCData != null))
            {
                this.LOVM.Decompress(LCData);
            }
        }   // выполнить развертку . Окончание


    }   // команда развертки . Окончание

}   // пространство имен Task1 . Окончание
