using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcAssembly
{
    public class Calculator
    {
        private double _number;
        private const double _pi = 3.14;

        public Calculator(double number) { _number = number; }

        public double Number { get { return _number; } set { _number = value; } }
        public static double Pi { get { return _pi; } }

        public void Clear() { _number = 0.0; }
        private double Add(double number) { return _number + number; }
        public static double GetPi() { return _pi; }
    }
}
