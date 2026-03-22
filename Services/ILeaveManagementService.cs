namespace EnterpriseAIPoc.Services;

public interface ILeaveManagementService
{
    int GetRemainingAnnualLeaveDays(string employeeName);
}