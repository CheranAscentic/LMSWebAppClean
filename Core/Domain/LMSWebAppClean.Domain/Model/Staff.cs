using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Linq;

namespace LMSWebAppClean.Domain.Model
{
    public class Staff : BaseUser
    {
        public Staff()
            : base() { }

        public Staff(string name, string type) : base(name, type) { }

        public override string Type
        {
            get { return type; }
            set
            {
                string[] validTypes = { UserType.StaffMinor, UserType.StaffManagement };

                if (!validTypes.Contains(value))
                {
                    throw new Exception("Invalid user type for Staff.");
                }
                type = value;
            }
        }
    }
}
