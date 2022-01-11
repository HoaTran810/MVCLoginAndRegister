using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolution.Application.Model.Dtos
{
    public class ApiClientResult
    {
        /// <summary>
        /// Ok=1 or Failed=2
        /// </summary>
        public byte Status;

        /// <summary>
        /// Message
        /// </summary>
        public string Message;
    }
}
