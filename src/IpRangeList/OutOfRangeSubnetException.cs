using System;

namespace IpRangeList
{
    public class OutOfRangeSubnetException : Exception
    {
        public OutOfRangeSubnetException()
        {

        }
        public OutOfRangeSubnetException(string message) : base(message)
        {

        }
    }
}
