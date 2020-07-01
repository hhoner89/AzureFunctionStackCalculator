using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace StackCalculatorFunction
{
    public static class Function1
    {
        #region Starter Code from project creation
        /*
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
        */
        #endregion

        #region Lab 22 - Cloud Hosting - Azure Function Stack Calculator Methods

        public class StackCalculator
        {
            Stack<decimal> stack = new Stack<decimal>();
            StringBuilder result = new StringBuilder();

            public string Calculate(string[] commands)
            {
                foreach (var command in commands)
                {
                    var splits = command.Split(' ');

                    if (splits.Length > 0)
                    {
                        switch (splits[0])
                        {
                            case "PUSH":
                                Push(splits);
                                break;
                            case "POP":
                                Pop();
                                break;
                            case "PRINT":
                                Print();
                                break;
                            case "ADD":
                                Add();
                                break;
                            case "SUBTRACT":
                                Subtract();
                                break;
                            case "MULTIPLY":
                                Multiply();
                                break;
                            case "DIVIDE":
                                Divide();
                                break;
                            default:
                                Console.WriteLine("Unknown command");
                                break;
                        }
                    }
                }
                return result.ToString();
            }

            void Push(string[] splits)
            {
                if (splits.Length > 1)
                {
                    var number = decimal.Parse(splits[1]);
                    stack.Push(number);
                }
            }

            void Pop()
            {
                if (stack.Count > 0)
                {
                    var popped = stack.Pop();
                    result.AppendLine(popped.ToString());
                }
            }

            void Print()
            {
                if (stack.Count > 0)
                {
                    result.AppendLine(stack.Peek().ToString());
                }
            }

            void Add()
            {
                if (stack.Count >= 2)
                {
                    var numberOne = stack.Pop();
                    var numberTwo = stack.Pop();
                    var sum = numberOne + numberTwo;
                    stack.Push(sum);
                }
                else
                {
                    result.AppendLine("Error: could not perform Add function - there are only " + stack.Count + " items in the stack.");
                }
            }
            void Subtract()
            {
                if (stack.Count >= 2)
                {
                    var numberOne = stack.Pop();
                    var numberTwo = stack.Pop();
                    var difference = numberTwo - numberOne;
                    stack.Push(difference);
                }
                else
                {
                    result.AppendLine("Error: could not perform Subtract function - there are only " + stack.Count + " items in the stack.");
                }
            }
            void Multiply()
            {
                if (stack.Count >= 2)
                {
                    var numberOne = stack.Pop();
                    var numberTwo = stack.Pop();
                    var product = numberOne * numberTwo;
                    stack.Push(product);
                }
                else
                {
                    result.AppendLine("Error: could not perform Multiply function - there are only " + stack.Count + " items in the stack.");
                }
            }
            void Divide()
            {
                if (stack.Count >= 2)
                {
                    var numberOne = stack.Pop();
                    var numberTwo = stack.Pop();
                    
                    if(numberTwo != 0)
                    {
                        var quotient = numberTwo / numberOne;
                        stack.Push(quotient);
                    }
                }
                else
                {
                    result.AppendLine("Error: could not perform Divide function - there are only " + stack.Count + " items in the stack.");
                }
            }
        }

        public static class HeatherStackCalculator
        {
            [FunctionName("HeatherStackCalculator")]
            public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                ILogger log)
            {
                var lines = new List<string>();

                using (var reader = new StreamReader(req.Body))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        Console.WriteLine(line);
                        lines.Add(line);
                    }
                }

                var calculator = new StackCalculator();
                var response = calculator.Calculate(lines.ToArray());

                return new OkObjectResult(response);
            }
        }

        #endregion
    }
}
