using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab14;
using PersonLibrary;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Program.Make_University();
            Program.Select_Students(1);
            Program.Select_Students(2);
            Program.Select_Students(3);
            Program.Select_Students(4);
            Program.Select_Students(5);
            Program.Count_People_By_Gender("Мужской");
            Program.Count_People_By_Gender("Женский");
            Program.Persons_From_Two_Faculty();
            Program.Average_Students_Score();
            Program.Group_Students();
        }
    }
}
