namespace System.Windows.Navigation
{
    internal class RequestNavigateEventHandler
    {
        private object hyperlink_RequestNavigate;

        public RequestNavigateEventHandler(object hyperlink_RequestNavigate)
        {
            this.hyperlink_RequestNavigate = hyperlink_RequestNavigate;
        }
    }
}