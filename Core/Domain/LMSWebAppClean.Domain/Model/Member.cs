using LMSWebAppClean.Domain.Base;
using LMSWebAppClean.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Domain.Model
{
    public class Member : BaseUser
    {
        public List<Book> BorrowedBooks { get; set; }

        public Member() : base()
        {
            
        }

        public Member(string name) : base(name, UserType.Member)
        {
            BorrowedBooks = new List<Book>();
        }

        public override UserType Type
        {
            get { return type; }
            set
            {
                if (value != UserType.Member)
                {
                    throw new Exception("Invalid user type for member.");
                }
                type = value;
            }
        }
    }
}
