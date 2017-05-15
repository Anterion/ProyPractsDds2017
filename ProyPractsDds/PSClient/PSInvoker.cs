using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace proyPractsDds.pS
{
    partial class PSClient //business delegate
    {

        partial class PSInvoker
        {
            private Runspace _runspace;
            public List<PSTuple> AvailableCommands { get; private set; }
            private static PSInvoker _instance;//SINGLETON
            private PSInvoker()
            {
                AvailableCommands = new List<PSTuple>
              (
                  new PSTuple[]
                  {
                    new PSTuple( new GetADComputers()),
                    new PSTuple( new GetComputerSerialNumber()),
                    new PSTuple( new GetOperatingSystem()),
                    new PSTuple( new GetOSSerialNumber()),
                    new PSTuple( new GetUserName()),
                    new PSTuple( new ResetUserPassword()),
                    new PSTuple( new InstallOVPN()),
                    new PSTuple( new StartSleep()),
                    new PSTuple( new StartSleep5()),
                    new PSTuple( new ProgExcuse()),
                    new PSTuple( new AdminExcuse()),
                    new PSTuple( new BOFH()),
                    new PSTuple( new RestartComputer()),
                    new PSTuple( new InstallChocolatey())
                  }
              );
                _runspace = RunspaceFactory.CreateRunspace();
                _runspace.Open();
                _state = new FreeRSState();
                Strategy = new JustExecuteStrategy();
            }//SINGLETON
            public static PSInvoker Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new PSInvoker();
                    }
                    return _instance;
                }
            }//SINGLETON
            internal PSReturnStrategy Strategy; //Patrón estrategia
            private PSInvokerState _state; //Patrón estado

            private void SetBusyState()
            {
                _state = new BusyRSState();
            }

            private void SetFreeState()
            {
                _state = new FreeRSState();
            }

            internal List<string[]> Invoke(PSCommand command, string arg)
            {
                return Strategy.ProcessResult(command, command.Normalize(_state.Invoke(Strategy.ProcessCommand(command,arg))),arg);
            }
        }
    }
}
