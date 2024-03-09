using System;

namespace CirnoFramework.Interface.Commands
{
    public class CommandAttribute : Attribute
    {
        private string _command;

        public CommandAttribute(string name)
        {
            this._command = name;
        }

        public string Command
        {
            get
            {
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
    }
}
