using System;
using System.Collections.Generic;
using System.Globalization;

namespace BudgetManager
{
    public class Calc
    {
        public static double Calculate(string input) // метод счета с ним работаем 
        {
            var output = GetExpression(input);
            var result = Counting(output);
            return result;
        }
        private static string GetExpression(string input)  //  метод получение из строки опз
        {
            var output = string.Empty;
            var operationStack = new Stack<char>();
            for (byte i = 0; i < input.Length; i++) 
            {
               
                if (IsDelimeter(input[i]))
                    continue;
                if (char.IsDigit(input[i]) || input[i] == ',') 
                {
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i];
                        i++; 

                        if (i == input.Length) break; 
                    }
                    output += " ";
                    i--; 
                }
                if (!IsOperator(input[i])) continue;
                if (input[i] == '(')
                    operationStack.Push(input[i]); 
                else if (input[i] == ')')
                {
                    var s = operationStack.Pop();
                    while (s != '(')
                    {
                        output += s.ToString() + ' ';
                        s = operationStack.Pop();
                    }
                }
                else 
                {
                    if (operationStack.Count > 0) 
                        if (GetPriority(input[i]) <= GetPriority(operationStack.Peek()))
                            output += operationStack.Pop() + " ";
                    operationStack.Push(char.Parse(input[i].ToString()));
                }
            }

           
            while (operationStack.Count > 0)
                output += operationStack.Pop() + " ";

            return output;
        }

        private static double Counting(string input) //  метод который получает опз и выдает результат
        {
            double result = 0; 
            var temp = new Stack<double>();
            for (byte i = 0; i < input.Length; i++) 
            {
                if (char.IsDigit(input[i]) || input[i] == ',')
                {
                    var a = string.Empty;
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) 
                    {
                        a += input[i]; 
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(double.Parse(a)); 
                    i--;
                }
                else if (IsOperator(input[i])) 
                {
                    var a = temp.Pop();
                    var b = temp.Pop();
                    switch (input[i]) 
                    {
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case '*': result = b * a; break;
                        case '/': result = b / a; break;
                        case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString(CultureInfo.CurrentCulture)), 
                            double.Parse(a.ToString(CultureInfo.CurrentCulture))).ToString(CultureInfo.CurrentCulture)); break;
                    }
                    temp.Push(result); 
                }
            }
            return temp.Peek(); 
        }
        private static bool IsDelimeter(char c)
        {
            return " =".IndexOf(c) != -1;
        }
        private static bool IsOperator(char с)
        {
            return "+-/*^()".IndexOf(с) != -1;
        }
        private static byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }
    }
}
