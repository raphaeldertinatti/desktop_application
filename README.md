# Desktop Application with C#

Development of a desktop application using C# (CSharp) and Windows Forms. 

![badge1](https://img.shields.io/badge/Language-C%23-brightgreen)
![badge1](https://img.shields.io/badge/UI%20Framework-WinForms-blue)
![badge3](https://img.shields.io/badge/status-development-red)

## Objective
This repository will contain multiple scripts in C#/Winforms that are usefull for develop desktop applications in general. The application is not for a specific kind of company or operation, but is about a set of scripts to serve different purposes such as a login system, registrations, CRUD, data import, among others.

## 1. Login Screen
<div style="text-align:center;">
<img src="https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Login_Screen.png" alt="Image">
</div>

> ## program.cs

First, in `program.cs` the application starts by running the `LoginScreen` Form, and **IF** the variable *LoginSucess* become **"true"** the application runs the Main Form MDI `(Frm_Main)`.

```
static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginScreen Login = new LoginScreen();
            Application.Run(Login);

            if (Login.LoginSucess == true)
            {               
                Application.Run(new Frm_Main());
            } 
```

[code: program.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Program.cs)
> ## cls_mysql_conn.cs

To verify the user and pass we must connect to the database, i'm using MySQL database, so i have created a class called `cls_mysql_conn` that will contain the methods for connect and send commands to MySQL database.
The methods are:
- public bool OpenConnection()
- public bool CloseConnection()
- public MySqlCommand CreateCommand(string query, params MySqlParameter[] parameters)
- private bool IsConnectionOpen() - *check if the connection is already open*

The constructor of the class contains all the needed information for the connection:

```
  public class cls_mysql_conn
    {
        private MySqlConnection conn;
        private string server;        
        private string user;
        private string pass;

        public cls_mysql_conn()
        {
            server = "00.0.000.000";
            user = "system_user";
            pass = "@aeiou0011*ABC";

            string connectionString = $"datasource={server};username={user};password={pass};";

            conn = new MySqlConnection(connectionString);
        }
```

[code: cls_mysql_conn.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Classes/cls_mysql_conn.cs)

> ## LoginScreen.cs
Here in the login screen i have a method inside the click button event, the method `btn_enter_Click` will connect to the database using the methods of the `cls_mysql_conn` class to verify that the **user** and **password** entered in the textbox really exists in the database (previously registered). 

if it is a new user, he will have a password called *default*, if the user found in the database has this *default* password, the user will be redirected to a new password registration form, if it is not the default pass, the *LoginSucess* variable will be set true, and the user will be redirected to the main form, as you can see in the code below:

```
MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    if (txt_pass.Text == "default")
                    {
                        Frm_NewPass newpass = new Frm_NewPass();
                        newpass.Show();
                    }
                    if (txt_pass.Text != "default")
                    {
                        LoginSucess = true;
                        this.Close();
                    }
                }
                else
                {                       
                    MessageBox.Show("User/Pass incorrect, verify your credentials", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                connection.CloseConnection();
```

[code: LoginScreen.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/LoginScreen.cs)

> ## Frm_NewPass.cs
<div style="text-align:center;">
<img src="https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/new_pass.png" alt="Image">
</div>

This is a "first-time-log-in-form", a method inside the click button event changes the *default* password for a new one of user preference, checking if the password is null and if the new password textbox and confirmation  textbox are the same.

[code: Frm_NewPass.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_NewPass.cs)

## 2. Main Form (MDI)

The main form MDI contains a toolstrip menu, being the parent form that contains multiple child forms within, the child forms that currently exists are:

- Frm_Customers.
    - Frm_CustomersList.
- Frm_Companies.
    - Frm_CompaniesList.
    - Frm_CompaniesXCustomers.
- Frm_Contacts.
- Frm_Departments.
- Frm_Services.
- Frm_Taxes.
- Frm_Search_Doc.
- Frm_Search_Products.

[code: Frm_Main.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Main.cs)

## 3. Class: cls_populate_views

Before we move on to the Forms, I think it's important to talk about the  `cls_populate_views` class, it's a class that was created to populate the ListViews of the various forms with information taken from the database. This is a way to optimize the code by centralizing the operation of the **DataReaders** in a single method, avoiding code repetition.

Just call the method that receives as parameters the List View, the SQL command, and the indexes of the columns that will be populated.
```
 public void PopulateListViews(ListView listview, MySqlCommand cmd, int[] columnIndexes)
 ```
For more details access the full code.

[code: cls_populate_views.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Classes/cls_populate_views.cs)

## 4. Frm_Customers

![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Customers.png)

> ### Listviews
This is the Customers form, which has three ListViews:

- **Contacts:** individuals registered as contacts of the customer, with name, phone number, position, and email.
- **Companies:** the different companies registered that the customer owns.
- **Services:** the list of services provided to that customer, such as filtering by department.

> ### Buttons
In this form, there are four buttons, which are:

- **tsb_search_Click:** This is the button to search for customers in the registry. When clicked, it will open the `Frm_CustomersList` form, returning a list of registered customers. I will talk about this form in the next topic.
- **tsb_add_Click:** This button adds the customer to the database based on the data filled in the form.
- **tsb_save_Click:** This button updates the customer's data. It will save the changes made to the customer's registry in the database.
- **tsb_clean_Click:** This button will clear all fields in the form.

These are almost the complete CRUD operations, except for the delete operation, which, in this case, is not allowed for customers to be deleted by the application.

> ### Methods
The main method of this form is `public void Capture()`. This method will be called in the customer list form `Frm_CustomersList`. When the user selects the desired customer, this method will be called, capturing the code of this customer and bringing it to the `Frm_Customers` form, populating the textboxes with the customer's data and calling three more methods:

- ListContacts();
- ListCompanies();
- ListServices();

These methods will populate the three ListViews that exists in this Form.

[code: Frm_Customers.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Customers.cs)

## 4.1 Frm_CustomersList
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_CustomersList.png)

This form is opened when the user clicks on the `tsb_search_Click` button of the `Frm_Customers` form, returning with a list of customers from the database.

> ### Listviews
- **lsv_customers**: This ListView returns all customers registered in the database with the information of code, name, and status.

> ### Buttons
- **btn_search_Click:** This button searches for clients using filters for Name and Status, populating the listview.

- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListCustomers():** Method called in the constructor of the form, populates the listview with the list of clients.
- **lsv_customers_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod.Text` variable instantiated in the Frm_Customers form.
- **Frm_CustomersList_FormClosed():** when this form is closed the method **Capture()** of the `Frm_Customer` is called.

[code: Frm_CustomersList.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_CustomersList.cs)

## 5. Frm_Companies
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Companies.png)

This is the form for querying, modifying or including companies, where each company is associated with a customer, and a customer can have multiple companies, such as the parent company and all its subsidiaries.

The structure of this form is very similar to that of customers, in fact, most of these registration forms are similar.

> ### Buttons
- **tsb_search_Click_1:** This button search for companies in the registry. When clicked, it will open the `Frm_CompaniesList` form, returning a list of registered companies.
- **tsb_add_Click:** This button adds the companie to the database based on the data filled in the form, to include a company, it is necessary to have previously selected a customer to which the company belongs through the button `btn_associa_Click`.
- **tsb_save_Click:** This button updates the company data. It will save the changes made to the company registry in the database.
- **tsb_clean_Click:** This button will clear all fields in the form.
- **btn_associa_Click:** This button will open the `Frm_CompaniesXCustomers` form with a list of customers so that a company can be associated with its respective customer.

> ### Methods
- **Capture():** This method will be called in the companies list form `Frm_CompaniesList`. When the user selects the desired company, this method will be called, capturing the code of this company and bringing it to the `Frm_Companies` form, populating the textboxes with the company data and calling one more method (below):
- **CaptureCBB():** This method will check the state of the selected company and compare it with the character set of the `cbb_state` combobox. Upon finding the corresponding state, it will set the select index of the combobox to its respective state. The same is done for the `cbb_matriz` combobox, selecting whether the company is the parent company or a subsidiary.
- **CaptureCodCustomer():** This method serves only to bring the selected customer code from the `Frm_CompaniesXCustomers` form to the *txt_Cliente.Text* field, following the same logic as the *Capture()* method.

[code: Frm_Companies.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Companies.cs)

## 5.1 Frm_CompaniesList
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_CompaniesList.png)

This form is opened when the user clicks on the `tsb_search_Click_1` button of the `Frm_Companies` form, returning with a list of companies from the database.

> ### Listviews
- **lsv_companies**: This ListView returns all companies registered in the database with their information.

> ### Buttons
- **btn_search_Click:** This button searches for companies using filters for Name and Status, populating the listview.
- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListCustomers():** Method called in the constructor of the form, populates the listview with the list of companies.
- **lsv_companies_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod.Text` variable instantiated in the Frm_Companies form.
- **Frm_CompaniesList_FormClosed():** when this form is closed the method **Capture()** of the `Frm_Customer` is called.

[code: Frm_CompaniesList.cs]()