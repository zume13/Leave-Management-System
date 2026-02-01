using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Value_Objects;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Domain.Entities
{
    public class Department : Entity
    {
        private Department(Guid id, Name deptname) : base(id)
        { 
            this.DepartmentName = deptname;
        }
        private Department() { }
        public Name DepartmentName { get; private set; }
        public static ResultT<Department> Create(Name deptName)
        {
            if (deptName == null)
                return DomainErrors.General.EmptyName;

            return ResultT<Department>.Success(new Department(Guid.NewGuid(), deptName));
        }
    }
}
