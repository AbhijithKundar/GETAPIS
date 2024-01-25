using AngularAuthYtAPI.Models.ViewModel;

namespace AngularAuthYtAPI.Repository.Interface
{
    public interface IDashboardRepository
    {
        DashboardModel GetDashboard(string userName);
    }
}
