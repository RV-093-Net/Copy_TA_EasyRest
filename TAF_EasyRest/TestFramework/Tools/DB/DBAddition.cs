﻿namespace TestFramework.Tools.DB
{
    public class DBAddition
    {

        public static void AddRestaurantViaDB(string name, string address = "3807 Brook Street, Huston, TX 77030, USA", int ownerId = 1, int status = 0)
        {
            string addRestaurant = $"INSERT INTO restaurants (name, address_id, owner_id, status) VALUES ('{name}', '{address}', {ownerId}, {status})";
            DBConnectionWrapper.ExecuteQuery(addRestaurant);
        }

        public static void AddUserViaDB(string name, string email, int role_id, bool is_active)
        {
            string addUser = $"INSERT INTO users (name, email, role_id, is_active) VALUES ('{name}', '{email}', {role_id}, {is_active})";
            DBConnectionWrapper.ExecuteQuery(addUser);
        }

        public static void AddWaiterViaDB(string name, string email, string restaurantName, string phoneNumber = "1234567890", string password = "1111")
        {
            string addWaiter = $"INSERT INTO users (name, email, phone_number, password, role_id, restaurant_id, is_active) SELECT '{name}', '{email}', '{phoneNumber}', '{password}', 6, id, true from restaurants where name = '{restaurantName}'";
            DBConnectionWrapper.ExecuteQuery(addWaiter);
        }
    }
}
