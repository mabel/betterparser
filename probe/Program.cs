using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Better.Bookmakers.Types;

namespace Better.Bookmakers

{

    public class DummyParser : ParserInterface
    {

            public int BookmakerID { get; set; }

            /// <summary>
            /// * Кодировка страницы матчей
            /// 65001 = utf8; 1251 = win-1251; 20866 = KOI8-R;
            /// </summary>
            public int Encoding_Matches { get; set; }

            /// <summary>
            /// * Кодировка страницы одного матча
            /// </summary>
            public int Encoding_OneMatch { get; set; }

            /// <summary>
            /// Инициализация при первичной подгрузке парсера
            /// </summary>
            /// <returns>Возвращает результат инициализации, т.е. готовность парсера к работе</returns>
            public bool init(){return true;}


            /// <summary>
            /// Точка входа в библиотеку. Отделяет полезные запросы от бесполезных и направляет каждый на свои парсеры
            /// </summary>
            /// <param name="page_type">Тип страницы: список матчей или страница кефа. Если страницы кефов нет - по умолчанию страница матчей</param>
            /// <param name="request_type">Тип запроса: обычный или фрейм WebSocket`а</param>
            /// <param name="request">Инфо с обычного запроса, либо NULL</param>
            /// <param name="wsFrame">Инфо о фрейме WebSocket`а, либо NULL</param>
            /// <returns>Вернуть распаршенную структуру Parsed или NULL, если запросу парсинг не требуется</returns>
            public Parsed Router(pageTypes page_type, requestTypes request_type, ref NormalRequest request, ref WebSocketRequest wsFrame)
            {
                return new Parsed();
            }

            /// <summary>
            /// Запустить парсер нужной страницы, отправляя запросы вручную
            /// Предпочтение отдавать асинхронным запросам и сохранению куков
            /// </summary>
            /// <param name="urlMatches">Ссылка на страницу списка матчей, если надо запустить её. Иначе - пустая строка</param>
            /// <param name="urlOneMatch">Ссылка на страницу конкретного матча, если надо запустить парсинг кефов. Иначе - пустая строка.</param>
            /// <param name="speed_kef">Коэффициент ускорения отправки запросов</param>
            /// <param name="callback">Коллбек, который надо вызвать со структурой Parsed, которую вернёт роутер, если нужно</param>
            public Task Start(string urlMatches, string urlOneMatch, double speed_kef = 1, Delegate callback = null)
            {
                return null;
            }

            /// <summary>
            /// Отправить ошибку парса в основную прогу немедленно [асинхронно]
            /// [реализовано через callback, чтобы редкие виды ошибок не приводили к неотправке самого списка ошибок]
            /// </summary>
            /// <param name="name">Название\описание ошибки</param>
            /// <param name="path">Путь до ошибки. Реализовано в виде строки можно было тонко описать момент её возникновения</param>
            /// <param name="data">Любые прикладные данные этой ошибки, которые вам нужно будет знать для её исправления. Будет сериализовано в json и записано.</param>
            public void sendException(string name, string path = "", object data = null)
            {
                // TODO
            }

    }

    class Program
    {
        static void Main(string[] args)
        {
            DummyParser dp = new DummyParser();
            Console.WriteLine(dp.GetType());
        }
    }
}
