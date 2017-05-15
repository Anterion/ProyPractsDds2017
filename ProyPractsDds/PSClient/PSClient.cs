using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyPractsDds.pS
{
    public partial class PSClient //business delegate
    {

        public string Command { get; set; }
        public string ArgumentType { get { return ((PSCommand) _invoker.AvailableCommands.Find(x => x.Id.Contains(Command)).PS).ArgumentType; } }
        public List<string> AvailableCommands { get; private set; }
        public List<string> AvailableStrategies { get; private set; }
        private List<PSTuple> _availableStrategies { get; set; }

        public PSClient()
        {
            _invoker = PSInvoker.Instance;
           

            _availableStrategies = new List<PSTuple>
            (
                new PSTuple[]
                {
                    new PSTuple( new GetDescription()),
                    new PSTuple( new SeeCommandStrategy()),
                    new PSTuple( new JustExecuteStrategy()),
                    new PSTuple( new SaveLogStrategy())
                }
            );

            AvailableCommands = _invoker.AvailableCommands.Select(command => command.ToString()).ToList();
            AvailableStrategies = _availableStrategies.Select(strategy => strategy.ToString()).ToList();
        }
        public void SetInvokerStrategy(string strategy)
        {
            _invoker.Strategy = (PSReturnStrategy) _availableStrategies.Find(x => x.Id.Equals(strategy)).PS;
        }
        private PSInvoker _invoker = PSInvoker.Instance;
        public List<string[]> ExecuteCommand( string arg)
        {
            return _invoker.Invoke((PSCommand)_invoker.AvailableCommands.Find(x => x.Id.Equals(Command)).PS, arg);
        }

        class PSTuple
        {
            public Object PS;
            public string Id;
            
            public PSTuple(PSCommand command)
            {
                PS = command;
                Id = command.ToString();
            }
            public PSTuple(PSReturnStrategy strategy)
            {
                PS = strategy;
                Id = strategy.ToString();
            }
            public override string ToString()
            {
                return PS.ToString();
            }
        }

    }
} //business delegate
