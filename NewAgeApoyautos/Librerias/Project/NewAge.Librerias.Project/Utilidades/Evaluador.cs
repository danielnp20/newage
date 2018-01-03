using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    public class Evaluador
    {
        /**
        * Ya construida la lista del tipo:
        * [nodo número] [nodo operador] [nodo número] [nodo operador] .....
        * es ir del operador con mas precedencia al de menos precedencia.
        * 
        * Este método se llama después de haber sido analizada la expresión.
        * 
        * @return El valor de la expresión evaluada (double)
        */
        public static double Evaluate(string expr)
        {
            Stack<String> stack = new Stack<String>();

            string value = "";
            for (int i = 0; i < expr.Length; i++)
            {
                String s = expr.Substring(i, 1);
                char chr = s.ToCharArray()[0];

                if (!char.IsDigit(chr) && chr != ',' && value != "")
                {
                    stack.Push(value);
                    value = "";
                }

                if (s.Equals("("))
                {

                    string innerExp = "";
                    i++; //Fetch Next Character
                    int bracketCount = 0;
                    for (; i < expr.Length; i++)
                    {
                        s = expr.Substring(i, 1);

                        if (s.Equals("("))
                            bracketCount++;

                        if (s.Equals(")"))
                            if (bracketCount == 0)
                                break;
                            else
                                bracketCount--;


                        innerExp += s;
                    }

                    stack.Push(Evaluate(innerExp).ToString());

                }
                else if (s.Equals("+")) stack.Push(s);
                else if (s.Equals("-")) stack.Push(s);
                else if (s.Equals("*")) stack.Push(s);
                else if (s.Equals("/")) stack.Push(s);
                else if (s.Equals("sqrt")) stack.Push(s);
                else if (s.Equals(")"))
                {
                }
                else if (char.IsDigit(chr) || chr == ',')
                {
                    value += s;

                    if (value.Split(',').Length > 2)
                        throw new Exception("Invalid decimal.");

                    if (i == (expr.Length - 1))
                        stack.Push(value);

                }
                else
                    throw new Exception("Invalid character.");

            }


            double result = 0;
            while (stack.Count >= 3)
            {

                double right = Convert.ToDouble(stack.Pop());
                string op = stack.Pop();
                double left = Convert.ToDouble(stack.Pop());

                if (op == "+") result = left + right;
                else if (op == "+") result = left + right;
                else if (op == "-") result = left - right;
                else if (op == "*") result = left * right;
                else if (op == "/") result = left / right;

                stack.Push(result.ToString());
            }


            return Convert.ToDouble(stack.Pop());
        }

        /// <summary>
        /// Función para determinar el digito de verif segun NIT
        /// </summary>
        /// <param name="_nit">numero de identidad o NIT del tercero</param>
        /// <returns>digito</returns>
        public static int Nit_DV(string _nit)
        {
            //Arreglo de 15 posiciones
            List<int> arr = new List<int>(15);
            arr.Add(71); // Pos 1
            arr.Add(67); // Pos 2
            arr.Add(59); // Pos 3
            arr.Add(53); // Pos 4
            arr.Add(47); // Pos 5
            arr.Add(43); // Pos 6
            arr.Add(41); // Pos 7
            arr.Add(37); // Pos 8
            arr.Add(29); // Pos 9
            arr.Add(23); // Pos 10
            arr.Add(19); // Pos 11
            arr.Add(17); // Pos 12
            arr.Add(13); // Pos 13
            arr.Add(7);  // Pos 14
            arr.Add(3);  // Pos 15

            int lnRetorno = 0;
            //Completa el nit con espacios en blanco a la izquierda para que quede de 15 posiciones
            _nit = _nit.Trim();
            string comleteDato = _nit.PadLeft(15);
            string dato = comleteDato.Substring(comleteDato.Length - 15);

            int WSuma = 0;

            for (int i = 0; i < 15; ++i)
            {
                string datoInt = dato.Substring(i, 1);
                try
                {
                    int aux = Convert.ToInt32(datoInt);
                    WSuma += aux * arr[i];
                }
                catch (Exception e) { }
            }

            WSuma = WSuma % 11;

            if (WSuma == 0 || WSuma == 1)
            {
                lnRetorno = WSuma;
            }
            else
            {
                lnRetorno = 11 - WSuma;
            }

            return lnRetorno;
        }

        /// <summary>
        /// Redondea un entero
        /// </summary>
        /// <param name="num">Numero  redondear</param>
        /// <param name="places">Numero de digitos que se deben redondear</param>
        /// <returns></returns>
        public static int RedondearEntero(int num, int places)
        {
            string numStr = Convert.ToString(num.ToString());
            int length = numStr.Length;
            string lastNro = numStr.Substring(length - 1, 1);
            int n;
            if (places >= length)
            {
                n = Convert.ToInt16(lastNro);
                if (n <= 4)
                    return 0;
                else
                    return 1;
            }

            int valCompare = Convert.ToInt16(numStr.Substring(places, 1));
            string ini = numStr.Substring(0, length - places);
            string fin = string.Empty;

            for (int i = 0; i < places; ++i)
                fin += "0";

            double vlrini = Convert.ToInt32(ini);
            if (valCompare >= 5)
                vlrini += 1;

            string total = vlrini + fin;
            return Convert.ToInt32(total);
        }

        /// <summary>
        /// Calcula el valor de la cuota de un credito de cartera
        /// Aplica cuanto el componente es de tipo saldo
        /// </summary>
        /// <param name="valorPrestamo">Valor del prestamo</param>
        /// <param name="plazo">Plazo número de cuotas</param>
        /// <param name="tasaTotal">Porcentaje para hacer el calculo</param>
        /// <returns>Retorna el valor de la cuota, para un componente</returns>
        public static int GetCuotaCreditoCartera(int valorPrestamo, int plazo, decimal tasaTotal)
        {
            try
            {
                int valorCuota = 0;

                if (tasaTotal != 0)
                {
                    double plazoTemp = (double)plazo;
                    double tasaTemp = (double)tasaTotal;
                    double pretamosTemp = (double)valorPrestamo;

                    double t = Math.Pow((1 + (tasaTemp / 100)), plazoTemp);
                    double r = (tasaTemp / 100 * t) / (t - 1);

                    //Temporal
                    valorCuota = Convert.ToInt32(valorPrestamo * r);
                }
                else
                {
                    if (plazo != 0)
                    {
                        valorCuota = valorPrestamo / plazo;
                        if (valorCuota * plazo != valorPrestamo)
                            valorCuota = 0; 
                    }
                }
                
                return valorCuota;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Devuelve un valor Long que especifica el número de
        /// intervalos de tiempo entre dos valores Date.
        /// </summary>
        /// <param name="interval">Obligatorio. Valor de enumeración
        /// DateInterval o expresión String que representa el intervalo
        /// de tiempo que se desea utilizar como unidad de diferencia
        /// entre Date1 y Date2.</param>
        /// <param name="date1">Obligatorio. Date. Primer valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <param name="date2">Obligatorio. Date. Segundo valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            long rs = 0;
            TimeSpan diff = date2.Subtract(date1);
            switch (interval)
            {
                case DateInterval.Day:
                case DateInterval.DayOfYear:
                    rs = (long)diff.TotalDays;
                    break;
                case DateInterval.Hour:
                    rs = (long)diff.TotalHours;
                    break;
                case DateInterval.Minute:
                    rs = (long)diff.TotalMinutes;
                    break;
                case DateInterval.Month:
                    rs = (date2.Month - date1.Month) + (12 * DateTimeExtension.DateDiff(DateInterval.Year, date1, date2));
                    break;
                case DateInterval.Quarter:
                    rs = (long)Math.Ceiling((double)(DateTimeExtension.DateDiff(DateInterval.Month, date1, date2) / 3.0));
                    break;
                case DateInterval.Second:
                    rs = (long)diff.TotalSeconds;
                    break;
                case DateInterval.Weekday:
                case DateInterval.WeekOfYear:
                    rs = (long)(diff.TotalDays / 7);
                    break;
                case DateInterval.Year:
                    rs = date2.Year - date1.Year;
                    break;
            }//switch
            return rs;
        }//Dat
    }


}
