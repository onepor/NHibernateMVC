using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using ZAJCZN.MIS.Manager;


namespace ZAJCZN.MIS.Component
{
    public partial class ComponentDishes : BaseComponent<tm_Dishes, ManagerDishes>, IServiceDishes
    {
    }
}
