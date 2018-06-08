using Better.Bookmakers.Types;
using System;
using System.Threading.Tasks;

/// <summary>
/// * Букмекер [ИМЯ]
/// </summary>
namespace Better.Bookmakers
{

    /// <summary>
    /// * Парсер букмекера [ИМЯ]
    /// </summary>
    public class NewBookmaker: ParserInterface
    {

        /// <summary>
        /// * ID букмекера
        /// </summary>
        public int BookmakerID { get; set; } = 1;
        
        /// <summary>
        /// * Кодировка страницы матчей
        /// 65001 = utf8; 1251 = win-1251; 20866 = KOI8-R;
        /// </summary>
        public int Encoding_Matches { get; set; } = 65001;

        /// <summary>
        /// * Кодировка страницы одного матча
        /// </summary>
        public int Encoding_OneMatch { get; set; } = 65001;

        /// <summary>
        /// Инициализация при первичной подгрузке парсера
        /// </summary>
        /// <returns>Возвращает результат инициализации, т.е. готовность парсера к работе</returns>
        public bool init()
        {
            return true;
        }

        /// <summary>
        /// Точка входа в библиотеку. Отделяет полезные запросы от бесполезных и направляет каждый на свои парсеры
        /// </summary>
        /// <param name="page_type">Тип страницы: список матчей или страница кефа. Если страницы кефов нет - по умолчанию страница матчей</param>
        /// <param name="request_type">Тип запроса: обычный или фрейм WebSocket`а</param>
        /// <param name="request">Инфо с обычного запроса, либо NULL</param>
        /// <param name="wsFrame">Инфо о фрейме WebSocket`а, либо NULL</param>
        /// <returns>Вернуть распаршенную структуру Parsed или NULL, если запросу парсинг не требуется</returns>
        public Parsed Router(pageTypes page_type, requestTypes request_type, ref NormalRequest request, ref WebSocketRequest wsFrame) {

            // По request.request_url (если надо request_headers) направить на выполнение

            // Вернуть Parsed или null, если парсинг не нужен
            return null;
        }

        /// <summary>
        /// Запустить парсер нужной страницы, отправляя запросы вручную
        /// Предпочтение отдавать асинхронным запросам и сохранению куков через WebClientCookie
        /// </summary>
        /// <param name="urlMatches">Ссылка на страницу списка матчей, если надо запустить её. Иначе - пустая строка</param>
        /// <param name="urlOneMatch">Ссылка на страницу конкретного матча, если надо запустить парсинг кефов. Иначе - пустая строка.</param>
        /// <param name="speed_kef">Коэффициент ускорения отправки запросов</param>
        /// <param name="callback">Коллбек, который надо вызвать со структурой Parsed, которую вернёт роутер, если нужно</param>
        public async Task Start(string urlMatches, string urlOneMatch, double speed_kef = 1, Action<Parsed> callback = null)
        {

            // WebClientCookie - расширенная версия вебклиента, которая сама хранит сессию, подставляет User-agent
            // и другие стандартные заголовки
            // System.Net.CookieContainer GlobalCookieContainer = new System.Net.CookieContainer();
            // WebClientCookie WC = new WebClientCookie(GlobalCookieContainer);


            // "Разогреть" страницу и получить основные куки букмекера
            // WC.Headers.Add("Referer", "");
            // await WC.DownloadStringTaskAsync(urlMatches);

            // получить одноразовые данные, если нужно

            // запустить цикл обновлений
            // NormalRequest request = new NormalRequest();WebSocketRequest noRequest = null;
            // while (true) {

            // отправить запросы асинхронно WC.DownloadStringTaskAsync
            // создать экземпляр NormalRequest, прописав туда 
            // request_url и response_data, и если нужно для парса - response_headers
            // отправить NormalRequest в роутер
            // Parsed data = Router(pageTypes.MatchesList, requestTypes.Normal, ref request, ref noRequest);
            // Вызвать коллбек callback.DynamicInvoke(data), если он есть
            // Подождать стандартное для букмекера время обновления с учётом коэффициента ускорения
            // await Task.Delay(speed_kef*5000)
            // }

        }

        #region "Контроль ошибок"

        /// <summary>
        /// Отправить ошибку парса в основную прогу немедленно [асинхронно]
        /// [реализовано через callback, чтобы редкие виды ошибок не приводили к неотправке самого списка ошибок]
        /// </summary>
        /// <param name="name">Название\описание ошибки</param>
        /// <param name="path">Путь до ошибки. Реализовано в виде строки можно было тонко описать момент её возникновения</param>
        /// <param name="data">Любые прикладные данные этой ошибки, которые вам нужно будет знать для её исправления. Будет сериализовано в json и записано.</param>
        public void sendException(string name , string path = "", object data=null) {
            if (ExceptionCallback != null)
            {
                ExceptionCallback.BeginInvoke(name, path, data, (iResult) => 
                {
                    ExceptionCallback.EndInvoke(iResult);
                },
                null);
            }
        }

        /// <summary>
        /// Коллбек для ловли ошибок в реалтайме в основной проге
        /// [присоединяется родительской программой]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public delegate void ExceptionCallbackDelegate(string name, string path = "", object data = null);
        public ExceptionCallbackDelegate ExceptionCallback;

        #endregion

        #region "Обратные запросы"
        
        // По-умолчанию Router лишь получает страницы и обрабатывает их
        // Тут можно отправить запрос из самого парсера, а не функции Start или управляющей программы.
        
        // Пока реализована как заготовка, если будет нужно - пишите.

        /// <summary>
        /// Коллбек: отправить обычный запрос
        /// </summary>
        /// <returns></returns>
        public delegate NormalRequest RequestCallback();

        /// <summary>
        /// Коллбек: отправить WS фрейм
        /// </summary>
        /// <returns></returns>
        public delegate WebSocketRequest WebSocketRequestCallback();

        #endregion
    }
}
