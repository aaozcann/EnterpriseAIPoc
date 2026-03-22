using Microsoft.Extensions.Logging;

namespace EnterpriseAIPoc.Services;

public class LeaveManagementService : ILeaveManagementService
{
    private readonly ILogger<LeaveManagementService> _logger;

    public LeaveManagementService(ILogger<LeaveManagementService> logger)
    {
        _logger = logger;
    }

    public int GetRemainingAnnualLeaveDays(string employeeName)
    {
        _logger.LogInformation("[RDBMS/ERP Call] '{EmployeeName}' için yıllık izin sorgulanıyor...", employeeName);

        // POC amaçlı sahte (mock) veri dönüyoruz. 
        // Gerçek bir senaryoda burası SQL, SAP, veya HRMS'e gider.
        if (employeeName.Equals("Ahmet", StringComparison.OrdinalIgnoreCase))
            return 14;

        if (employeeName.Equals("Ayşe", StringComparison.OrdinalIgnoreCase))
            return 22;

        return 0; // Eğer çalışan bulunamazsa veya izni yoksa
    }
}