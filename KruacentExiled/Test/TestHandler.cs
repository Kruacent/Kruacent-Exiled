using Exiled.API.Extensions;
using Exiled.API.Features;
using InventorySystem.Items.Firearms.Modules.Scp127;
using KE.Utils.API;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace KruacentExiled.Test
{
    public class TestHandler
    {
        public void Start()
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            int count = 0;
            int countSuccess = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsDefined(typeof(TestClassAttribute), true))
                    continue;

                if (type.IsAbstract)
                    continue;

                object instance = Activator.CreateInstance(type);

                MethodInfo[] methods = type.GetMethods(
                    BindingFlags.Public | BindingFlags.Instance
                );

                foreach (MethodInfo method in methods)
                {
                    if (method.IsDefined(typeof(TestMethodAttribute), true))
                    {
                        count++;
                        if(ExecuteTest(instance, method, null))
                        {
                            countSuccess++;
                        }
                        continue;
                    }

                    TestCaseAttribute[] testCases =
                        method.GetCustomAttributes(typeof(TestCaseAttribute), true)
                            .Cast<TestCaseAttribute>()
                            .ToArray();

                    foreach (TestCaseAttribute testCase in testCases)
                    {
                        count++;
                        if(ExecuteTest(instance, method, testCase.Arguments))
                        {
                            countSuccess++;
                        }
                    }
                }
            }

            Log.SendRaw($"Test passed : {countSuccess}/{count}", ConsoleColor.DarkGreen);
            int testFailed = count - countSuccess;
            if(testFailed > 0)
            {
                Log.SendRaw($"Test failed : {testFailed}/{count}", ConsoleColor.DarkRed);
            }
            

        }

        private bool ExecuteTest(object instance, MethodInfo method, object[] arguments)
        {
            bool result = false;
            try
            {
                LogExecuting(method);

                method.Invoke(instance, arguments ?? new object[0]);

                LogSuccess();
                result = true;
            }
            catch (TargetInvocationException ex)
            {
                Exception exception = ex.InnerException ?? ex;
                if (exception is AssertionException)
                {
                    LogFail(exception.Message);
                }
                else
                {
                    LogFail($"Unexpected exception: {exception}");
                }
            }
            catch (Exception ex)
            {
                LogFail($"Runner error: {ex}");
            }
            return result;
        }


        private void LogFail(string messageIfFailed)
        {
            Log.SendRaw("Assertion failed : " + messageIfFailed, ConsoleColor.DarkRed);
        }


        private void LogSuccess()
        {
            Log.SendRaw("Success", ConsoleColor.DarkGreen);
        }

        private void LogExecuting(MethodInfo method)
        {
            Log.SendRaw($"Executing: {method.Name}", ConsoleColor.White);
        }

    }
}
