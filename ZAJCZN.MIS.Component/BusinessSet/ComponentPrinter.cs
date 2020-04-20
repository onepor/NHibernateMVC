using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Manager;

namespace ZAJCZN.MIS.Component
{
    public class ComponentPrinter:BaseComponent<tm_Printer,ManagerPrinter>,IServicePrinter
    {
    }
}
