using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradingSystem.Business.UserManagement;
using TradingSystem.Business.Market;
using System.Threading;
using System.Threading.Tasks;

namespace TradingSystemTests.IntegrationTests.ConcurrentTests
{
    [TestClass]
    public class UserManagementConcurrentTests
    {
        public UserManagementConcurrentTests()
        {
          
        }
        private string signupFirstUser()
        {
            return UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733");
        }
        private bool deleteFirstUser()
        {
            return UserManagement.Instance.DeleteUser("inbi2001");
        }

        private bool delete(string username)
        {
            MemberState m;
            MarketUsers.Instance.logout(username);
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.MemberStates.TryRemove(username, out m);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }
        private bool delete2(string username)
        {
            MemberState m;
            MarketUsers.Instance.RemoveGuest(username);
            MarketUsers.Instance.MemberStates.TryRemove(username, out m);
            return UserManagement.Instance.DeleteUser("inbi2001");
        }

        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentSignup()
        {
            String val1 = "";
            String val2 = "";
            Task task1 = Task.Factory.StartNew(() => val1 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
            Task task2 = Task.Factory.StartNew(() => val2 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
            Task.WaitAll(task1, task2);
            bool check1 = String.Equals("success", val1);
            bool check2 = String.Equals("success", val2);

            Assert.IsTrue(check1 || check2);
            Assert.IsFalse(check1 && check2);
            delete2("inbi2001");


        }

        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentSignupLoop()
        {
            for(int i = 0; i<1000; i++)
            {
                String val1 = "";
                String val2 = "";
                String val3 = "";
                Task task1 = Task.Factory.StartNew(() => val1 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
                Task task2 = Task.Factory.StartNew(() => val2 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
                Task task3 = Task.Factory.StartNew(() => val3 = UserManagement.Instance.SignUp("inbi2001", "123456", new Address("lala", "lala", "lala", "la"), "0501234733"));
                Task.WaitAll(task1, task2, task3);
              
                bool check1 = String.Equals("success", val1);
                bool check2 = String.Equals("success", val2);
                bool check3 = String.Equals("success", val3);

                if (check1 == true)
                    Console.WriteLine("first");

                else if (check2 == true)
                    Console.WriteLine("second");

                else if (check3 == true)
                    Console.WriteLine("third");

                Assert.IsTrue(check1 || check2 || check3);
                Assert.IsFalse(check1 && check2 && check3);
                delete2("inbi2001");
            }
        }

        /// test for function :<see cref="TradingSystem.Business.UserManagement.UserManagement.LogIn(string, string)"/>
        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentLoginSuccess()
        {
            signupFirstUser();
            String val1 = "";
            String val2 = "";
            DataUser d;
            String guestId = MarketUsers.Instance.AddGuest();
            Task task1 = Task.Factory.StartNew(() => val1 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
            Task task2 = Task.Factory.StartNew(() => val2 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
            Task.WaitAll(task1, task2);


            //Console.WriteLine("hehe: " + val1);
            //Console.WriteLine("haha: " + val2);
            bool check1 = String.Equals(val1, "success");
            bool check2 = String.Equals(val2, "success");
            Assert.IsTrue(check1 || check2);
            Assert.IsFalse(check1 && check2);
            // UserManagement.Instance.DataUsers.TryGetValue("inbi2001", out d);
            // Assert.IsTrue(d.IsLoggedin);
            delete("inbi2001");

        }

        
        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentLoginSuccessLooped()
        {

            for (int i = 0; i < 1000; i++)
            {
                signupFirstUser();
                String val1 = "";
                String val2 = "";
                DataUser d;
                String guestId = MarketUsers.Instance.AddGuest();

                Task task1 = Task.Factory.StartNew(() => val1 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task task2 = Task.Factory.StartNew(() => val2 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task.WaitAll(task1, task2);

                if (val1.Equals("success"))
                    Console.WriteLine("first");

                if (val2.Equals("success"))
                    Console.WriteLine("second");

                bool check1 = String.Equals(val1, "success");
                bool check2 = String.Equals(val2, "success");
                Assert.IsTrue(check1 || check2);
                Assert.IsFalse(check1 && check2);
                delete("inbi2001");
            }
        }

        /*
        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentLoginSuccessLooped2()
        {

            for (int i = 0; i < 1000; i++)
            {
                signupFirstUser();
                String val1 = "";
                String val2 = "";
                DataUser d;
                Thread thread1 = new Thread(() => {
                    String guestId = MarketUsers.Instance.AddGuest();
                    if (val2 != "")
                        Console.WriteLine("second");
                    val1 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId);
                    //Console.WriteLine("1111");
                });
                Thread thread2 = new Thread(() => {
                    String guestId = MarketUsers.Instance.AddGuest();
                    if (val1 != "")
                        Console.WriteLine("first");
                    val2 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId);


                });
                thread1.Start();
                thread2.Start();
                thread1.Join();
                thread2.Join();
                //Console.WriteLine("hehe: " + val1);
                //Console.WriteLine("haha: " + val2);
                bool check1 = String.Equals(val1, "success");
                bool check2 = String.Equals(val2, "success");
                Assert.IsTrue(check1 || check2);
                Assert.IsFalse(check1 && check2);
                delete("inbi2001");
            }


        }

        */

        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentAtLeastOneLoginSuccess()
        {
            for(int i = 0; i< 1000; i++)
            {
                signupFirstUser();
                String val1 = "";
                String val2 = "";
                DataUser d;
                String guestId = MarketUsers.Instance.AddGuest();
                Task task1 = Task.Factory.StartNew(() => val1 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task task2 = Task.Factory.StartNew(() => val2 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task.WaitAll(task1, task2);
                bool check1 = String.Equals(val1, "success");
                bool check2 = String.Equals(val2, "success");
                bool or = check1 || check2;
                Assert.IsTrue(or);
                delete("inbi2001");
            }
        }

        [TestMethod]
        [TestCategory("uc2")]
        public void TestConcurrentLessThanTwoLoginSuccess()
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 1000; i++)
            {
                signupFirstUser();
                String val1 = "";
                String val2 = "";
                DataUser d;
                String guestId = MarketUsers.Instance.AddGuest();
                Task task1 = Task.Factory.StartNew(() => val1 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task task2 = Task.Factory.StartNew(() => val2 = MarketUsers.Instance.AddMember("inbi2001", "123456", guestId));
                Task.WaitAll(task1, task2);


                bool check1 = String.Equals(val1, "success");
                bool check2 = String.Equals(val2, "success");
                bool and = check1 && check2;

                if (and == false)
                    Console.WriteLine("Ok");
                else
                    Console.WriteLine("bad");
           
                Assert.IsFalse(and);
                delete("inbi2001");
            }
        }
    }
}
