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
    - Frm_ContactList.
    - Frm_ContactsXCustomers.
- Frm_Departments.
- Frm_Services.
    - Frm_DepList.
    - Frm_ServicesXCustomers.
- Frm_TaxAudit.
    - Frm_Lst_Cst_Audit.
    - Frm Lst_Cmp_Audit
    - Frm_Audit_System.
    - Frm_Audit_IRS.
    - Frm_Audit_Supplier.
    - Frm_Audit_Values.
        - Frm_Audit_Values_Detailed.
    - Frm_Audit_Cancelled.
    - Frm_Audit_Unrelated.
    - Frm_Analyses.
    - Frm_Natureza_Operacao.
    - Frm_CFOP_CST.
    - Frm_Rural_Producer.
    - Frm_Transferences.
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
- **ListCompanies():** Method called in the constructor of the form, populates the listview with the list of companies.
- **lsv_companies_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod.Text` variable instantiated in the Frm_Companies form.
- **Frm_CompaniesList_FormClosed():** when this form is closed the method **Capture()** of the `Frm_Companies` is called.

[code: Frm_CompaniesList.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_CompaniesList.cs)

## 5.2 Frm_CompaniesXCustomers
This form is opened when the user clicks on the `btn_associa_Click` button of the `Frm_Companies` form, returning with a list of customers from the database, so that the user can associate the company registration with its respective customer.

> ### Listviews
- **lsv_customers2**: This ListView returns all customers registered in the database with the information of code, name, and status.

> ### Buttons
- **btn_search_Click:** This button searches for customers using filters for Name and Status, populating the listview.
- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListCustomers():** Method called in the constructor of the form, populates the listview with the list of customers.
- **lsv_customers_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod_customer.Text` and `name_customer.Text` variables instantiated in the Frm_Companies form.

[code: Frm_CompaniesXCustomers.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_CompaniesXCustomers.cs)

## 6. Frm_Contacts
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Contacts.png)

This is the contact form where you can register contacts and associate them with their respective clients. This form allows for the full CRUD process, allowing you to create, read, update, or delete contact information.

> ### Buttons
- **tsb_search_Click:** This button search for contacts in the registry. When clicked, it will open the `Frm_ContactList` form, returning a list of registered contacts.
- **tsb_add_Click:** This button adds the contact to the database based on the data filled in the form, to include a contact, it is necessary to have previously selected a customer to which the contact belongs through the button `btn_associa_Click`.
- **tsb_save_Click:** This button updates the contact data. It will save the changes made to the contact registry in the database.
- **tsb_clear_Click:** This button will clear all fields in the form.
- **btn_associa_Click:** This button will open the `Frm_ContactsXCustomers` form with a list of customers so that a contact can be associated with its respective customer.
- **tsb_delete_Click:** This button will delete the contact record in the database.

> ### Methods
- **Capture():** This method will be called in the contact list form `Frm_ContactsList`. When the user selects the desired contact, this method will be called, capturing the code of this contact and bringing it to the `Frm_Contacts` form, populating the textboxes with the contact data.

[code: Frm_Contacts.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Contacts.cs)

## 6.1 Frm_ContactsList
This is the form that returns the list of clients, the form is opened when the user clicks on the `tsb_search_Click` button of the `Frm_Contacts` form. I won't include the image, as all list forms are practically the same.

> ### Listviews
- **lsv_contacts**: This ListView returns all contacts registered in the database with their information.

