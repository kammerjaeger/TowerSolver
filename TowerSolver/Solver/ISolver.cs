using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSolver.Solver
{
    internal interface ISolver
    {
        public Task<byte[]> Solve(byte[] input);
    }
}
