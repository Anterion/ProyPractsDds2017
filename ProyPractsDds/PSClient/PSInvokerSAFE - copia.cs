using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace ProyPractsDds.PS
{

    class PSInvoker
    {
        private Runspace runspace;

        private static PSInvoker instance;//SINGLETON
        private PSInvoker()
        {
            runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            state = new FreeRSState();
            strategy = new AppendCommandStrategy();
        }//SINGLETON
        public static PSInvoker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PSInvoker();
                }
                return instance;
            }
        }//SINGLETON
        internal ReturnStrategy strategy; //Patrón estrategia
        private InvokerState state; //Patrón estado

        private void SetBusyState()
        {
            state = new BusyRSState();
        } ///  Patrón estado + Lazy Initialization
        private void SetFreeState()
        {
            state = new FreeRSState();
        } ///  Patrón estado

        internal List<string[]> Invoke(PSCommand command, string arg)
        {
            return strategy.Strategize(command, command.Normalize(state.Invoke(command.GetCommand(arg))));
        }

        abstract class InvokerState
        {

            internal abstract Collection<PSObject> Invoke(string script);

        }//Patrón estado
        class FreeRSState : InvokerState
        {
            internal override Collection<PSObject> Invoke(string script)
            {
                PowerShell powershell = PowerShell.Create();
                powershell.Runspace = PSInvoker.Instance.runspace;
                powershell.AddScript(script);
                Instance.SetBusyState();
                Collection<PSObject> result = powershell.Invoke();
                Instance.SetFreeState();
                return result;
            }
        }//Patrón estado
        class BusyRSState : InvokerState
        {
            internal override Collection<PSObject> Invoke(string script)
            {

                using (Runspace myRunSpace = RunspaceFactory.CreateRunspace())
                {
                    Collection<PSObject> result;
                    myRunSpace.Open();
                    PowerShell powershell = PowerShell.Create();
                    powershell.Runspace = myRunSpace;
                    using (powershell)
                    {
                        powershell.AddScript(script);
                        result = powershell.Invoke();
                    }
                    powershell = null;
                    myRunSpace.Close();
                    return result;
                }
            }
        }//Patrón estado + Lazy Initialization
    }

    abstract class ReturnStrategy
        {
            abstract internal List<string[]> Strategize(PSCommand command, List<string[]> result);
    }//Patrón estrategia
    class JustExecuteStrategy : ReturnStrategy
    {
        public override string ToString()
        {
            return "Estandar";
        }
        override internal List<string[]> Strategize(PSCommand command, List<string[]> result)
        {
            return result;
            }
    }//Patrón estrategia
    class AppendCommandStrategy : ReturnStrategy
    {
        public override string ToString()
        {
            return "Comando";
        }
        override internal List<string[]> Strategize(PSCommand command, List<string[]> result)
        {
            result.Insert(0, new string[] { command.ToString()+":" });
            return result;
            }
    }//Patrón estrategia
    class SaveLogStrategy : ReturnStrategy
    {
            override internal List<string[]> Strategize(PSCommand command, List<string[]> result)
        {
            return result;

        }
    }//Patrón estrategia

}
