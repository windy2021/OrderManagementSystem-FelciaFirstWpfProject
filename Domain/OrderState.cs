using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class OrderState
    {
        protected OrderHeader _orderHeader;
        public abstract OrderStates State { get; }
        public abstract void Complete(ref OrderState state);
        public abstract void Reject(ref OrderState state);
        public abstract void Submit(ref OrderState state);
        public enum OrderStates
        {
            New = 1,
            Pending = 2,
            Rejected = 3,
            Complete = 4
        };
    }
}
