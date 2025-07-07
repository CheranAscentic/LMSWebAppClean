using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.DTO
{
    public class StandardRequestObject<T> where T : class
    {
        public int Bearer { get; set; }
        public T? Data { get; set; }

        public StandardRequestObject()
        {
        }

        public StandardRequestObject(int bearer, T? data)
        {
            Bearer = bearer;
            Data = data;
        }
    }
}
