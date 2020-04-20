using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Manager;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Component
{
    public class ComponentPayment : BaseComponent<tm_Payment, ManagerPayment>, IServicePayment
    {
    }
}
