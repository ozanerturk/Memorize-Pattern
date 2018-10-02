using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCacheExample
{
    public class DBContext
    {
         public static List<Student> Students =>
                  new List<Student>(){
                            new Student() {Age=11,Name="ozan",Number=1 },
                            new Student() {Age=8,Name="okan",Number=2 },
                            new Student() {Age=27,Name="ceren",Number=3},
                            new Student() {Age=28,Name="fatih",Number=4 },
                            new Student() {Age=34,Name="emin",Number=5 },
                            new Student() {Age=32,Name="aytaç",Number=6 },
                            new Student() {Age=29,Name="atılay",Number=7 },
            };
    }
}
