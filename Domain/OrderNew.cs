using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    class OrderNew : OrderState
    {
        public override OrderStates State
        {
            get
            {
                return OrderStates.New;
            }
        }
        public override void Complete(ref OrderState state)
        {
            state = new OrderComplete();
        }
        public override void Reject(ref OrderState state)
        {
            state = new OrderRejected();
        }
        public override void Submit(ref OrderState state)
        {
            state = new OrderPending();
        }
    }
}
