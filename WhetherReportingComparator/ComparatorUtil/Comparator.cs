using System;
using System.Collections.Generic;
using System.Reflection;

namespace WhetherReportingComparator.ComparatorUtility
{
    public class Comparator
    {
        public Dictionary<string, string> Compare(object obj1, object obj2)
        {
            try
            {
                var actualVariance = new Dictionary<string, string>();
                if ((obj1 == null) || (obj2 == null)) return null;

                Type t1 = obj1.GetType();
                Type t2 = obj2.GetType();

                PropertyInfo[] props1 = t1.GetProperties();
                PropertyInfo[] props2 = t2.GetProperties();

                for (int i = 0, j = 0; i < props1.Length; i++, j++)
                {

                    if (props1[i].Name == props2[j].Name)
                    {
                        switch (props1[i].PropertyType.Name)
                        {
                            case "Double":
                                {
                                    double val1 = 0.0d; double val2 = 0.0d; double val3 = 0.0d;
                                    val1 = Convert.ToDouble(props1[i].GetValue(obj1));
                                    val2 = Convert.ToDouble(props2[i].GetValue(obj2));
                                    val3 = Math.Abs(val1 - val2);
                                    actualVariance.Add(props1[i].Name, val3.ToString("0.####"));
                                    break;
                                }
                            case "Int32":
                                {
                                    int val1 = 0; int val2 = 0; int val3 = 0;
                                    val1 = Convert.ToInt32(props1[i].GetValue(obj1));
                                    val2 = Convert.ToInt32(props2[i].GetValue(obj2));
                                    val3 = Math.Abs(val1 - val2);
                                    actualVariance.Add(props1[i].Name, val3.ToString());
                                    break;
                                }

                            case "String":
                                {
                                    string val1 = props1[i].GetValue(obj1).ToString();
                                    string val2 = props2[i].GetValue(obj2).ToString();
                                    int val3 = string.Compare(val1, val2);
                                    actualVariance.Add(props1[i].Name, val3.ToString());
                                    break;
                                }

                            default:
                                {
                                    Console.WriteLine("Property type mismtach Failed @ Comparator");
                                    actualVariance = null;
                                    break;
                                }
                        }
                    }
                }
                return actualVariance;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed @ Comparator" + ex.Message);
                return null;
            }

        }
    }
}

