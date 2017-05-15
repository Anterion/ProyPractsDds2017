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
    partial class PSClient
    {
        partial class PSInvoker
        {
            abstract class PSInvokerState
            {

                internal abstract Collection<PSObject> Invoke(string script);

            }

            class FreeRSState : PSInvokerState
            {
                internal override Collection<PSObject> Invoke(string script)
                {
                    PowerShell powershell = PowerShell.Create();
                    powershell.Runspace = PSInvoker.Instance._runspace;
                    powershell.AddScript(script);
                    Instance.SetBusyState();
                    Collection<PSObject> result = powershell.Invoke();
                    Instance.SetFreeState();
                    return result;
                }
            }

            class BusyRSState : PSInvokerState
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
            }

        }
    }
}
