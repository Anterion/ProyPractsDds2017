using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace proyPractsDds.pS
{
    partial class PSClient 
    {

         abstract class PSReturnStrategy
        {
            abstract internal string ProcessCommand(PSCommand command, string arg);
            abstract internal List<string[]> ProcessResult(PSCommand command, List<string[]> result, string arg);
        }//Patrón estrategia

         class JustExecuteStrategy : PSReturnStrategy
        {
            override internal string ProcessCommand(PSCommand command, string arg)
            {
                return command.GetCommand(arg);
            }
            public override string ToString()
            {
                return "Just Exec";
            }
            override internal List<string[]> ProcessResult(PSCommand command, List<string[]> result, string arg)
            {
                List<string[]> aux = new List<string[]> { new string[] { command.ToString() + " Execution:" } };
                foreach (string[] str in result)
                {
                    aux.Add(str);
                }
                return aux;
            }
        }//Patrón estrategia

         class SeeCommandStrategy : PSReturnStrategy
        {
            override internal string ProcessCommand(PSCommand command, string arg)
            {
                return "out-null";
            }
            public override string ToString()
            {
                return "Get Command";
            }
            override internal List<string[]> ProcessResult(PSCommand command, List<string[]> result, string arg)
            {
               // result.Insert(0, new string[] { command.ToString() + ":" });
                return new List<string[]> { new string[] { command.ToString() + " Command:" } ,new string[]{command.GetCommand(arg) }};
            }
        }//Patrón estrategia

         class GetDescription : PSReturnStrategy
        {
            override internal string ProcessCommand(PSCommand command, string arg)
            {
                return "out-null";
            }
            public override string ToString()
            {
                return "Description";
            }
            override internal List<string[]> ProcessResult(PSCommand command, List<string[]> result, string arg)
            {

                List<string[]> aux = new List<string[]> { new string[] { command.ToString() + " Description:" } };
                foreach (string[] str in command.Description)
                {
                    aux.Add(str);
                }
                return aux;

            }
        }//Patrón estrategia
         class SaveLogStrategy : PSReturnStrategy
        {
            public override string ToString()
            {
                return "Log in C:\\ no va";
            }
            override internal string ProcessCommand(PSCommand command, string arg)
            {
                return "$mylog=("+command.GetCommand(arg)+");$mylog|out-file C:\\log.txt -append -force";
            }
            override internal List<string[]> ProcessResult(PSCommand command, List<string[]> result, string arg)
            {
                return new List<string[]>();

            }
        }//Patrón estrategia
    }
}

