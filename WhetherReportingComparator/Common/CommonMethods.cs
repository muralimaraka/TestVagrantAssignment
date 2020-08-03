using System;
using System.Reflection;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace WhetherReportingComparator.Common
{
    public class CommonMethods
    {
        public void PrintObjectInfo(Object obj)
        {
            Type t1 = obj.GetType();
            PropertyInfo[] props = t1.GetProperties();
            for (int i = 0, j = 0; i < props.Length; i++, j++)
            {
                Extent.test.Log(Status.Info, props[i].Name + ":" + props[i].GetValue(obj));
                Logger.LogMessage(props[i].Name + ":" + props[i].GetValue(obj));
            }
        }
    }
}
