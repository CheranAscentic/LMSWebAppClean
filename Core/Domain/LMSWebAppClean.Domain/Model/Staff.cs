using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Domain.Model
{
    public class Staff : BaseUser
    {
        public Staff() : base()
        {
            
        }
        public Staff(string name, UserType type) : base(name, type) { }

        public override UserType Type
        {
            get { return type; }
            set
            {
                UserType[] validTypes = { UserType.StaffMinor, UserType.StaffManagement };

                if (!validTypes.Contains(value) || value == null)
                {
                    throw new Exception("Invalid user type for Staff.");
                }
                type = value;
            }
        }
    }
}
