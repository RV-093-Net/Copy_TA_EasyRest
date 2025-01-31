﻿using TestFramework.Tools;

namespace TestFramework.PageComponents.Owner
{
    public class AdministratorItemPageComponent
    {
        private IWebElement _adminElement;
        public AdministratorItemPageComponent(IWebElement element)
        {
            _adminElement = element;
        }

        public string? Name => _adminElement.FindElementSafe(By.XPath("./div/span"))?.Text;
        public string? Contacts => _adminElement.FindElementSafe(By.XPath("./div/p/span"))?.Text;
    }
}
