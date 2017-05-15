using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace proyPractsDds.pS
{
    partial class PSClient //business delegate
    {
         abstract class PSCommand
        {
            abstract public string ArgumentType { get; }
            abstract public List<string[]> Description { get; } 
            protected static string[] _command;
            abstract public string GetCommand(string arg);
            abstract public List<string[]> Normalize(Collection<PSObject> PSresult);
        }

         class GetADComputers : PSCommand
        { //necesita un dominio de active directory para funcionar
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description { get { return new List<string[]> {
                new string[] { "Gets ComputerNames Asociated with domain" },
                new string[] { "Currently only SRVR is asociated with domain :(" },
            }; } }
            public override string ToString()
            {
                return "Get-ADcomputers";
            }
            new static string[] _command = { "Get-ADComputer -Filter * | where {$_.enabled} | select -expandProperty name" };
            public override string GetCommand(string arg)
            {
                return _command[0];
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                foreach (PSObject pso in PSresult)
                {
                    result.Add(new string[] { pso.ToString() });
                }
                return result;
            }
        }
         class GetUserName : PSCommand
        {
            public override string ArgumentType { get { return "Computername"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "Gets Current logged on User username" },
                new string[] { "arg: asks for logged on user on 'arg' computer" }
            };
                }
            }
            public override string ToString()
            {
                return "Get User";
            }
            new static string[] _command = { " Get-WMIObject -class Win32_ComputerSystem", " | select username" };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return _command[0] + _command[1];
                }
                else
                {
                    return _command[0] + " -computername " + arg + _command[1];
                }
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First().Properties["username"].Value ?? "<< Null result >>").ToString() });
                return result;
            }
        }
         class GetOSSerialNumber : PSCommand
        {
            public override string ArgumentType { get { return "Computername"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "Gets Operative system serial number" },
                new string[] { "arg: asks OS serial on 'arg' computer" }
            };
                }
            }
            public override string ToString()
            {
                return "Get OS serial";
            }
            new static string[] _command = { " Get-WMIObject -Class win32_operatingsystem ", " | select serialnumber" };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return _command[0] + _command[1];
                }
                else
                {
                    return _command[0] + " -computername " + arg + _command[1];
                }
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First().Properties["serialnumber"].Value ?? "<< it's Nothing>>").ToString() });
                return result;
            }
        }
         class GetOperatingSystem : PSCommand
        {
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "Gets Current Operative System Name" },
                new string[] { "arg: asks for Operative System name on 'arg' computer" },
            };
                }
            }
            public override string ArgumentType { get { return "Computername"; } }
            public override string ToString()
            {
                return "Get OS name";
            }
            new static string[] _command = { " Get-WMIObject -Class win32_operatingsystem ", " | select Caption" };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return _command[0] + _command[1];
                }
                else
                {
                    return _command[0] + " -computername " + arg + _command[1];
                }
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if(PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First().Properties["Caption"].Value ?? "<< it's Nothing>>").ToString() });
                return result;
            }
        }
         class GetComputerSerialNumber : PSCommand
        {
            public override string ArgumentType { get { return "Computername"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "Gets Current Operative System Serial number" },
                new string[] { "arg: asks for Operative System Serial number on 'arg' computer" },
            };
                }
            }
            public override string ToString()
            {
                return "Get MB serial";
            }
            new static string[] _command = { " Get-WMIObject -Class win32_bios ", " | select serialnumber" };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return _command[0] + _command[1];
                }
                else
                {
                    return _command[0] + " -computername " + arg + _command[1];
                }
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First().Properties["serialnumber"].Value ?? "<< it's Nothing>>").ToString() });
                return result;
            }
        }
         class InstallOVPN : PSCommand
        {//test de un comando más complejo, sólo funcionará en la red corporativa de mi empresa, así que no tiene sentido testearlo fuera de ella. 
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This scripts installs OpenVPN and its config in 'arg' computer" },
                new string[] { "Nowadays this wont work in this computer" },
                new string[] { "This was made for my job in Sesderma :) as sysadmin" },
            };
                }
            }
            public override string ToString()
            {
                return "Install OpenVPN";
            }
            new static string[] _command = { "$Computername = ", @"
                                #crea las conexiones de red
                                $S1= New-PSSession $COMPUTERNAME
                                $S2 =New-PSSession DFS1
                                #instala en el ordenador que va a recibir open vpn
                                Invoke-Command -Session $s1 -ScriptBlock {
                                    Set-ExecutionPolicy Remotesigned -Force
                                    iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex 
                                    choco install openvpn -y
                                }
                                #copia la configuración de DFS1 a localhost y de local a $computername
                                Copy-Item -FromSession $S2 -Path ""E:\DeployApps\Corporate\VPN\OpenVPN\OpenVPNconfigSesderma"" -Destination ""$env:ProgramFiles\openvpn\config"" -Force
                                get-childitem -path ""$env:ProgramFiles\openvpn\config""| %{Copy-Item -ToSession $S1 -Path ($_).fullname -Destination ""$env:ProgramFiles\openvpn\config"" -Force
                                }
                                #cierra las conexiones de red
                                Remove-PSSession $s1,$s2
                                " };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return _command[0] + "localhost" + _command[1];
                }
                else
                {
                    return _command[0] + arg + _command[1];
                }
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { "TODO :)" });
                return result;
            }

        }
         class ResetUserPassword : PSCommand
        {
            public override string ArgumentType { get { return "Username"; } }
            public override string ToString()
            {
                return "Reset AD pw";
            }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This command resets 'arg' user's password to:" },
                new string[] { "Ab1234" },
                new string[] { "You still need admin rights, so, only for Carlos" },
            };
                }
            }
            new static string[] _command = { "Set-ADAccountPassword -Identity ", " -NewPassword (ConvertTo-SecureString 'Ab1234' -AsPlainText -Force) -Reset" };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return "'Enter UserName'";
                }
                else return _command[0];
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                result.Add(new string[] { "password reset" });
                return result;
            }
        }
         class StartSleep : PSCommand
        {
            public override string ArgumentType { get { return "Seconds"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This command starts an 'arg' seconds pause " },
                new string[] { "Main use of this command is to test the threading of this app" }
            };
                }
            }
            public override string ToString()
            {
                return "Start-sleep X";
            }
            new static string[] _command = { "Start-sleep " };
            public override string GetCommand(string arg)
            {
                if (String.IsNullOrEmpty(arg))
                {
                    return "Start-sleep 1";
                }
                return _command[0] + arg;

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                result.Add(new string[] { " I'm awake! ☺" });
                return result;
            }
        }
         class StartSleep5 : PSCommand
        {
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This command starts a 5s pause, use to test threads" }
            };
                }
            }
            public override string ToString()
            {
                return "Start-sleep 5";
            }
            new static string[] _command = { "Start-sleep 5" };
            public override string GetCommand(string arg)
            {
                return _command[0];

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                result.Add(new string[] { "5 secs asleep" });
                return result;
            }
        }
         class ProgExcuse : PSCommand
        {
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This scripts travels to this url" },
                new string[] { "http://programmingexcuses.com/" },
                new string[] { "and extracts the text from the given programming excuse"}
            };
                }
            }
            public override string ToString()
            {
                return "Prog. excuse";
            }
            new static string[] _command = { "(Invoke-WebRequest -Uri http://programmingexcuses.com/).links.outerText" };
            public override string GetCommand(string arg)
            {
                return _command[0];

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First()).ToString() });
                return result;
            }
        }
         class AdminExcuse : PSCommand
        {
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This scripts travels to this url" },
                new string[] { "http://pages.cs.wisc.edu/~ballard/bofh/excuses" },
                new string[] { "and randomizes one sysadmin excuse"}
            };
                }
            }

            public override string ToString()
            {
                return "Admin. excuse";
            }
            new static string[] _command = { "((Invoke-WebRequest -Uri http://pages.cs.wisc.edu/~ballard/bofh/excuses).content).split(\"`n\")| where {$_}| Get-Random" };
            public override string GetCommand(string arg)
            {
                return _command[0];

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { (PSresult.First()).ToString() });
                return result;
            }
        }
         class BOFH : PSCommand
        {//test de un comando más complejo, sólo funcionará en la red corporativa de mi empresa, así que no tiene sentido testearlo fuera de ella. 
            public override string ArgumentType { get { return "None"; } }
            public override string ToString()
            {
                return "BOFH key party";
            }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "Just dont use this command, its very anoying" }
            };
                }
            }
            new static string[] _command = { @"
                                powershell.exe {$wshell = New-Object -ComObject wscript.shell;while($true)
                                { 
                                $wshell.SendKeys('{CAPSLOCK}');
                                $wshell.SendKeys('{NUMLOCK}');
                                $wshell.SendKeys('{SCROLLLOCK}');
                                Start-Sleep -Milliseconds 50}
                                }
                                -windowstyle hidden
                                " };
            public override string GetCommand(string arg)
            {
                return _command[0];
            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                result.Add(new string[] { "Play DarudeSandstorm.mp3 along with it" });
                return result;
            }


        }
         class RestartComputer : PSCommand
        {
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This command will restart this computer :)" }
            };
                }
            }
            public override string ToString()
            {
                return "Restart Computer";
            }
            new static string[] _command = { "Restart-Computer -force" };
            public override string GetCommand(string arg)
            {
                return _command[0];

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                return result;
            }
        }
         class InstallChocolatey : PSCommand
        {
            public override string ArgumentType { get { return "None"; } }
            public override List<string[]> Description
            {
                get
                {
                    return new List<string[]> {
                new string[] { "This command will install chocolatey on this computer" },
                new string[] { "But it is already installed XD so..."}
            };
                }
            }
            public override string ToString()
            {
                return "Install Choco";
            }
            new static string[] _command = { @"
            powershell.exe -NoProfile -ExecutionPolicy Bypass -Command ""iex((New - Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))"" && SET ""PATH =% PATH %;%ALLUSERSPROFILE%\chocolatey\bin""
            "};
            public override string GetCommand(string arg)
            {
                return _command[0];

            }
            public override List<string[]> Normalize(Collection<PSObject> PSresult)
            {
                List<string[]> result = new List<string[]>();
                if (PSresult.Count == 0) { return result; }
                foreach (PSObject a in PSresult)
                {
                    result.Add(new string[] { a.ToString() });
                }
                return result;
            }
        }
    }
}
