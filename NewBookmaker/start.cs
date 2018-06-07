using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Better.Bookmakers
{
    class start
    {
        /// <summary>
        /// Точка входа для тестов в консоли
        /// </summary>
        /// <returns></returns>
        public static void Main()
        {

            // Запустим в консоли
            NewBookmaker bookmaker = new NewBookmaker();
            bookmaker.init();
            bookmaker.ExceptionCallback = exceptionCallback;

            bookmaker.Start("https://fonbet.com/#!/live", "").Wait();

        }

        /// <summary>
        /// Коллбек ошибок для тестов в консоли чисто для примера (сделаете как удобно, если надо)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        static void exceptionCallback(string name, string path = "", object data = null)
        {
            System.Diagnostics.Debugger.Break();
        }

    }
}
