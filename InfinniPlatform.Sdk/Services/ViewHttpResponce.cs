﻿namespace InfinniPlatform.Sdk.Services
{
    public class ViewHttpResponce : HttpResponse
    {
        public ViewHttpResponce(string viewName, object model = null)
        {
            ViewName = viewName;
            Model = model;
        }

        public string ViewName { get; set; }

        public object Model { get; set; }
    }
}