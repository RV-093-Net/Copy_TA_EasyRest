﻿using TestFramework.PageComponents;
using TestFramework.Tools.DB;

namespace TestFramework.Test
{
    [TestFixture]
    public class OwnerTests : BaseTest
    {
        public IWebDriver driver { get; private set; }
        public string UserEmail { get; private set; }
        public string RestaurantName { get; private set; }


        [OneTimeSetUp]
        public void BeforeOwnersTests()
        {
            driver = new ChromeDriver();
            userEmail = GetRoleCredentials.GetCredentials("Owner").Email;
            userPassword = GetRoleCredentials.GetCredentials("Owner").Password;
            UserEmail = "test@test.com";
            RestaurantName = "A Test Restaurant";
            DBAddition.AddRestaurantViaDB(RestaurantName);
            UserLogin(driver, userEmail, userPassword);
        }

        [Category("Smoke")]
        [Category("Positive")]
        [Test]
        public void RemoveWaiterTest()
        {
            //Arrange
            var waiter = new
            {
                Email = UserEmail,
                Name = "Test Waiter"
            };
            DBAddition.AddWaiterViaDB(waiter.Name, waiter.Email, RestaurantName);
            OwnerPanelRestaurantsPage ownerPanelRestaurantsPage = new OwnerPanelRestaurantsPage(driver);

            ownerPanelRestaurantsPage.GoToUrl();

            //Act
            var restaurant = ownerPanelRestaurantsPage.RestaurantItems.Where(r => r.Name == RestaurantName).FirstOrDefault();
            OwnerEditRestaurantPage editRestaurantPage = restaurant
                .ClickThreeDotButton(5)
                .ClickManageButton(5);

            ManageWaitersPage manageWaitersPage = editRestaurantPage.ManageRestaurantPageComponent.ClickWaitersButton();
            int expected = manageWaitersPage.WaiterItems.Count - 1;
            manageWaitersPage.WaiterItems[0].ClickRemoveButton(3);
            int actual = manageWaitersPage.WaiterItems.Count;

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Category("Smoke")]
        [Category("Positive")]
        [Test]
        public void AddAdministratorTest()
        {
            //Arrange
            OwnerPanelRestaurantsPage ownerPanelRestaurantsPage = new OwnerPanelRestaurantsPage(driver);
            ownerPanelRestaurantsPage.GoToUrl();
            var expected = new
            {
                Name = "Test Admin",
                Email = UserEmail,
                Password = "12345678",
                PhoneNumber = "0987654321"
            };

            //Act
            var restaurant = ownerPanelRestaurantsPage.RestaurantItems.Where(r => r.Name == RestaurantName).FirstOrDefault();
            OwnerEditRestaurantPage editRestaurantPage = restaurant
                .ClickThreeDotButton(5)
                .ClickManageButton(5);

            CreateNewAdministratorPageComponent createAdminComponent = editRestaurantPage.ManageRestaurantPageComponent
                .ClickAdministratorsButton(3)
                .ClickAddAdministratorButton(3);

            ManageAdministratorPage manageAdminPage = createAdminComponent.SendKeysToFields(expected.Name, expected.Email, expected.Password, expected.PhoneNumber)
                .ClickAddButton(3);

            var actual = manageAdminPage.AdministratorItem;

            //Assert
            Assert.That(actual?.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Contacts, Is.EqualTo($"{expected.PhoneNumber} / {expected.Email}"));
        }

        [Category("Smoke")]
        [Category("Positive")]
        [Test]
        public void AddWaiterTest()
        {
            //Arrange
            OwnerPanelRestaurantsPage ownerPanelRestaurantsPage = new OwnerPanelRestaurantsPage(driver);
            ownerPanelRestaurantsPage.GoToUrl();
            var expected = new
            {
                Name = "Test Waiter1",
                Email = UserEmail,
                Password = "11111111",
                PhoneNumber = "0981111111"
            };

            //Act
            var restaurant = ownerPanelRestaurantsPage.RestaurantItems.Where(r => r.Name == RestaurantName).FirstOrDefault();
            OwnerEditRestaurantPage editRestaurantPage = restaurant
                .ClickThreeDotButton(5)
                .ClickManageButton(5);
            ManageWaitersPage manageWaitersPage = editRestaurantPage.ManageRestaurantPageComponent.ClickWaitersButton();

            int expect = manageWaitersPage.WaiterItems.Count + 1;

            CreateNewWaiterPageComponent createWaiterComponent = manageWaitersPage.ClickAddWaiterButton();

            ManageWaitersPage manageWaiterPage = createWaiterComponent.SendKeysToFields(expected.Name, expected.Email, expected.Password, expected.PhoneNumber)
                .ClickAddButton();

            int actual = manageWaiterPage.WaiterItems.Count;

            //Assert
            Assert.That(actual, Is.EqualTo(expect));
        }

        [TearDown]
        public void AfterEachTest()
        {
            DBCleanup.UnlinkAdministratorFromRestaurant(UserEmail);
            DBCleanup.DeleteUserByEmail(UserEmail);
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            DBCleanup.DeleteRestaurantByName(RestaurantName);
            driver.Quit();
        }
    }
}