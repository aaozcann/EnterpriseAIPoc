using System.ComponentModel;
using EnterpriseAIPoc.Services;
using Microsoft.SemanticKernel;

namespace EnterpriseAIPoc.Plugins;

public class LeaveManagementPlugin
{
    private readonly ILeaveManagementService _leaveService;

    // Gerçek DI Entegrasyonumuz
    public LeaveManagementPlugin(ILeaveManagementService leaveService)
    {
        _leaveService = leaveService;
    }

    [KernelFunction("GetRemainingLeave")]
    [Description("Belirli bir çalışanın şirket sistemindeki kalan yıllık izin günlerini sorgular.")]
    public int GetRemainingLeaveDays(
        [Description("İzni sorgulanacak çalışanın adı, örneğin: Ahmet, Ayşe")] string employeeName)
    {
        // Plugin sadece iş servisini (Service layer) çağırır, asıl işi Service yapar.
        return _leaveService.GetRemainingAnnualLeaveDays(employeeName);
    }
}