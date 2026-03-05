using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Models;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.AssignRole
{
    internal class AssignRoleCommandHandler(UserManager<User> _userManager, RoleManager<IdentityRole> roleManager) : ICommandHandler<AssignRoleCommand>
    {
        public async Task<Result> Handle(AssignRoleCommand command, CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(command.userId);

            if(user is null)
                return ApplicationErrors.Employee.NoEmployeesFound;
            
            var roleExits = await roleManager.RoleExistsAsync(command.role);

            if(!roleExits)
                return ApplicationErrors.Employee.InvalidRole;
            
            var isInRole = await _userManager.IsInRoleAsync(user, command.role);  
                
            if(isInRole)
                return ApplicationErrors.Employee.AlreadyInRole;

            var removeFromRolesResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

            if(!removeFromRolesResult.Succeeded)
                return ApplicationErrors.General.InternalError;

            var assignRoleResult = await _userManager.AddToRoleAsync(user, command.role);

            if(!assignRoleResult.Succeeded)
                return ApplicationErrors.General.InternalError;

            return Result.Success();
        }
    }
}