> ### Buttons
- **btn_search_Click:** This button searches for contacts using a filter for Name, populating the listview.
- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListContacts():** Method called in the constructor of the form, populates the listview with the list of contacts.
- **lsv_contacts_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod.Text` variable instantiated in the Frm_Contacts form.
- **Frm_ContactsList_FormClosed():** when this form is closed the method **Capture()** of the `Frm_Contacts` is called.

[code: Frm_ContactsList.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_ContactsList.cs)

## 6.2 Frm_ContactsXCustomers

This form is opened when the user clicks on the `btn_associa_Click` button of the `Frm_Contacts` form, returning with a list of customers from the database, so that the user can associate the contact registration with its respective customer.

> ### Listviews
- **lsv_customers2**: This ListView returns all customers registered in the database with the information of code, name, and status.

> ### Buttons
- **btn_search_Click:** This button searches for customers using filters for Name and Status, populating the listview.
- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListCustomers():** Method called in the constructor of the form, populates the listview with the list of customers.
- **lsv_customers2_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod_customer.Text` and `name_customer.Text` variables instantiated in the Frm_Contacts form.

[code: Frm_ContactsXCustomers.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_ContactsXCustomers.cs)

## 7. Frm_Departments
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Departments.png)

Form for querying and adding departments. As this is something that will not be created, deleted, or changed frequently, only query and add options are available. In the context of our application, these departments are the company's own departments. They need to exist in the database so that the services the company provides can be separated by department.

There is no component in the code structure of this form that does not already exist in the previous ones, so I will not detail it.

[code: Frm_Departments.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Departments.cs)

## 8. Frm_Services
![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Services.png)

In this form, it is possible to add and modify the services that the company provides, as well as link a specific service to a customer, meaning that the company provides a certain service to the customer. Through two Listviews, we can also see all the registered services and the services filtered by customer.

> ### Listviews
- **lsv_services**: This ListView returns all services registered in the database with their information.
- **lsv_servclient**: This ListView returns all services registered linked with the respective customer, being able to filter by customer selected.

> ### Buttons
- **tsb_clear_Click:** This button will clear all fields in the form.
- **btn_department_Click:** This button search for departments in the registry. When clicked, it will open the `Frm_DepList` form, returning a list of registered departments.
- **tsb_add_Click:** This button adds the service to the database based on the data filled in the form, it is necessary to have previously selected a department to which the service belongs through the button `btn_department_Click`.After including a service, you can select a customer by clicking on the `btn_associa_Click` button, and then link the service to one or more customers through the `btn_vincula_Click` button.
- **tsb_save_Click:** This button updates the service data. It will save the changes made to the service registry in the database.
- **btn_associa_Click:** This button will open the `Frm_ServicesXCustomers` form with a list of customers so that a contact can be associated with its respective customer.
- **btn_vincula_Click:** This button link the service with the selected customer.

> ### Methods
- **ListServices():** Method called in the constructor of the form, populates the listview with the list of all services.
- **ListServCustomers():** Method called in the constructor of the form, populates the listview with the list of all services linked to your respective customer. 
- **ListServicesDEP():** Method called inside the `Frm_DepList` when the form is closed. It will populate the `lsv_services` listview filtering by the select department.
- **ListServCustomersDEP():** Method called inside the `Frm_DepList` when the form is closed. It will populate the `lsv_servclient` listview filtering by the select customer if there is one selected.
- **lsv_services_ItemSelectionChanged():** This method maps the selected item in the listview.

[code: Frm_Services.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Services.cs)

## 8.1 Frm_DepList

This form is opened when the user clicks on the `btn_department_Click` button of the `Frm_Services` form, returning with a list of departments from the database.

> ### Listviews
- **lsv_dep**: This ListView returns all departments registered in the database with their information.

> ### Buttons

- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListDEP():** Method called in the constructor of the form, populates the listview with the list of departments.
- **lsv_dep_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod_dep.Text` and `desc_dep.Text` variables instantiated in the `Frm_Services` form.
- **Frm_CompaniesList_FormClosed():** when this form is closed the methods **ListServicesDEP()** and **ListServCustomersDEP** of the `Frm_Services`.

[code: Frm_DepList.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_DepList.cs)

## 8.2 Frm_ServicesXCustomers

This form is opened when the user clicks on the `btn_associa_Click` button of the `Frm_Services` form, returning with a list of customers from the database.

> ### Listviews
- **lsv_customers2**: This ListView returns all customers registered in the database with their information.

> ### Buttons

- **btn_select_Click:** This button simply closes the form.

> ### Methods
- **ListClients():** Method called in the constructor of the form, populates the listview with the list of customers.
- **lsv_clientes2_ItemSelectionChanged():** This method maps the selected item in the listview and passes this information to the `cod_cliente.Text` and `desc_cliente.Text` variables instantiated in the `Frm_Services` form.

[code: Frm_ServicesXCustomers.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_ServicesXCustomers.cs)

## 9. Frm_TaxAudit

![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_TaxAudit.png)

This form is the general tax conference panel. By selecting the client, company, and competence, and after importing the data source, which are .csv files related to the data of invoices issued in the system (System Button) and invoices issued and sent to the Internal Revenue Service (IRS Button), it is possible to perform various analyses and conferences by navigating through the other buttons that we will describe.

> ### Buttons
- **btn_selcli_Click:** This button search for customers in the registry. When clicked, it will open the `Frm_Lst_Cst_Audit` form, returning a list of registered customers.
- **btn_selemp_Click**:  This button search for companies in the registry. When clicked, it will open the `Lst_Cmp_Audit` form, returning a list of registered companies.
- **btn_limpar_Click**: This "reset" button clears the form, disabling all components except for the client selection button.
- **btn_system_Click**: This button opens the `Frm_Audit_System` form for file import, allowing the user to import the .csv file generated by the system with information about the issued invoices during the period.
- **btn_IRS_Click**: This button opens the `Frm_Audit_IRS` form for file import, allowing the user to import the .csv file generated by the Internal Revenue Service website with information about the issued invoices during the period.
- **btn_basesupplier_Click**: This button opens the `Frm_Conf_Fornec` form for file import, allowing the user to import the .csv file generated by the system with information about all the registered suppliers.
- **btn_values_Click**: This button opens the `Frm_Audit_Values` form that returns all invoices with value discrepancies between those issued by the system and those recorded by the IRS, with detailed tax data.
- **brn_cancelled_Click**: Opens the `Frm_Audit_Cancelled` form, returns all invoices that are marked as cancelled in the IRS but were not cancelled in the company's system.
- **btn_unrelated_Click**: This button opens the `Frm_Audit_Unrelated` form, which returns all invoices that exist in the system but for some reason are not present in the IRS database.
- **btn_analyses_Click**: This button opens the `Frm_Analyses` form that returns differences in accounting values grouped by CFOP (Fiscal Operation Code).
- **btn_nat_operacao_Click**: This button opens the `Frm_Natureza_Operacao` form that compares the Fiscal Operation Codes (CFOP) and the Operation Nature of the invoice to verify if they are compatible.
- **button1__CFOPClick**: This button opens the `Frm_CFOP_CST` form that compares the Fiscal Operation Codes (CFOP) and the CST code (tax situation code) to verify if they are compatible.
- **btn_prod_rural_Click**: This button opens the `Frm_Prod_Rural` form that performs a conference between the CGO (General Operation Code) in the system and the rural producer invoices to verify if they are compatible.
- **btn_transf_Click**: This button opens the `Frm_Transferencia` form that displays all transfer invoices issued during the period.
- **btn_save_Click**: This button is used to save the observation/annotation for that conference. If the field is empty, the `InsertObs()` method will be called, which will insert the observation into the database table. If there is already an observation and the save button is pressed, the `UpdateObs()` method will be called, which will update the field in the table.

> ### Methods

I will only mention here the methods that are truly relevant and different from those previously mentioned in other forms, which only capture a code from another form, for example. The complete code is also available for verification below.

You will see that as we select the necessary data to perform the conference, the components we previously selected are disabled and new components are enabled for selection. The method that enables all components is `cbb_ano_SelectionChangeCommitted()`, which is called when the year is selected in the final combobox, this method also call other four methods that will be explained below:

- **CheckRowsSys()**: This method checks if there is an imported database for the system's fiscal data. If there is, the method `Check_Importado()` will be called.
- **CheckRowsIRS()**: This method checks if there is an imported database for the IRS data. If there is, the method `Check_Importado()` will be called.
- **CheckRowsNSupply()**: This method checks if there is an imported database for the Supplier data. If there is, the method `Check_Importado()` will be called.
- **CapturaObs()**: Method that returns the observation that was saved in the database for that conference.
- **Check_OpenForm()**: This method creates a DataTable and fills it with data retrieved from the database through a query passed as a parameter. It tests and, if the DataTable is filled with any rows, it means that there is information to be shown, and thus the form passed in the parameter is opened.
- **Check_Importado()**: This method simply populates the specified textbox informing that there are imported data for that client in that audit period, ir there are no datam the textbox will remain blank.
- **GetSqlParameters()**: Method that contains all the MySqlParameters used in the form.

Obs: IRS Internal Revenue Service is equivalent to Receita Federal ou SEFAZ. (Brazil)

[code: Frm_TaxAudit.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_TaxAudit.cs)

## 9.1 Classes for importing .csv files.

A different class was created for each type of .csv file that will be imported, as we have the report file generated by the system, the file generated by the IRS, and the suppliers file, three different classes were created, namely:
- **cls_csv_sys**: Import of the file generated by the system [code: cls_csv_sys](https://github.com/raphaeldertinatti/desktop_application/blob/main/Classes/cls_csv_sys.cs)
- **cls_csv_irs**: Import of the file generated by IRS [code: cls_csv_irs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Classes/cls_csv_irs.cs)
- **cls_csv_supply**: Import of the suppliers file [code: csl_csv_supply](https://github.com/raphaeldertinatti/desktop_application/blob/main/Classes/cls_csv_supply.cs)

I did it this way because for each file, the fields may change order or name, so I had to use a more customized approach for each file and couldn't generalize it enough to create a single class.

The general structure of these classes is as follows:

> ### public struct Indexes

For each .csv file (class), a different struct was created, containing a data structure referring to the columns of the file. This struct will be important for us to find the indices of each column.

> ###  public static Indexes SetColumnsIndex(string[] columns)

This method receives an array of strings for the columns (headers) of the .csv file as a parameter and returns an object of type "Indexes".

The logic of the method is basically to iterate through the "columns" string array and add each element as a new instance of "Index" in a list of column indexes containing the number of each index depending the column name. The "Indexes" object is created and initialized with this list of column indexes.

> ###  public static List<cls_csv_sys> BuildConfC5(StreamReader reader, Indexes ind)

The method BuildConfC5 receives two parameters: a StreamReader and the Indexes objects. The method returns a List of objects of type cls_csv_sys.

The method read the contents of a text/csv file using the StreamReader object passed as a parameter.

The Indexes object passed as a parameter is used to determine the index of the column that contains each value in the file. The Indexes object contains a list of IndexColumn objects, where each object represents a column in the file and its corresponding index.

Once the file is read, the method uses the Indexes object to extract the values for each property of a cls_csv_sys object. A new instance of cls_csv_sys is created for each line in the file, and its properties are set based on the values extracted from the file using the Indexes object.

Finally, each instance of cls_csv_sys is added to a List of cls_csv_sys objects, which is returned as the result of the method.

In summary, the BuildConfC5 method is used to read a text/csv file, extract the data from the file using an Indexes object, and create a list of cls_csv_sys objects, this list will be used in the respective form to import the data into a table in MySQL.

## 9.2 Frm_Audit_System, Frm_Audit_IRS, Frm_Audit_Supplier.

![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Audit_System.png)

These forms are used to import the system's .csv files, IRS files, and supplier files, respectively. A label displays the date of the last file import. The DataGridView includes filters using the AGV (Advanced GridView) package. The forms are almost identical, with the same methods and buttons, except for the IRS form, which has separate imports for entries and exits.

> ### Buttons
- **btn_importar_Click:** This button opens the file selection window and reads the selected file using a StreamReader. Then, the `SetColumnsIndex()` and `BuildConfC5()` methods from the class corresponding to the import of a specific .csv file are called. After these methods organize the indices and return the list of columns with their respective values, a foreach loop is used to import the items in this list into a table in the database. Finally, a messagebox is displayed to indicate the success of the import. Before the file is imported, the `Delete()` method is called to delete all previous records from that specific company and period, so that with each new import, the new file replaces the old one.
- **btn_limpar_Click**: Clears the filters and resets the data of the DataGridViews.

> ### Methods

- **Frm_Audit_System_Load:** Invokes the `BindData()` method when the form is loaded.
- **BindData():** It selects the imported data, populates the DataGridView and calls the `CaptureUltImport()` method.
- **CaptureUltImport():** Sets the label with the date of the last import from the file.

## 9.3 Frm_Audit_Values.

![image](https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Frm_Audit_Values.png)

This form displays a general panel with all invoices issued by the client that have some tax discrepancies between what was issued by the system and what was sent to the IRS.

The Datagridview is paginated to make navigation between data lighter, considering that there are various color formats that interfere a little with performance, pagination was something that came as a solution for that.

There is also a checkbox where the user can mark the files that have already been checked.

By double-clicking on any row, another form is opened with detailed data of the document for a more thorough review.

> ### Buttons

In this form, there are only 2 buttons, **btn_Next_Click** and **btn_previous_Click**, which serve to navigate between the pages of the datagridview. The control is made by the global variables **offset**, **totPG**, and **inPG**, where offset is the variable that adds or subtracts by 26, which is the number of rows in the gridview, as the buttons are triggered. It is also an input parameter of the procedure that fills the gridview. The variable totPG receives the total number of pages from the method countRows(), and the variable inPG controls the initial page and the page number the user is currently on.

> ### Methods

- **BindData():** This method is responsible for calling the SQL procedure that performs the necessary field comparisons to return the discrepancies. This query with the tax discrepancies is then populated into the datagridview. The default colors of the columns are also defined in this method. Finally, a new checkbox column is created which serves as a control for what has already been reviewed.
- **countRows():** This method calls another SQL procedure without the LIMIT clause that limits the query results to always 26 rows. By doing so, it returns the total number of rows and divides it by 26, giving us the total number of pages, which is then stored in the global variable totPG.
- **Frm_Conf_Values_Load():** When the form is loaded, the method `BindData()` is called with the parameter offset 0, meaning that it will return the first 26 rows, populating the datagridview. The method `countRows()` is also called to capture the total number of pages in the datagridview. The method `CapturaFleg()` is called to return which files in the datagridview have already been reviewed, filling the checkbox and painting the row green. Finally, the label containing the current page/total pages is populated.
- **dgv_conf_values_CellFormatting():** This method compares certain columns of the datagridview that represent tax values. When it finds a difference, the font color is changed to red. If the values are the same, the font color remains black.
- **dgv_conf_values_CellDoubleClick():** When a row in the datagridview is double-clicked, the form `Frm_Audit_Values_Detailed` is opened and receives the values of that specific row in the gridview in its textboxes.
- **dgv_conf_values_CellValueChanged():** This method is triggered when the checkbox in the gridview is clicked, indicating that the corresponding row or invoice has been reviewed. This causes the row to turn green, and this information is automatically inserted into a table in the database. When the gridview is repopulated, we already know which invoices have been reviewed, so these are marked with a checked checkbox and a green row.
- **CapturaFleg():** This is the method that performs the above-mentioned operation of checking which invoices have already been reviewed, marking the checkbox and painting the row green.

- Form: [code: Frm_Audit_Values.cs](https://github.com/raphaeldertinatti/desktop_application/blob/main/Forms/Frm_Audit_Values.cs)
- Sql Procedure: [code: sp_Conf_Values.sql](https://github.com/raphaeldertinatti/desktop_application/blob/main/SQL/sp_Conf_Values.sql)

