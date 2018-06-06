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

            bookmaker.Start("https://fonbet.com/#!/live", "").Wait();

        }

    }
}
