﻿using TestFramework.PageComponents;
using TestFramework.PageComponents.NavigationMenuComponents;

namespace TestFramework.Pages
{
    public class OrderHistoryPage : BasePage
    {

        public NavigationMenuPageComponent NavigationMenuPageComponent { get; }
        public PersonalInfoPageComponent PersonalInfoPageComponent { get; }
        public List<OrderPageComponent> orders { get; }

        public OrderHistoryPage(IWebDriver driver): base(driver)
        {
            NavigationMenuPageComponent = new NavigationMenuPageComponent(driver);
            PersonalInfoPageComponent = new PersonalInfoPageComponent(driver);
            FillOdersList(orders);
        }

        #region Elements

        private IWebElement _allButton => driver.FindElement(By.XPath("//span[contains(text(),'All')]/ancestor::a"));
        private IWebElement _historyButton => driver.FindElement(By.XPath("//span[contains(text(),'History (')]/ancestor::a"));
        private IWebElement _declinedButton => driver.FindElement(By.XPath("//span[contains(text(),'Declined')]/ancestor::a"));
        private IWebElement _orderField => driver.FindElement(By.XPath("(//div[@class='UserOrders-root-1340']/child::div)[1]"));

        #endregion

        #region Methods

        private int countOrders()
        {
            IReadOnlyCollection<IWebElement> items = driver.FindElements(By.XPath(""));
            return items.Count();
        }

        private void FillOdersList(List<OrderPageComponent> orders)
        {
            orders = new List<OrderPageComponent>(countOrders());
            for (int i = 0; i < orders.Count; i++)
            {
                orders.Add(new OrderPageComponent(driver, (i+1)));
            }
        }

        public OrderHistoryPage ClickAllButton()
        {
            _allButton.Click();
            return this;
        }

        public OrderHistoryPage ClickHistoryButton()
        {
            _historyButton.Click();
            return this;
        }

        public OrderHistoryPage ClickDeclineButton()
        {
            _declinedButton.Click();
            return this;
        }

        public OrderPageComponent ExpandOrderField()
        {
            _orderField.Click();
            return new OrderPageComponent(driver);
        }

        #endregion
    }
}
