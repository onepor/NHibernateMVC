using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Manager;

namespace ZAJCZN.MIS.Component
{
    public class ComponentLossOrder : BaseComponent<LossOrder, ManagerLossOrder>, IServiceLossOrder
    {
    }
}
