using LMSWebAppClean.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.DTO
{
    public class StandardRequestObject<T> where T : IQuery, ICommand
    {
        public int AuthId { get; set; }
        public T Request { get; set; }
    }
}
