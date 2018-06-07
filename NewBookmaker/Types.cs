using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Структуры, которые получают и должны вернуть парсеры
/// </summary>
namespace Better.Bookmakers.Types
{

    #region "ЗАПРОСЫ"

    /// <summary>
    /// Один обычный запрос к серверу (Get\Post\Fetch\итд)
    /// </summary>
    public class NormalRequest
    {

        /// <summary>
        /// Полная строка запроса (пример: https://www.site.com/info/request.php?id=5)
        /// </summary>
        string request_url;

        /// <summary>
        /// Строка с заголовками, разделёнными по CrLf, из ответа сервера
        /// (необязательно указывать при симуляции, если инфа не нужна)
        /// </summary>
        string response_headers;

        /// <summary>
        /// Строка с текстовым ответом сервера, раскодированная по указанной кодировке (в Encoding_Matches или Encoding_OneMatch соответственно)
        /// </summary>
        string response_data;

    }

    /// <summary>
    /// Один фрейм WebSocket`а
    /// </summary>
    public class WebSocketRequest
    {

        /// <summary>
        /// Путь к websocket`у (пример: https://www.site.com/info/request.php?id=5)
        /// </summary>
        string WebSocket_url;

        /// <summary>
        /// Тип фрейма
        /// </summary>
        WebSocketRequestFrameType FrameType;

        /// <summary>
        /// Содержимое текстового фрейма (0x1), раскодированного по указанной кодировке (в Encoding_Matches или Encoding_OneMatch соответственно)
        /// </summary>
        string StringFrame;

        /// <summary>
        /// Содержимое двоичного фрейма (0x1)
        /// </summary>
        byte[] BinaryFrame;

        /// <summary>
        /// Тип фрейма
        /// </summary>
        public enum WebSocketRequestFrameType : byte { Text = 1, Binary = 2 }

    }


