using Microsoft.VisualStudio.TestTools.UnitTesting;
using proyPractsDds.pS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace proyPractsDds.pS.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void PSClientConstructorTest()
        {//TEST DE CONSTRUCTOR DE TOTA LA VIDA
            PSClient pscli = new PSClient();
            Assert.IsNotNull(pscli);
        }

        [TestMethod()]
        public void SetInvokerStrategyTest()
        {//PASA POR TODAS LAS ESTRATEGIAS Y EJECUTA GET OS NAME (QUE ES INOFENSIVO)
            PSClient pscli = new PSClient();
            List<string> strats = pscli.AvailableStrategies;
            pscli.Command = "Get OS name";
            foreach (string strategy in strats)
            {
                pscli.SetInvokerStrategy(strategy);
                Assert.IsNotNull(pscli.ExecuteCommand(""));
            }
        }

        [TestMethod()]
        public void ExecuteCommandTest()
        {//PASA POR TODOS LOS COMANDOS Y OBTIENE EL COMANDO Y LA DESCRIPCIÓN, NO SE PUEDE HACER LA EJECUCIÓN POR MOTIVOS OBVIOS
            PSClient pscli = new PSClient();
            List<string> comms = pscli.AvailableCommands;
            foreach (string command in comms)
            {
                pscli.SetInvokerStrategy("Get Command"); 
                pscli.Command = command;
                Assert.IsNotNull(pscli.ExecuteCommand("Argument"));
                Assert.IsNotNull(pscli.ExecuteCommand(""));
                pscli.SetInvokerStrategy("Description");
                Assert.IsNotNull(pscli.ExecuteCommand(""));
            }

        }

        [TestMethod()]
        public void BusyStateTest()
        {
            // EJECUTA 2 SLEEPS EN PARALELO PARA QUE PASE POR ESTADO BUSY
            PSClient pscli = new PSClient();
            pscli.SetInvokerStrategy("Just Exec");
            pscli.Command = "Start-sleep 5";
            Task.Factory.StartNew(() => { Assert.IsNotNull(pscli.ExecuteCommand("")); });
            Thread.Sleep(1000);
            Task.Factory.StartNew(() => { Assert.IsNotNull(pscli.ExecuteCommand("")); });
            Assert.IsNotNull(pscli.ExecuteCommand(""));
            
        }
    }
}