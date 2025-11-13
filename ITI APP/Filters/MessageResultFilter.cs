
namespace ITI_APP.Filters
{
    public class MessageResultFilter : Attribute, IResultFilter
    {
        private readonly string _message;

        public MessageResultFilter(string message)
        {
            _message = message;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

            if (context.Result is ViewResult viewResult)
            {
                viewResult.ViewData["UserMessage"] = _message;
            }

        }
    }
}