    /// <summary>
    /// Тип поступившего запроса
    /// </summary>
    public enum requestTypes : byte
    {
        /// <summary>
        /// Любой обычный запрос (get\post\fetch\etc)
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Один фрейм WebSocket`а
        /// </summary>
        WebSocket = 2
    }

    /// <summary>
    /// Тип страницы
    /// </summary>
    public enum pageTypes : byte
    {
        /// <summary>
        /// Страница со списком матчей
        /// </summary>
        MatchesList = 1,

        /// <summary>
        /// Страница с конкретным матчем
        /// </summary>
        OneMatch = 2
    }


    #endregion

    /// <summary>
    /// Результат парсинга всех страниц целиком описывается этим классом
    /// Если запрос отдаёт и матчи и кефы, то их всё-равно надо разделить по двум вложенным структурам
    /// </summary>
    public class Parsed
    {

        /// <summary>
        /// Если на запросе была распаршена страница со списком матчей - создаём этот объект и заполняем его
        /// </summary>
        ParsedMatches matches;

        /// <summary>
        /// Если была распаршена страница с одним матчем - заполняем структуру с кефами
        /// </summary>
        ParsedKefs kefs;

    }

    /// <summary>
    /// Спаршенный список матчей со страницы матчей
    /// </summary>
    public class ParsedMatches
    {

        /// <summary>
        /// Режим парсера матчей: распаршена вся страница или обновлён только её кусочек
        /// </summary>
        ParserModes mode;

        /// <summary>
        /// Список матчей, которые были добавлены или обновлены для обоих режимов парсинга
        /// </summary>
        List<Match> matches = new List<Match>();

        /// <summary>
        /// Уникальные ID матчей (Match.ID) на странице-источнике, которые были удалены в режиме LiveMode
        /// </summary>
        List<string> deletedMatches;

        /// <summary>
        /// Один матч со страницы матчей
        /// </summary>
        public class Match
        {
            /// <summary>
            /// Любой УНИКАЛЬНЫЙ идентификатор вида спорта на сайте источнике
            /// Может быть прямым ID или записью типа "Волейбол", чем угодно
            /// </summary>
            string Sport;

            /// <summary>
            /// Уникальный ID матча на сайте-источнике
            /// </summary>
            string ID;

            /// <summary>
            /// Имена команд 
            /// [избавление от тегов и нормализация над ними будет проведена в основной программе, можно не парится]
            /// </summary>
            string Team1Name, Team2Name;

            /// <summary>
            /// Полная ссылка на матч, если существует
            /// </summary>
            string link;

            /// <summary>
            /// Название турнира, лига, дивизион или другая подкатегория в этом виде спорта.
            /// Если содержит много уровней категорий, то привести к одной строке, разделяя точкой.
            /// </summary>
            string category;

            /// <summary>
            /// Счёт в виде цельной строки (без парса её составляющих), если указан
            /// </summary>
            string score;

            /// <summary>
            /// Время матча, как оно указано на сайте, если есть
            /// </summary>
            string time;

            /// <summary>
            /// Если есть видео, указать прямую ссылку на его iframe (src=...)
            /// </summary>
            string video;

        }

    }

    /// <summary>
    /// Коэффициенты одного спаршенного матча
    /// </summary>
    public class ParsedKefs
    {

        /// <summary>
        /// Режим парсера кефов: перепаршены все кефы целиком или только отдельные из них
        /// </summary>
        ParserModes mode;

        /// <summary>
        /// Список матчей, для которых были распаршены линии и кефы
        /// Обычно со страницы матча парсится 1 матч, и массив будет из одного элемента
        /// но есть Фонбет и мб. другие буки, где матчи в запросе приходят сразу все
        /// </summary>
        List<Match> matches;

        /// <summary>
        /// Один матч
        /// </summary>
        public class Match
        {

            /// <summary>
            /// Уникальный ID матча на странице источнике 
            /// равен тому, что парсится со страницы матчей ParsedMatches.Match.ID
            /// </summary>
            string ID;

            /// <summary>
            /// [FullMode] Список всех линий с вложенными кефами, режим распарса всех кефов целиком
            /// </summary>
            List<Line> lines;

            /// <summary>
            /// [LiveMode] Список новых линий БЕЗ вложенных кефов, только описание линии
            /// </summary>
            List<Line> NewLines;

            /// <summary>
            /// [LiveMode] Список уникальных айдишников линий Line.ID, которые были удалены, на сайте-источнике
            /// </summary>
            List<string> DeletedLines;

            /// <summary>
            /// [LiveMode] Список новых кефов
            /// </summary>
            List<Kef> NewKefs;

            /// <summary>
            /// [LiveMode] Обновлённые кефы
            /// При блокировке кефов их нужно вносить сюда и указывать отрицательное значение Kef.value
            /// </summary>
            List<Kef> UpdatedKefs;

            /// <summary>
            /// [LiveMode] Удалённые кефы
            /// </summary>
            List<Kef> DeletedKefs;

        }
        
        /// <summary>
        /// Одна линия [т.е. группа коэффициентов]
        /// </summary>
        public class Line
        {

            /// <summary>
            /// Уникальный ID линии на сайте источнике
            /// </summary>
            string ID;

            /// <summary>
            /// Имя линии
            /// Если линия имеет несколько уровней названий, то объединить их в одну строку, разделяя ". "
            /// (подробнее см. в README.md)
            /// </summary>
            string name;

            /// <summary>
            /// [FullMode] Список кефов этой линии
            /// </summary>
            List<Kef> kefs;

        }

        /// <summary>
        /// Один коэффициент
        /// </summary>
        public class Kef
        {

            /// <summary>
            /// Уникальный ID линии (Line.ID) сайта-источника, к которой кеф принадлежит.
            /// </summary>
            string LineID;

            /// <summary>
            /// Уникальный ID конкретно этого кефа в этом матче на сайте-источнике
            /// </summary>
            string ID;

            /// <summary>
            /// Название кефа
            /// [Не требуется при обновлении кефов в LiveMode]
            /// </summary>
            string name;

            /// <summary>
            /// Значение кефа
            /// Если кеф заблокирован, то отрицательное значение
            /// </summary>
            double value;

        }

    }

    /// <summary>
    /// Режим работы парсера
    /// 
    /// Есть два формата работы букмекеров с лайв-данными: 
    /// Одни просто перерисовывают всю страницу и каждый раз отсылают цельный HTML или цельный JSON, 
    /// который надо заново распарсить. 
    /// И есть те, кто высылает только изменения: кэф добавлен, изменён, удалён, заблокирован 
    /// с момента последнего запроса.
    /// </summary>
    public enum ParserModes : byte
    {

        /// <summary>
        /// Парсер в этом запросе получил страницу целиком и распарсил абсолютно все данные, а не их часть
        /// </summary>
        FullMode = 1,

        /// <summary>
        /// Парсер получил только кусочек данных и сказал что с ними надо сделать: добавить\изменить\удалить\заблокировать
        /// </summary>
        LiveMode = 2

    }
    
}
