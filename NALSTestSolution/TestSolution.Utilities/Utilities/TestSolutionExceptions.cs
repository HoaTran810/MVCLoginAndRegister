using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolution.Application.Utilities
{
    public class TestSolutionExceptions : Exception
    {
        public TestSolutionExceptions()
        {
        }

        public TestSolutionExceptions(string message)
            : base(message)
        {
        }

        public TestSolutionExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
