using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;


namespace Chrome_Driver_Launch
{

	public static class GlobalVariables
	{
		public static Dictionary<string, string> my_data;
		public static Driver driver;
		public static string pay_grade_id;
		public static string path_to_driver = "/Users/donevd/Downloads/driver";
		public static string url = "https://opensource-demo.orangehrmlive.com/index.php/auth/login";


		public static void InitialiseSecret()
		{
			my_data = new Dictionary<string, string>();
			my_data.Add("login", "Admin");
			my_data.Add("password", "admin123");
			my_data.Add("payGrade_name", "DDonev");
			my_data.Add("currencyName", "AUD - Australian Dollar");
			my_data.Add("minSalary", "100.00");
			my_data.Add("maxSalary", "200.00");

		}

		public static void InitialiseDriver()
		{
			driver = new Driver(path_to_driver, url, my_data);
		}
	}

	public class Driver : ChromeDriver
	{

		String login;
		String password;
		String payGrade_name;
		String currencyName;
		String minSalary;
		String maxSalary;


		public Driver(string path, string url, Dictionary<string, string> my_data) : base(path)
		{
			this.Url = url;
			this.login = my_data["login"];
			this.password = my_data["password"];
			this.payGrade_name = my_data["payGrade_name"];
			this.currencyName = my_data["currencyName"];
			this.minSalary = my_data["minSalary"];
			this.maxSalary = my_data["maxSalary"];
		}

		public string Exception(string module, string span = ".//span[@class = \"validation-error\"]")
		{
			string answer, error;
			try
			{
				error = this.FindElement(By.XPath(span)).Text;
			}
			catch (NoSuchElementException)
			{
				error = "";
			}


			if (error == "")
			{
				answer = "SUCCESS";
			}
			else
			{
				answer = "ERROR";
			}
			Console.Write($"{answer} {error}, in {module} \n");
			return answer;
		}


		public string LogIn()
		{
			this.FindElement(By.Name("txtUsername")).SendKeys(this.login);
			this.FindElement(By.Name("txtPassword")).SendKeys(this.password);
			this.FindElement(By.Name("Submit")).Click();
			string answer = this.Exception("login", ".//span[@id=\"spanMessage\"]");
			return answer;

		}
		public void ToGradeSection()
		{
			this.FindElement(By.Id("menu_admin_viewAdminModule")).Click();
			this.FindElement(By.Id("menu_admin_Job")).Click();
			this.FindElement(By.Id("menu_admin_viewPayGrades")).Click();
		}
		public string AddGrade()
		{
			this.ToGradeSection();
			this.FindElement(By.Name("btnAdd")).Click();
			this.FindElement(By.Id("payGrade_name")).SendKeys(this.payGrade_name);
			this.FindElement(By.Name("btnSave")).Click();
			string answer = this.Exception("AddUsers");

			return answer;
		}
		public string AddCurr()
		{
			string answer;
			this.FindElement(By.Id("btnAddCurrency")).Click();
			this.FindElement(By.Name("payGradeCurrency[currencyName]")).SendKeys(this.currencyName + Keys.Return );
			this.FindElement(By.Name("payGradeCurrency[minSalary]")).SendKeys(this.minSalary + Keys.Return);
			string is_pass_curr_add = this.Exception("currencyName");
			string is_pass_min_sal_add = this.Exception("minSalary");
			this.FindElement(By.Name("payGradeCurrency[maxSalary]")).SendKeys(this.maxSalary + Keys.Return);
			string is_pass_max_sal_add = this.Exception("maxSalary");
			this.FindElement(By.Name("btnSaveCurrency")).Click();
			
			if (is_pass_curr_add == "SUCCESS" && is_pass_min_sal_add == "SUCCESS" && is_pass_max_sal_add == "SUCCESS")
			{
				answer = "SUCCESS";
			}
			else
            {
				answer = "ERROR";
			}

			return answer;
		}

 
		public string Delete()
		{
			int id = Int32.Parse(GlobalVariables.pay_grade_id);
			string answer;
			this.ToGradeSection();
			this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr/td/input[@type='checkbox' and @value={id}]")).Click();
			this.FindElement(By.Name("btnDelete")).Click();
			this.FindElement(By.Id("dialogDeleteBtn")).Click();
			try
			{
				this.FindElement(By.XPath($"//*[text()='{this.payGrade_name}']"));
				answer = "ERROR";
			}
			catch (NoSuchElementException)
			{
				answer = "SUCCESS";
			}
			Console.Write($"Delete: {answer}\n");
			return answer;
		}

		public string IsChangesVisible( )  
		{
			string answer, result;
			string[] separator = { "payGradeId=" };
			string min_curr_from_site = "";
			string max_curr_from_site = "";

			this.ToGradeSection();

			try
			{
				this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr/td[@class='left']/a[text()='{this.payGrade_name}']")).Click();
				GlobalVariables.pay_grade_id = this.Url.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries)[1];
				  min_curr_from_site = this.FindElement(By.XPath( "//table[@id='tblCurrencies']/tbody/tr[@class='odd']/td[3]")).Text;
				  max_curr_from_site = this.FindElement(By.XPath( "//table[@id='tblCurrencies']/tbody/tr[@class='odd']/td[4]")).Text;
				result = "SUCCESS";
			}
			catch (NoSuchElementException)
			{
				result = "ERROR";
			}

			if (result == "SUCCESS"  && max_curr_from_site == this.maxSalary && min_curr_from_site == this.minSalary)
			{
				answer = "SUCCESS";
			}
			else
			{
				answer = "ERROR";
			}

			 

			return answer;

		}

		public string IsDeleteSuccess()
		{
			string answer, result;
			this.ToGradeSection();
			try
			{
				this.FindElement(By.XPath($"//table[@id='resultTable']/tbody/tr/td[@class='left']/a[text()='{this.payGrade_name}']")).Click();

				answer = "SUCCESS";
			}
			catch (NoSuchElementException)
			{
				answer = "ERROR";
			}

		 

			return answer;

		}




	}




	[TestFixture]
	class MainClass
	{

		[Test]
		public void Test0_Connection()
		{
			GlobalVariables.InitialiseSecret();
			GlobalVariables.InitialiseDriver();
			var actualUrl = GlobalVariables.driver.Url;
			Assert.AreEqual(actualUrl, GlobalVariables.url);
		}

		[Test]
		public void Test1_LogIn()
		{
			var result = GlobalVariables.driver.LogIn();
			Assert.AreEqual(result, "SUCCESS");
		}

		[Test]
		public void Test2_1_AddGrade()
		{
			 
			var result = GlobalVariables.driver.AddGrade();
			Assert.AreEqual(result, "SUCCESS");
		}

		[Test]
		public void Test2_2_AddCurr()
		{

			var result = GlobalVariables.driver.AddCurr();
			Assert.AreEqual(result, "SUCCESS");
		}

		[Test]
		public void Test2_3_AddInfo_Check()
		{
			 
			var result = GlobalVariables.driver.IsChangesVisible( );
			Assert.AreEqual(result, "SUCCESS");
		}


		[Test]
		public void Test4_1_Delete()
		{
			 
			var result = GlobalVariables.driver.Delete();
			Assert.AreEqual(result, "SUCCESS");
		}
		[Test]
		public void Test4_2_DeleteInfo_Check()
		{
			 
			var result = GlobalVariables.driver.IsDeleteSuccess( );
			GlobalVariables.driver.Quit();
			Assert.AreEqual(result, "ERROR");
		}
	}

}
 