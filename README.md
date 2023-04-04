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

